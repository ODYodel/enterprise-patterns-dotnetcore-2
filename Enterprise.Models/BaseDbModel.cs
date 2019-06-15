using System;

namespace Enterprise.Models
{
    public abstract class BaseDbModel
    {
        public int? Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedById { get; set; }
    }
}
