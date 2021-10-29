using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Sample.Api.ViewModels.Safe2PayViewModel
{
    public class TransactionReferenceViewModel
    {
        [Key]
        public int IdTransaction { get; set; }

        public string Document { get; set; }

        public string CallBackUrl { get; set; }

        public string ReferenceDescription { get; set; }

        public string YearMonth { get; set; }
        public ReferenceType ReferenceType { get; set; }
    }
}
