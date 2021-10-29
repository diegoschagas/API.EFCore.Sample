using Newtonsoft.Json;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using EFCore.Sample.Business.Enum;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Models;
using EFCore.Sample.Business.Models.Safe2Pay;
using EFCore.Sample.ExternalApi.Extensions;
using EFCore.Sample.ExternalApi.Services.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFCore.Sample.ExternalApi.Services
{
    public class SafePayService : BaseService, ISafePayService
    {
        private ISafePayRepository _safePayRepository;
        private AsyncRetryPolicy _retryPolicy;
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public SafePayService(ISafePayRepository safePayRepository, INotificator notificator) : base(notificator)
        {
            _safePayRepository = safePayRepository;
            _retryPolicy = Policy
               .Handle<Exception>()
               .WaitAndRetryAsync(2, retryAttempt =>
               {
                   var timeToWait = TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
                   Console.WriteLine($"Waiting {timeToWait.TotalSeconds} seconds");
                   return timeToWait;
               }
               );

            _circuitBreakerPolicy = Policy.Handle<Exception>()
                .CircuitBreakerAsync(1, TimeSpan.FromMinutes(1),
                (ex, t) =>
                {
                    Console.WriteLine("Circuit broken!");
                },
                () =>
                {
                    Console.WriteLine("Circuit Reset!");
                });
        }

        public async Task<TransactionResult> GetClientByDocument(string document)
        {
            Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
            return await _retryPolicy.ExecuteAsync<TransactionResult>
                (
                async () => await _safePayRepository.GetClientByDocument(document)
                );

        }

        public async Task<ReenviarNotificacao> ReenviarNotificacao(int idTransaction)
        {
            Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");
            return await _retryPolicy.ExecuteAsync<ReenviarNotificacao>
                (
                async () => await _safePayRepository.ReenviarNotificacao(idTransaction)
                );
        }

        public async Task<IEnumerable<TransactionResponse>> AtualizarReferencia(IEnumerable<TransactionReference> collection)
        {
            Console.WriteLine($"Circuit State: {_circuitBreakerPolicy.CircuitState}");

            return await _retryPolicy.ExecuteAsync<IEnumerable<TransactionResponse>>
                (
                async () => await _safePayRepository.UpdateCallBack(collection)
                );

        }
        public async Task<IEnumerable<TransactionReference>> AdicionarListaReferencia(Business.Models.Safe2Pay.Object[] model)
        {
            //TODO Refactoring para utilizar FluentValidation com Dependency Inject na Classe de modelo.
            TransactionReference reference;
            List<TransactionReference> listReference = new List<TransactionReference>();
            ReferenceType referenceType = ReferenceType.ENOTA;
            foreach (var item in model)
            {
                if (!Validations.VerifyCallBack(item.CallbackUrl) && Validations.VerifyReference(item.Reference, out referenceType)) { continue; }
                reference = new TransactionReference()
                {
                    IdTransaction = item.IdTransaction,
                    Document = item.Customer.Identity,
                    CallBackUrl = item.CallbackUrl,
                    ReferenceDescription = Validations.FormatReferenceDescription(item.Reference, item.Customer.Identity),
                    ReferenceType = referenceType,
                    YearMonth = "202109"
                };
                listReference.Add(reference);
            }

            await _safePayRepository.AdicionarReferencia(listReference);

            return await Task.FromResult(listReference);
        }


        public async Task<IEnumerable<TransactionReference>> GetAll()
        {
            return await _safePayRepository.GetAllReferences();
        }

        public async Task<IEnumerable<TransactionResponse>> AjustaReferenciaContasReceber()
        {
            var responseReferenceFromJson = _safePayRepository.GetAllReferences();

            IEnumerable<TransactionResponse> responseAtualizarReferencia = null;

            if (responseReferenceFromJson.Result.Count() > 0)
            {
                responseAtualizarReferencia = AtualizarReferencia(responseReferenceFromJson.Result).Result;
            }
            return await Task.FromResult(responseAtualizarReferencia);

        }

        public async Task<IEnumerable<TransactionResponse>> AjustaReferenciaContasReceberByDocument(string document)
        {
            var responseDocumentos = GetClientByDocument(document).Result?.ResponseDetail?.Objects;

            var responseReference = AdicionarListaReferencia(responseDocumentos).Result;

            var responseAtualizarReferencia = AtualizarReferencia(responseReference).Result;

            return await Task.FromResult(responseAtualizarReferencia);

        }


        public async Task<IEnumerable<TransactionReference>> AddContasReceberJsonFile(ContasReceberJsonFile model)
        {

            string fileContent = string.Empty;

            List<TransactionReference> listTransactions = new List<TransactionReference>();
            var transaction = new TransactionReference();
            bool matchIdTransaction = false;
            bool matchCallbackUrl = false;
            bool matchReference = false;
            bool matchIdentity = false;

            string stringCallBackUrl;
            string line;

            var callBackPattern = @"[,""><|]+";

            Regex regex = new Regex(@"[\W_]+");
            
            ReferenceType referenceType = ReferenceType.ENOTA;

            using (var sr = new StreamReader(model.JsonUpload.OpenReadStream()))
            {
                while ((line = sr.ReadLine()) != null)
                {

                    if (line.Contains("IdTransaction"))
                    {
                        Int32.TryParse(regex.Replace(line.Replace("IdTransaction", ""), "").ToUpper(), out int transactionClient);

                        transaction.IdTransaction = transactionClient;

                        if (18660100 == transaction.IdTransaction)
                            break;

                        matchIdTransaction = true;
                    }

                    if (line.Contains("CallbackUrl"))
                    {
                        stringCallBackUrl = Regex.Replace(line.Replace("CallbackUrl", ""), callBackPattern, "");

                        var found = stringCallBackUrl.IndexOf(":");
                        transaction.CallBackUrl = stringCallBackUrl.Substring(found + 2);
                        matchCallbackUrl = true;
                    }

                    if (line.Contains("Reference"))
                    {
                        transaction.ReferenceDescription = regex.Replace(line.Replace("Reference", ""), "").ToUpper();
                        matchReference = true;
                    }

                    if (line.Contains("Identity") && !line.Contains("GuarantorIdentity"))
                    {
                        transaction.Document = regex.Replace(line.Replace("Identity", ""), "").ToUpper();
                        matchIdentity = true;

                    }

                    if (matchIdTransaction && matchCallbackUrl && matchReference && matchIdentity)
                    {
                        matchIdTransaction = false;
                        matchCallbackUrl = false;
                        matchReference = false;
                        matchIdentity = false;

                        if (!Validations.VerifyCallBack(transaction.CallBackUrl)) { continue; }

                        Validations.VerifyReference(transaction.ReferenceDescription, out referenceType);

                        transaction.YearMonth = "202109";
                        transaction.ReferenceDescription = Validations.FormatReferenceDescription(transaction.ReferenceDescription, transaction.Document);
                        transaction.ReferenceType = referenceType;
                        
                        listTransactions.Add(transaction);
                        transaction = new TransactionReference();

                    }

                    System.Console.WriteLine(line);
                }
            }
            await _safePayRepository.AdicionarTransactionsFromJson(listTransactions);

            return await Task.FromResult(listTransactions);
        }

    }
}
