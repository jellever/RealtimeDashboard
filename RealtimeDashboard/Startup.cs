using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using RealtimeDashboard.Server.Database;
using RealtimeDashboard.Core.Database;
using RealtimeDashboard.Database.Models;
using RealtimeDashboard.Database;
using RealtimeDashboard.Core.Logging;
using System.Data.Entity;

[assembly: OwinStartup(typeof(RealtimeDashboard.Startup))]

namespace RealtimeDashboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //test code
            DbContext dbContext = new DatabaseContext();
            Ilog log = new DebugLog();
            IUnitOfWork unitOfWork = new ChangeTrackingEFUnitOfWork(dbContext, log);
            ChatRoom room = new ChatRoom();
            room.Name = "TestRoom";
            room.ChatMessages.Add(new ChatMessage
            {
                Name = "A",
                Text = "Hello!"
            });
            unitOfWork.ChatRoomRepository.Add(room);
            unitOfWork.Commit();
        }
    }
}
