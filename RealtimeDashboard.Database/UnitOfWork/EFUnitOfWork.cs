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

        public IRepository<MyModel> MyModelRepository { get; protected set; }


        public EFUnitOfWork(DbContext context)
        {
            this.dbContext = context;
            this.MyModelRepository = new GenericEFRepository<MyModel>(context);
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
