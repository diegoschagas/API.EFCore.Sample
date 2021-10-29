using Microsoft.EntityFrameworkCore;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Models;
using EFCore.Sample.ORM.Context;
using System.Threading.Tasks;

namespace EFCore.Sample.ORM.Repository
{
    public class ContasReceberRepository : Repository<ContasReceber>, IContasReceberRepository
    {
        public ContasReceberRepository(Sped_SafewebContext db) : base(db)
        {
        }

        public async Task<ContasReceber> GetByDocumento(string documento)
        {
            return await Db.ContasRecebers.AsNoTracking().FirstOrDefaultAsync(p => p.CnpjCpf.Equals(documento));
        }

    }
}
