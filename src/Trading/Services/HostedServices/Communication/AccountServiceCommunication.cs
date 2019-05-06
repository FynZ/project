using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Trading.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace Trading.Services.HostedServices.Communication
{
    public class AccountServiceCommunication : BackgroundService, IAccountServiceCommunication
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private EventingBasicConsumer _consumer;

        private readonly IMonsterIniter _monsterIniter;

        public AccountServiceCommunication(IMonsterIniter monsterIniter, IOptions<RabbitMQSettings> settings)
        {
            Log.Information("Instanciating Hosted Service @{HostedService}", nameof(AccountServiceCommunication));

            _monsterIniter = monsterIniter;

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

            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: _queueName,
                exchange: MessageExchanges.USER_CREATED,
                routingKey: "");
        }

        private void UserCreated(object model, BasicDeliverEventArgs eventArgs)
        {
            if (Int32.TryParse(Encoding.UTF8.GetString(eventArgs.Body), out int userId))
            {
                if (_monsterIniter.InitUser(userId))
                {
                    Log.Information("Successfully inited user @{UserId}", userId);
                }
                else
                {
                    Log.Error("Unable to init user @{UserId}", userId);
                }
            }
            else
            {
                Log.Error("Unable to parse message body @{MessageBody} to Int32", eventArgs.Body);
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Log.Information("Launching Hosted Service @{HostedService}", nameof(AccountServiceCommunication));

            stoppingToken.ThrowIfCancellationRequested();

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += UserCreated;

            _channel.BasicConsume(queue: _queueName,
                autoAck: true,
                consumer: _consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();

            base.Dispose();
        }
    }
}