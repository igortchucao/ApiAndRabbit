using RabbitMQ.Client;

namespace Teste.Repositorio.Service
{
    public interface IEnviarMensagem
    {
        void EnviarMensagemRabbit(IModel channel, string mensagem);
    }
}