using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Teste.Aplicacao.Service;

namespace Teste.Aplicacao.Rabbit;

public class RabbitMessage : BaseRota, IRabbitMessage
{
    public RabbitMessage(IConfiguration configuration) : base(configuration)
    {
    }

    public async Task<IActionResult> PreencherListaRabbit()
    {
        var envia = new EnviarMensagem();

        try
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhsot",
                Port = 5672,
                UserName = "admin",
                Password = "123456",
                VirtualHost = "/",
                RequestedHeartbeat = new TimeSpan(60)
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "rabbitMensagesQueue",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                envia.EnviarMensagemRabbit(channel, "343.228.350-40");
                envia.EnviarMensagemRabbit(channel, "869.230.000-41");
                envia.EnviarMensagemRabbit(channel, "568.946.870-30");
                envia.EnviarMensagemRabbit(channel, "433.510.120-12");
                envia.EnviarMensagemRabbit(channel, "415.022.590-79");
            }

            return new OkObjectResult("Lista rabbitMensagesQueue preenchida!");

        }catch(Exception e)
        {
            return new BadRequestObjectResult("Erro ao acessar Rabbit: " + e.Message);
        }
    }

    public async Task<IActionResult> LerListaRabbit()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };
        var mensagem = string.Empty;

        try
        {
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare("rabbitMensagesQueue", exclusive: false);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    mensagem = Encoding.UTF8.GetString(body);
                };

                channel.BasicConsume(queue: "rabbitMensagesQueue", autoAck: true, consumer: consumer);
            }

            return new OkObjectResult(mensagem);

        } catch (Exception e)
        {
            return new BadRequestObjectResult("Erro ao ler lista do Rabbit: " + e.Message);
        }
    }
}
