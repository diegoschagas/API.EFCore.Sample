using Microsoft.Extensions.Configuration;
using EFCore.Sample.Business.Interfaces;

namespace EFCore.Sample.Business.Models.Safe2Pay
{
    public class Safe2PayConfigManager : ISafe2PayConfigManager
    {
        private readonly IConfiguration _configuration;
        public Safe2PayConfigManager(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string UrlTransactionList => this._configuration["Safe2Pay:UrlTransactionList"];

        public string ApiKey => this._configuration["Safe2Pay:ApiKey"];

        public string UrlReenviarNotificacao => this._configuration["Safe2Pay:UrlReenviarNotificacao"];

        public string UrlAquisicaoCredito => this._configuration["Safe2Pay:UrlAquisicaoCredito"];

        public string UrlCallBackNotifyV2 => this._configuration["Safe2Pay:UrlCallBackNotifyV2"];
        public string UrlAtualizarCallBackUrl => this._configuration["Safe2Pay:UrlAtualizarCallBackUrl"];
    }
}
