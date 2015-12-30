using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Microsoft.AspNet.SignalR;
using RealtimeDashboard.Core.Database.EntityEvents;
using RealtimeDashboard.Core.Logging;
using RealtimeDashboard.Core.ServiceBus.Protocol;
using RealtimeDashboard.Database;
using RealtimeDashboard.Database.Models;
using RealtimeDashboard.Server.Database;
using RelatedEntityInfo = RealtimeDashboard.Core.ServiceBus.Protocol.RelatedEntityInfo;

namespace RealtimeDashboard.Hubs
{
    public class LiveHubController : IRegisteredObject
    {
        private Ilog log;

        private IHubContext hub;
        private IUnitOfWork unitOfWork;
        private IEntityEventsProvider entityChangeProvider;

        public LiveHubController(Ilog log)
        {
            this.log = log;
            HostingEnvironment.RegisterObject(this);
            this.unitOfWork = new EFUnitOfWork(new DatabaseContext());
            this.hub = GlobalHost.ConnectionManager.GetHubContext<LiveHub>();
            this.entityChangeProvider = new EntityEventsProvider();

            this.entityChangeProvider.EntityChange += EntityChangeProvider_EntityChange;
            this.entityChangeProvider.Start();
        }

        private void EntityChangeProvider_EntityChange(object sender, EntityEventArgs e)
        {
            ProcessEntityChange(e.EntityEventData);
        }

        protected async Task<object> GetEntity(string type, Int64 id)
        {
            object entity = null;
            switch (type)
            {
                case nameof(ChatMessage):
                    entity = await unitOfWork.ChatMessageRepository.Get(id);
                    break;
                case nameof(ChatRoom):
                    entity = await unitOfWork.ChatRoomRepository.Get(id);
                    break;
            }
            return entity;
        }

        protected async void ProcessEntityChange(EntityEvent entityEvent)
        {
            object entity = await GetEntity(entityEvent.EntityType, entityEvent.EntityId);

            List<string> groups = new List<string>();
            groups.Add(entityEvent.EntityType);
            groups.Add($"{entityEvent.EntityType}.{entityEvent.EntityId}");
            foreach (RelatedEntityInfo relatedEntityInfo in entityEvent.RelatedEntityInfo)
            {
                groups.Add($"{relatedEntityInfo.TypeName}.{relatedEntityInfo.EntityId}.{relatedEntityInfo.RelationName}");
            }

            switch (entityEvent.EventType)
            {
                case EntityEvent.EntityEventType.ADD:
                    PublishSignalrNewEntityEvent(groups, entityEvent.EntityType, entityEvent.EntityId, entity);
                    break;
                case EntityEvent.EntityEventType.UPDATE:
                    PublishSignalrUpdatedEntityEvent(groups, entityEvent.EntityType, entityEvent.EntityId, entity);
                    break;
                case EntityEvent.EntityEventType.DELETE:
                    PublishSignalrDeletedEntityEvent(groups, entityEvent.EntityType, entityEvent.EntityId, entity);
                    break;
            }
        }

        protected void PublishSignalrNewEntityEvent(List<string> groups, string entityName, Int64 entityId, object data)
        {
            foreach (string @group in groups)
            {
                log.WriteLine(LogLevel.Info, nameof(LiveHubController), $"Broadcasting SignalR NewEntity event to group {group}");
                hub.Clients.Group(group).NewEntity(new
                {
                    Group = group,
                    Type = entityName,
                    Id = entityId,
                    Entity = data
                });
            }
        }

        protected void PublishSignalrUpdatedEntityEvent(List<string> groups, string entityName, Int64 entityId, object data)
        {
            foreach (string @group in groups)
            {
                log.WriteLine(LogLevel.Info, nameof(LiveHubController), $"Broadcasting SignalR UpdatedEntity event to group {group}");
                hub.Clients.Group(group).UpdatedEntity(new
                {
                    Group = group,
                    Type = entityName,
                    Id = entityId,
                    Entity = data
                });
            }
        }

        protected void PublishSignalrDeletedEntityEvent(List<string> groups, string entityName, Int64 entityId, object data)
        {
            foreach (string @group in groups)
            {
                log.WriteLine(LogLevel.Info, nameof(LiveHubController), $"Broadcasting SignalR DeletedEntity event to group {group}");
                hub.Clients.Group(group).DeletedEntity(new
                {
                    Group = group,
                    Type = entityName,
                    Id = entityId,
                    Entity = data
                });
            }
        }

        public void Stop(bool immediate)
        {
            HostingEnvironment.UnregisterObject(this);
            entityChangeProvider.Stop();
        }
    }
}