using EFCore.Sample.Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.Sample.Business.Interfaces
{
    public interface IContasReceberRepository : IRepository<ContasReceber>
    {
        Task<ContasReceber> GetByDocumento(string documento);
        
    }
}
