using System;
using System.Collections.Generic;

namespace RealtimeDashboard.Database.Models
{
    public interface IEntityNotifyChanged
    {
        Int64 GetID();
        List<RelatedEntityInfo> GetRelatedEntityInfo();
        bool ShouldNotify();
    }

    public class RelatedEntityInfo
    {
        public Int64 Id { get; set; }
        public string TypeName { get; set; }
        public string RelationName { get; set; }
    }
}
