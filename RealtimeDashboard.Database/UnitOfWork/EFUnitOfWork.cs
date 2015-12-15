using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealtimeDashboard.Database.Models;
using RealtimeDashboard.Database.Repositories;


namespace RealtimeDashboard.Server.Database
{
    public class EFUnitOfWork : IUnitOfWork
    {
        protected DbContext dbContext;

        public IRepository<ChatMessage> ChatMessageRepository { get; protected set; }


        public EFUnitOfWork(DbContext context)
        {
            this.dbContext = context;
            this.ChatMessageRepository = new GenericEFRepository<ChatMessage>(context);
        }

        public virtual void Commit()
        {
            this.dbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.dbContext.Dispose();
        }
    }
}
