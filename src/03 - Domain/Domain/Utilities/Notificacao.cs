namespace Application.Utilities
{
    public class Notificacao
    {
        public EnumTipoNotificacao StatusCode { get; set; } = EnumTipoNotificacao.Informacao;
        public string Descricao { get; set; }
        public Notificacao(EnumTipoNotificacao tipo, string mensagem)
        {
            StatusCode = tipo;
            Descricao = mensagem;
        }
    }

    public enum EnumTipoNotificacao
    {
        Informacao = 200,
        ClientError = 400,
        ServerError = 500,
    }
}
