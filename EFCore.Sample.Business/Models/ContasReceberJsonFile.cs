using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;

namespace EFCore.Sample.Business.Models
{
    public class ContasReceberJsonFile 
    {
        public ContasReceberJsonFile()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public IFormFile JsonUpload { get; set; }

        public string Json { get; set; }

        public string IdTransaction { get; set; }

        public string CallbackUrl { get; set; }

        public string Reference { get; set; }

    }
}
