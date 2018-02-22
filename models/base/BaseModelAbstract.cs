using System;

namespace depot {
    public abstract class BaseModelAbstract {

        public int ID { get; set; }

        public int CreatedAt { get; set; }
        public int UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public virtual void OnBeforeInsert () => this.CreatedAt = this.GetTimeStamp;
        public virtual void OnBeforeUpdate () => this.UpdatedAt = this.GetTimeStamp;

        protected int GetTimeStamp => (Int32) (DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1))).TotalSeconds;
    }

    public abstract class BaseModelUserRelationAbstract : BaseModelAbstract {

        public User CreatedBy { get; set; }
        public User UpdatedBy { get; set; }

    }
}