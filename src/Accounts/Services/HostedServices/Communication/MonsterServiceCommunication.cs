using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Services.HostedServices.Communication;
using Accounts.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Accounts.Services.HostedServices.Communication
{
    public class MonsterServiceCommunication : BackgroundService, IMonsterServiceCommunication
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MonsterServiceCommunication(IOptions<RabbitMQSettings> settings)
        {
            var factory = new ConnectionFactory
            {
                HostName = settings.Value.Host,
                Port = settings.Value.Port,
                UserName = settings.Value.Username,
                Password = settings.Value.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(MessageExchanges.USER_CREATED, "fanout");
        }

        public void UserCreated(int userId)
        {
            var message = Encoding.UTF8.GetBytes(userId.ToString());

            _channel.BasicPublish(exchange: MessageExchanges.USER_CREATED,
                routingKey: String.Empty,
                basicProperties: null,
                body: message);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();

            base.Dispose();
        }

        private void InitializeExchange(RabbitMQSettings settings, string exchangeName)
        {
            using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(settings.Username, settings.Password) })
            using (var client = new HttpClient(handler))
            {
                var result = client.GetAsync($"http://{settings.Host}:{settings.Port}/api/exchanges").Result;

                var data = result.Content.ReadAsAsync<object[]>().Result;
            }
        }
    }
}
