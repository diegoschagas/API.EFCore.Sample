namespace EFCore.Sample.Business.Interfaces
{
    public interface ISafe2PayConfigManager
    {
        string UrlTransactionList { get; }
        string UrlReenviarNotificacao { get; }
        string UrlAquisicaoCredito { get; }

        string UrlCallBackNotifyV2 { get; }
        string UrlAtualizarCallBackUrl { get; }

        string ApiKey { get; }
    }
}
