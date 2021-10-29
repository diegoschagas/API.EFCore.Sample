using EFCore.Sample.Business.Models;
using EFCore.Sample.Business.Models.Safe2Pay;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.Sample.Business.Interfaces
{
    public interface ISafePayRepository
    {
        Task<TransactionResult> GetClientByDocument(string document);
        Task<ReenviarNotificacao> ReenviarNotificacao(int idTransaction);
        Task AdicionarReferencia(List<TransactionReference> reference);
        Task<IEnumerable<TransactionReference>> GetAllReferences();
        Task<IEnumerable<TransactionResponse>> UpdateCallBack(IEnumerable<TransactionReference> model);
        Task AdicionarTransactionsFromJson(List<TransactionReference> listTransactions);
    }
}
