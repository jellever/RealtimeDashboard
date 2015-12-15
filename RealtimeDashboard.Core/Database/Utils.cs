using System;
using System.Collections.Generic;
using System.Data.Entity;
using RealtimeDashboard.Core.ServiceBus.Protocol;
using RealtimeDashboard.Database.Models;

namespace RealtimeDashboard.Core.Database
{
    public static class Utils
    {
        private static EntityEvent.EntityEventType GetEntityEventType(EntityState state)
        {
            switch (state)
            {
                case EntityState.Added:
                    return EntityEvent.EntityEventType.ADD;
                case EntityState.Deleted:
                    return EntityEvent.EntityEventType.DELETE;
                case EntityState.Modified:
                    return EntityEvent.EntityEventType.UPDATE;
            }
            return EntityEvent.EntityEventType.UNKNWON;
        }

        public static EntityEvent CreateEntityEvent(EntityState state, IEntityNotifyChanged entity)
        {
            EntityEvent.EntityEventType eventType = GetEntityEventType(state);
            Type entityType = entity.GetType();
            Int64 entityId = entity.GetID();
            List<RealtimeDashboard.Database.Models.RelatedEntityInfo> relatedEntityInfo = entity.GetRelatedEntityInfo();
            EntityEvent entityEvent = ServiceBusProtocolUtils.CreatEntityEvent(entityId, entityType.Name, eventType, relatedEntityInfo);
            return entityEvent;
        }

        public static string GetEntityEventName(EntityState state, IEntityNotifyChanged entity)
        {
            EntityEvent.EntityEventType eventType = GetEntityEventType(state);
            Type entityType = entity.GetType();
            Int64 entityId = entity.GetID();
            string eventObjTag = $"EntityEvents.{entityType.Name}.{eventType}.{entityId}";
            return eventObjTag;
        }
    }
}
