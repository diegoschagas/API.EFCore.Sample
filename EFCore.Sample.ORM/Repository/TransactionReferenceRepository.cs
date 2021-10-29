using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Models;
using EFCore.Sample.ORM.Context;

namespace EFCore.Sample.ORM.Repository
{
    public class TransactionReferenceRepository : RepositoryMemory<TransactionReference>, IReferenceRepository
    {
        public TransactionReferenceRepository(Sped_SafewebContextMemory db) : base(db)
        {
        }
    }
}
