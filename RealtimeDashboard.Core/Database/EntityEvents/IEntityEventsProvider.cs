using System;

namespace RealtimeDashboard.Core.Database.EntityEvents
{
    public interface IEntityEventsProvider : IDisposable
    {
        event EventHandler<EntityEventArgs> EntityChange;

        void Start();
        void Stop();
    }
}
