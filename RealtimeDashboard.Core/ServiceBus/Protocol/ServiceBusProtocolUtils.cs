using System;
using System.Collections.Generic;
using System.Linq;

namespace RealtimeDashboard.Core.ServiceBus.Protocol
{
    public static class ServiceBusProtocolUtils
    {
        public static EntityEvent CreatEntityEvent<T>(Int64 id, EntityEvent.EntityEventType eventType, List<RealtimeDashboard.Database.Models.RelatedEntityInfo> relatedEntityInfo )
        {
            string typeName = nameof(T);
            List<RelatedEntityInfo> convertedEntityInfo = relatedEntityInfo.Select(x => new RelatedEntityInfo()
            {
                TypeName = x.TypeName,
                RelationName = x.RelationName,
                EntityId = x.Id
            }).ToList();
            EntityEvent result =  new EntityEvent()
            {
                EntityId = id,
                EntityType = typeName,
                EventType = eventType,
                EventTime = DateTime.UtcNow.ToFileTime()
            };
            result.RelatedEntityInfo.AddRange(convertedEntityInfo);
            return result;
        }

        public static EntityEvent CreatEntityEvent(Int64 id, string entityType, EntityEvent.EntityEventType eventType, List<RealtimeDashboard.Database.Models.RelatedEntityInfo> relatedEntityInfo)
        {
            List<RelatedEntityInfo> convertedEntityInfo = relatedEntityInfo.Select(x => new RelatedEntityInfo()
            {
                TypeName = x.TypeName,
                RelationName = x.RelationName,
                EntityId = x.Id
            }).ToList();
            EntityEvent result = new EntityEvent()
            {
                EntityId = id,
                EntityType = entityType,
                EventType = eventType,
                EventTime = DateTime.UtcNow.ToFileTime()
            };
            result.RelatedEntityInfo.AddRange(convertedEntityInfo);
            return result;
        }
    }
}
