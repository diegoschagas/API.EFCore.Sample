using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EFCore.Sample.Api.ViewModels.Safe2PayViewModel
{
    public class ContasReceberJsonFileViewModel
    {
        public IFormFile JsonUpload { get; set; }

        public string Json { get; set; }
    }
}
