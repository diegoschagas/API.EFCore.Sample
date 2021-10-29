using EFCore.Sample.Business.Models.Safe2Pay;

namespace EFCore.Sample.Api.ViewModels.Safe2PayViewModel
{
    public class TransactionResponseViewModel
    {
        public ResponseDetail ResponseDetail { get; set; }
        public bool HasError { get; set; }

    }
}
