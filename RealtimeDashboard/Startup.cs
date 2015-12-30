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
using System.Globalization;
using System.Threading;
using Microsoft.AspNet.SignalR;
using RealtimeDashboard.Hubs;

[assembly: OwinStartup(typeof(RealtimeDashboard.Startup))]

namespace RealtimeDashboard
{
    public partial class Startup
    {
        private Ilog log;
        private LiveHubController liveHubController;


        public void Configuration(IAppBuilder app)
        {
            //http://www.zpqrtbnk.net/posts/appdomains-threads-cultureinfos-and-paracetamol
            SanitizeThreadCulture(app);
            HubConfiguration config = new HubConfiguration();
            config.EnableDetailedErrors = true;

            app.MapSignalR(config);

            log = new DebugLog();
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());
            liveHubController = new LiveHubController(log);
        }

        public static void SanitizeThreadCulture(IAppBuilder app)
        {
            var currentCulture = CultureInfo.CurrentCulture;

            // at the top of any culture should be the invariant culture,
            // find it doing an .Equals comparison ensure that we will
            // find it and not loop endlessly
            var invariantCulture = currentCulture;
            while (invariantCulture.Equals(CultureInfo.InvariantCulture) == false)
                invariantCulture = invariantCulture.Parent;

            if (ReferenceEquals(invariantCulture, CultureInfo.InvariantCulture))
                return;

            var thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.GetCultureInfo(thread.CurrentCulture.Name);
            thread.CurrentUICulture = CultureInfo.GetCultureInfo(thread.CurrentUICulture.Name);
        }
    }
}
