namespace AgileWall.Domain.Entity
{
    using System.Collections.Generic;

    public class Organization : BaseEntity
    {
        public string Name { get; set; }
        public string NameLowered { get; set; }
        public string NameUrl { get; set; }

        public string UserId { get; set; }
        public string CustomerType { get; set; }
        public int MaxUserCount { get; set; }

        public List<string> Groups { get; set; }
        public List<UserSummary> Users { get; set; }
        public List<UserSummary> OnlineUsers { get; set; }

        public List<string> WallIds { get; set; }
    }
}