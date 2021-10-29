using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Models;
using EFCore.Sample.Business.Models.Safe2Pay;
using EFCore.Sample.ExternalApi.Extension;
using EFCore.Sample.ORM.Context;
using EFCore.Sample.ORM.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.Sample.ExternalApi.Repository
{
    public class SafePayRepository : RepositoryMemory<TransactionReference>, ISafePayRepository
    {
        private readonly ISafe2PayConfigManager _safe2PayConfigManager;

        public SafePayRepository(ISafe2PayConfigManager safe2PayConfigManager, Sped_SafewebContextMemory db) : base(db)
        {
            _safe2PayConfigManager = safe2PayConfigManager;
        }

        public async Task AdicionarReferencia(List<TransactionReference> listReference)
        {
            Db.AddRange(listReference);

            await Db.SaveChangesAsync();
        }

        public async Task AdicionarTransactionsFromJson(List<TransactionReference> listTransactions)
        {
            Db.AddRange(listTransactions);

            await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TransactionReference>> GetAllReferences()
        {
            return await Db.TransactionReference.AsNoTracking().ToListAsync();
        }

        public async Task<TransactionResult> GetClientByDocument(string document)
        {
            string cnpjCpf = document == string.Empty ? "09546385000161" : document;  //"03264853000129" : document;

            string url = $"{_safe2PayConfigManager.UrlTransactionList}{cnpjCpf}";

            var request = new ServiceRequest<string>(_safe2PayConfigManager);

            var json = await request.Get(url);

            var response = JsonConvert.DeserializeObject<TransactionResult>(json);

            return response;
        }

        public async Task<ReenviarNotificacao> ReenviarNotificacao(int idTransaction)
        {
            string url = $"{_safe2PayConfigManager.UrlReenviarNotificacao}{idTransaction}";

            var data = new ReenviarNotificacaoRequest
            {
                IdTransaction = idTransaction
            };

            var request = new ServiceRequest<ReenviarNotificacaoRequest>(_safe2PayConfigManager);

            var json = await request.Post(url, data);

            var response = JsonConvert.DeserializeObject<ReenviarNotificacao>(json);
            
            return response;
        }

        public async Task<IEnumerable<TransactionResponse>> UpdateCallBack(IEnumerable<TransactionReference> collection)
        {
                string url = $"{_safe2PayConfigManager.UrlAtualizarCallBackUrl}";

                var request = new ServiceRequest<ReenviarNotificacaoRequest>(_safe2PayConfigManager);

                List<TransactionResponse> transactionResponse = new List<TransactionResponse>();

                foreach (var transaction in collection)
                {
                    var data = new AtualizarCallBackUrlRequest
                    {
                        Id = transaction.IdTransaction,
                        CallBackUrl = transaction.CallBackUrl,
                        Reference = transaction.ReferenceDescription
                    };
                    var json = await request.Put(url, data);

                    var response = JsonConvert.DeserializeObject<TransactionResponse>(json);

                    transactionResponse.Add(response);
                }

                return transactionResponse;
        }

    }
}
