using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using RealtimeDashboard.Core.Logging;

namespace RealtimeDashboard.Hubs
{
    public class LiveHub : Hub
    {
        private static readonly Dictionary<string, List<string>> connectionGroups;
        private static readonly object sync;
        private Ilog log;

        static LiveHub()
        {
            connectionGroups = new Dictionary<string, List<string>>();
            sync = new object();
        }

        public LiveHub()
        {
            log = new DebugLog();
        }

        public async Task<string> Subscribe(string type, int? id, string relationName)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            string groupName = type;
            if (id != null)
                groupName += $".{id}";
            if (relationName != null)
                groupName += $".{relationName}";
            await Subscribe(groupName);
            return groupName;
        }

        private Task Subscribe(string group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            log.WriteLine(LogLevel.Debug, nameof(LiveHub), $"Received subscribe call for group: {group}");
            lock (sync)
            {
                string conId = Context.ConnectionId;
                List<string> groups = null;
                if (connectionGroups.TryGetValue(conId, out groups))
                {
                    groups.Add(group);
                }
            }
            return Groups.Add(Context.ConnectionId, group);
        }

        public Task UnSubscribe(string group)
        {
            if (group == null)
                return Task.CompletedTask;
            lock (sync)
            {
                string conId = Context.ConnectionId;
                List<string> groups = null;
                if (connectionGroups.TryGetValue(conId, out groups))
                {
                    groups.Remove(group);
                }
            }
            return Groups.Remove(Context.ConnectionId, group);
        }

        public void ResetSubscribtions()
        {
            lock (sync)
            {
                string conId = Context.ConnectionId;
                List<string> groups = null;
                if (connectionGroups.TryGetValue(conId, out groups))
                {
                    groups.Clear();
                }
            }
        }

        public override Task OnConnected()
        {
            lock (sync)
            {
                string conId = Context.ConnectionId;
                connectionGroups.Add(conId, new List<string>());
            }
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            lock (sync)
            {
                string conId = Context.ConnectionId;
                connectionGroups.Remove(conId);
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}