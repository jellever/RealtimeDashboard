using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeDashboard.Database
{
    public class DatabaseContext : System.Data.Entity.DbContext
    {
        public DatabaseContext() :
           base("MyDatabaseContext")
        {

        }

        public System.Data.Entity.DbSet<RealtimeDashboard.Database.Models.ChatMessage> ChatMessages { get; set; }
    }
}
