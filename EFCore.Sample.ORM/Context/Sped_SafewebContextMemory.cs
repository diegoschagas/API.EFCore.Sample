using Microsoft.EntityFrameworkCore;
using EFCore.Sample.Business.Models;

namespace EFCore.Sample.ORM.Context
{
    public partial class Sped_SafewebContextMemory : DbContext
    {
        public Sped_SafewebContextMemory() { }

        public Sped_SafewebContextMemory(DbContextOptions<Sped_SafewebContextMemory> options)
          : base(options)
        { }

        public DbSet<TransactionReference> TransactionReference { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Sped_SafewebContextMemory).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
