using System;
using RealtimeDashboard.Core.ServiceBus.Protocol;

namespace RealtimeDashboard.Core.Database.EntityEvents
{
    public class EntityEventArgs : EventArgs
    {
        public EntityEvent EntityEventData { get; protected set; }

        public EntityEventArgs(EntityEvent evtData)
        {
            this.EntityEventData = evtData;
        }
    }
}
