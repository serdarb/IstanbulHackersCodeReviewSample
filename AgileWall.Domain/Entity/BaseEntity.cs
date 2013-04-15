namespace AgileWall.Domain.Entity
{
    using System;

    using MongoDB.Bson;

    public class BaseEntity
    {
        public BaseEntity()
        {
            CreatedOn = UpdatedOn = DateTime.Now;
        }

        public ObjectId Id { get; set; }

        public string IdStr
        {
            get
            {
                return Id.ToString();
            }
        }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}
