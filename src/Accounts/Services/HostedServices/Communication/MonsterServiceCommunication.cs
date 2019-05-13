using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Accounts.Services.HostedServices.Communication.Models;
using Accounts.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace Accounts.Services.HostedServices.Communication
{
    public class MonsterServiceCommunication : BackgroundService, IMonsterServiceCommunication
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MonsterServiceCommunication(IOptions<RabbitMQSettings> settings)
        {
            Log.Information("Instanciating Hosted Service @{HostedService}", nameof(MonsterServiceCommunication));

            var factory = new ConnectionFactory
            {
                HostName = settings.Value.Host,
                Port = settings.Value.Port,
                UserName = settings.Value.Username,
                Password = settings.Value.Password
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            InitializeExchange(settings.Value, MessageExchanges.USER_CREATED);
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
            Log.Information("Launching Hosted Service @{HostedService}", nameof(MonsterServiceCommunication));

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
                var result = client.GetAsync($"http://{settings.Host}:{settings.ApiPort}/api/exchanges").Result;

                var data = JsonConvert.DeserializeObject<List<Exchange>>(result.Content.ReadAsStringAsync().Result);

                if (data.Any(x => x.Name == exchangeName) == false)
                {
                    Log.Information("Initializing exchange @{ExchangeName}", exchangeName);

                    _channel.ExchangeDeclare(MessageExchanges.USER_CREATED, "fanout");
                }
            }
        }
    }
}
