using System.Text.Json.Serialization;

namespace EFCore.Sample.Business.Models.Safe2Pay
{

    public class TransactionResult
    {
        [JsonPropertyName("Responsedetail")]
        public ResponseDetailObject ResponseDetail { get; set; }
        public bool HasError { get; set; }
    }
}