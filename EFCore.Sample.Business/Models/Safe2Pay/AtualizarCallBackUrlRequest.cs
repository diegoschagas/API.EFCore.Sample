namespace EFCore.Sample.Business.Models.Safe2Pay
{
    public class AtualizarCallBackUrlRequest
    {
        public int Id { get; set; }
        public string CallBackUrl { get; set; }

        public string Reference { get; set; }
    }
}
