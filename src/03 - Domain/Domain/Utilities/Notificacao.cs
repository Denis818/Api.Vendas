namespace Application.Utilities
{
    public class Notificacao(string mensagem, EnumTipoNotificacao tipo = EnumTipoNotificacao.Informacao)
    {
        public EnumTipoNotificacao StatusCode { get; set; } = tipo;
        public string Descricao { get; set; } = mensagem;
    }

    public enum EnumTipoNotificacao
    {
        Informacao = 200,
        ClientError = 400,
        ServerError = 500,
    }
}
