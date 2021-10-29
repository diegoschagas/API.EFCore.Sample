using System.Text.Json.Serialization;

namespace EFCore.Sample.Business.Models.Safe2Pay
{

    public class TransactionResponse
    {
        [JsonPropertyName("Responsedetail")]
        public ResponseDetail ResponseDetail { get; set; }
        public bool HasError { get; set; }
    }
}