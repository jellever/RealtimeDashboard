using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtimeDashboard.Database.Models
{
    public class ChatMessage : IEntityNotifyChanged
    {
        public Int64 Id { get; set; }

        public string Text { get; set; }

        public string Name { get; set; }

        public Int64 ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }


        public long GetID()
        {
            return Id;
        }

        public List<RelatedEntityInfo> GetRelatedEntityInfo()
        {
            List<RelatedEntityInfo> result = new List<RelatedEntityInfo>();
            if (ChatRoom != null)
            {
                result.Add(new RelatedEntityInfo()
                {
                    Id = ChatRoomId,
                    RelationName = "ChatRoom_ChatMessages",
                    TypeName = nameof(ChatRoom)
                });
            }
            return result;
        }

        public bool ShouldNotify()
        {
            return true;
        }
    }
}
