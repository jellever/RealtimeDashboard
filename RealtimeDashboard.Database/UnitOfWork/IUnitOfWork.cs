using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RealtimeDashboard.Database.Models;
using RealtimeDashboard.Database.Repositories;


namespace RealtimeDashboard.Server.Database
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<ChatMessage> ChatMessageRepository { get; }

        IRepository<ChatRoom> ChatRoomRepository { get; }

        void Commit();
	}
}
