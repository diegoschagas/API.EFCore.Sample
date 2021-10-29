namespace EFCore.Sample.Business.Notifications
{
    public class Notification
    {
        public string mensagem { get; }

        public Notification(string mensagem)
        {
            this.mensagem = mensagem;
        }
    }
    
}
