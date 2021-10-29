using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.Sample.Business.Models;

namespace EFCore.Sample.ORM.Mappings
{
    class TransactionReferenceMapping : IEntityTypeConfiguration<TransactionReference>
    {
        public void Configure(EntityTypeBuilder<TransactionReference> entity)
        {
            entity.Property(e => e.ReferenceType)
                 .HasConversion<string>();

        }
    }
}
