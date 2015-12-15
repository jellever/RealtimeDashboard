using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RealtimeDashboard.Core.General;
using RealtimeDashboard.Core.ServiceBus.Protocol;

namespace RealtimeDashboard.Core.Database.EntityEvents
{
    public class EntityEventsProvider : IEntityEventsProvider
    {
        private const string RabbitMQExchangeName = "RD.EntityEvents.TopicExchange";
        private const string RabbitMQQueueName = "RD.Database.EntityEventsQueue";

        private ConnectionFactory rabbitMqFactory;
        private IConnection rabbitMqConnection;
        private IModel rabbitMqChannel;
        private EventingBasicConsumer rabbitMqConsumer;
        private string consumerTag;

        public event EventHandler<EntityEventArgs> EntityChange;

        public EntityEventsProvider()
        {
            InitializeRabbitMq();
        }

        public void Start()
        {
            consumerTag = rabbitMqChannel.BasicConsume(RabbitMQQueueName, true, rabbitMqConsumer);
        }

        public void Stop()
        {
            rabbitMqChannel.BasicCancel(consumerTag);
        }

        private void InitializeRabbitMq()
        {
            rabbitMqFactory = new ConnectionFactory();
            rabbitMqFactory.HostName = "localhost";
            rabbitMqFactory.AutomaticRecoveryEnabled = true;

            rabbitMqConnection = rabbitMqFactory.CreateConnection();
            rabbitMqChannel = rabbitMqConnection.CreateModel();
            rabbitMqChannel.BasicQos(0, 1, true);

            rabbitMqChannel.ExchangeDeclare(RabbitMQExchangeName, ExchangeType.Topic, true);
            rabbitMqChannel.QueueDeclare(RabbitMQQueueName, true, false, false, null);
            rabbitMqChannel.QueueBind(RabbitMQQueueName, RabbitMQExchangeName, "EntityEvents.#");

            rabbitMqConsumer = new EventingBasicConsumer(rabbitMqChannel);
            rabbitMqConsumer.Received += RabbitMqConsumer_Received;
        }

        protected void OnEntityChange(EntityEventArgs e)
        {
            if (EntityChange != null)
                EntityChange(this, e);
        }

        private void RabbitMqConsumer_Received(object sender, BasicDeliverEventArgs e)
        {
            byte[] msgBytes = e.Body;
            EntityEvent entityEvent = ProtoUtils.Deserialize<EntityEvent>(msgBytes);
            OnEntityChange(new EntityEventArgs(entityEvent));
        }

        public void Dispose()
        {
            rabbitMqConnection.Dispose();
            rabbitMqChannel.Dispose();
        }
    }
}
