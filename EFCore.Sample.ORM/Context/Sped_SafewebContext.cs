using Microsoft.EntityFrameworkCore;
using EFCore.Sample.Business.Models;

#nullable disable

namespace EFCore.Sample.ORM.Context
{
    public partial class Sped_SafewebContext : DbContext
    {
        public Sped_SafewebContext()
        {
        }

        public Sped_SafewebContext(DbContextOptions<Sped_SafewebContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ContasReceber> ContasRecebers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Sped_SafewebContext).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
