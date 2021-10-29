using EFCore.Sample.Business.Enum;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Sample.Business.Models
{
    public class TransactionReference
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
