using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFCore.Sample.Business.Models;

namespace EFCore.Sample.ORM.Mappings
{
    class ContasReceberMapping : IEntityTypeConfiguration<ContasReceber>
    {
        public void Configure(EntityTypeBuilder<ContasReceber> entity)
        {
            entity.HasKey(e => e.CodContaReceber);

            entity.ToTable("ContasReceber", "mco");

            entity.Property(e => e.CodContaReceber).HasColumnName("codContaReceber");

            entity.Property(e => e.CnpjCpf)
                .HasMaxLength(14)
                .HasColumnName("Cnpj_Cpf");

            entity.Property(e => e.CodBanco).HasColumnName("codBanco");

            entity.Property(e => e.CodDiario).HasColumnName("codDiario");

            entity.Property(e => e.CodFormaPagto).HasColumnName("codFormaPagto");

            entity.Property(e => e.CodNfe).HasColumnName("codNFe");

            entity.Property(e => e.CodPlConta).HasColumnName("codPlConta");

            entity.Property(e => e.DocPagto)
                .HasMaxLength(60)
                .HasColumnName("Doc_Pagto");

            entity.Property(e => e.DtMovimento)
                .HasColumnType("date")
                .HasColumnName("Dt_Movimento");

            entity.Property(e => e.DtPagamento)
                .HasColumnType("date")
                .HasColumnName("Dt_Pagamento");

            entity.Property(e => e.DtVencimento)
                .HasColumnType("date")
                .HasColumnName("Dt_Vencimento");

            entity.Property(e => e.Historico).HasMaxLength(500);

            entity.Property(e => e.IdTipoDocumento).HasColumnName("idTipoDocumento");

            entity.Property(e => e.NNf)
                .HasMaxLength(60)
                .HasColumnName("nNF");

            entity.Property(e => e.RsNome)
                .HasMaxLength(100)
                .HasColumnName("Rs_Nome");

            entity.Property(e => e.Serie).HasMaxLength(3);

            entity.Property(e => e.TipoPagamento)
                .HasMaxLength(150)
                .IsUnicode(false);

            entity.Property(e => e.Valor).HasColumnType("decimal(10, 2)");

            entity.Property(e => e.Vendedor).HasMaxLength(100);
        }
    }
}
