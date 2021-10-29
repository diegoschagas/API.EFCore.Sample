using System;

namespace EFCore.Sample.Api.ViewModels
{
    public class ContasReceberViewModel
    {
        public int CodContaReceber { get; set; }
        public int? CodDiario { get; set; }
        public int? CodBanco { get; set; }
        public int? CodFormaPagto { get; set; }
        public string CnpjCpf { get; set; }
        public string RsNome { get; set; }
        public string DocPagto { get; set; }
        public DateTime? DtMovimento { get; set; }
        public DateTime? DtVencimento { get; set; }
        public DateTime? DtPagamento { get; set; }
        public decimal? Valor { get; set; }
        public string Historico { get; set; }
        public bool? Editavel { get; set; }
        public string NNf { get; set; }
        public string Serie { get; set; }
        public int? CodNfe { get; set; }
        public string Vendedor { get; set; }
        public int? CodPlConta { get; set; }
        public int? IdTipoDocumento { get; set; }
        public int? IdTransaction { get; set; }
        public string TipoPagamento { get; set; }
    }
}
