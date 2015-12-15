using System;
using System.Collections.Generic;
using System.Data.Entity;
using RabbitMQ.Client;
using RealtimeDashboard.Core.General;
using RealtimeDashboard.Core.Logging;
using RealtimeDashboard.Core.ServiceBus.Protocol;
using RealtimeDashboard.Database.Models;
using RealtimeDashboard.Server.Database;

namespace RealtimeDashboard.Core.Database
{
    public class ChangeTrackingEFUnitOfWork : EFUnitOfWork
    {
        private const string RabbitMQExchangeName = "RD.EntityEvents.TopicExchange";
        private const string RabbitMQQueueName = "RD.Database.EntityEventsQueue";

        private object sync = new object();

        private ConnectionFactory rabbitMqFactory;
        private RabbitMQ.Client.IConnection rabbitMqConnection;
        private IModel rabbitMqChannel;

        protected Ilog log;

        public ChangeTrackingEFUnitOfWork(DbContext context, Ilog log)
            : base(context)
        {
            this.dbContext.Configuration.AutoDetectChangesEnabled = true;
            this.log = log;
            InitializeRabbitMq();
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
        }

        private List<KeyValuePair<EntityState, T>> GetChangedEntitiesByState<T>(List<EntityState> states) where T : class
        {
            List<KeyValuePair<EntityState, T>> result = new List<KeyValuePair<EntityState, T>>();

            var entities = dbContext.ChangeTracker.Entries<T>();
            foreach (var trackedEntity in entities)
            {
                if (states.Contains(trackedEntity.State))
                {
                    result.Add(new KeyValuePair<EntityState, T>(trackedEntity.State, trackedEntity.Entity));
                }
            }
            return result;
        }

        protected List<KeyValuePair<EntityState, IEntityNotifyChanged>> GetChanges()
        {
            List<EntityState> states = new List<EntityState>
            {
                EntityState.Added,
                EntityState.Deleted,
                EntityState.Modified
            };
            List<KeyValuePair<EntityState, IEntityNotifyChanged>> changedEntities = GetChangedEntitiesByState<IEntityNotifyChanged>(states);
            return changedEntities;
        }

        private void BroadcastEvent(IEntityNotifyChanged entity, EntityState entityState)
        {
            string eventName = Core.Database.Utils.GetEntityEventName(entityState, entity);
            EntityEvent entityEvent = Database.Utils.CreateEntityEvent(entityState, entity);
            byte[] eventBytes = ProtoUtils.Serialize(entityEvent);

            log.WriteLine(LogLevel.Debug, "ChangeTrackingEFUnitOfWork", $"Broadcasting event: {eventName}");
            rabbitMqChannel.BasicPublish(RabbitMQExchangeName, eventName, null, eventBytes);
        }
     
        protected void ProcessChanges(List<KeyValuePair<EntityState, IEntityNotifyChanged>> changes)
        {
            foreach (var change in changes)
            {
                if (change.Value.ShouldNotify())
                {
                    BroadcastEvent(change.Value, change.Key);
                }
            }
        }

        public override void Commit()
        {
            lock (sync)
            {
                List<KeyValuePair<EntityState, IEntityNotifyChanged>> changes = GetChanges();
                base.Commit();
                ProcessChanges(changes);
            }
        }
    }
}
