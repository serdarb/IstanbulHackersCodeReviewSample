namespace AgileWall.Domain.Entity
{
    using System;
    using System.Collections.Generic;

    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetRequestedOn { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string NameLowered { get; set; }

        public string Initial { get; set; }

        public List<string> Roles { get; set; }

        public string OrganizationId { get; set; }

        public string InvitationToken { get; set; }
        public DateTime? InviteAcceptedOn { get; set; }
    }
}
