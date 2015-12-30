using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeDashboard.Database.Models
{
    public class ChatRoom : IEntityNotifyChanged
    {
        public Int64 Id { get; set; }

        public string Name { get; set; }

        public ICollection<ChatMessage> ChatMessages { get; set; } 

        public ChatRoom()
        {
            this.ChatMessages = new List<ChatMessage>();
        }

        public long GetID()
        {
            return Id;
        }

        public List<RelatedEntityInfo> GetRelatedEntityInfo()
        {
            return new List<RelatedEntityInfo>();
        }

        public bool ShouldNotify()
        {
            return true;
        }
    }
}
