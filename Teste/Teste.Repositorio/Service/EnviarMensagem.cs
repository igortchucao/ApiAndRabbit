using RabbitMQ.Client;
using System.Text;
using Teste.Repositorio.Service;

namespace Teste.Aplicacao.Service
{
    public class EnviarMensagem : IEnviarMensagem
    {
        public EnviarMensagem() {}

        public void EnviarMensagemRabbit(IModel channel, string mensagem)
        {
            var body = Encoding.UTF8.GetBytes(mensagem);

            channel.BasicPublish(exchange: "", routingKey: "rabbitMensagesQueue", basicProperties: null, body: body);
        }
    }
}
