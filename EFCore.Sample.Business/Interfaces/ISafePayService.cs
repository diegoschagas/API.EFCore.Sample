using EFCore.Sample.Business.Models;
using EFCore.Sample.Business.Models.Safe2Pay;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.Sample.Business.Interfaces
{
    public interface ISafePayService
    {
        Task<TransactionResult> GetClientByDocument(string documento);

        Task<ReenviarNotificacao> ReenviarNotificacao(int idTransaction);

        Task<IEnumerable<TransactionReference>> AdicionarListaReferencia(Business.Models.Safe2Pay.Object[] modelTransaction);
        Task<IEnumerable<TransactionReference>> GetAll();
        Task<IEnumerable<TransactionResponse>> AjustaReferenciaContasReceber();

        Task<IEnumerable<TransactionResponse>> AtualizarReferencia(IEnumerable<TransactionReference> collection);
        Task<IEnumerable<TransactionReference>> AddContasReceberJsonFile(ContasReceberJsonFile model);
    }
}
