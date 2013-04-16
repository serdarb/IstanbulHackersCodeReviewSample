namespace AgileWall.Domain.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MongoDB.Bson;
    using MongoDB.Driver.Builders;

    using Conract;
    using Conract.RequestDto;
    using Entity;
    using Repo;
    using Utils;

    public class OrganizationService : IOrganizationService
    {
        private readonly IBaseRepo<Organization> _orgRepo;
        private readonly IBaseRepo<User> _userRepo;

        public OrganizationService(
            IBaseRepo<Organization> orgRepo,
            IBaseRepo<User> userRepo)
        {
            _orgRepo = orgRepo;
            _userRepo = userRepo;
        }

        #region Helper Methods
        private string UpdateSlugIfNecessary(string slug)
        {
            if (_orgRepo.AsQueryable().Any(x => x.NameUrl == slug))
            {
                slug = string.Format("{0}-{1}", slug, _orgRepo.AsQueryable().Count(x => x.NameUrl.StartsWith(slug)) + 1);
            }

            return slug;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        private bool IsUserDoesNotHaveSameOrganizaiton(string userId, string organizationName)
        {
            return !_orgRepo.AsQueryable().Any(x => x.UserId == userId && x.Name == organizationName);
        }

        private void CreateGroupWallsAndSetUsersRoles(Organization org, User user)
        {
            var roles = CreateGroups(org);
            SetUserRoles(org, user, roles);
        }

        private void SetUserRoles(Organization org, User user, List<string> roles)
        {
            _userRepo.Update(
                Query<User>.EQ(x => x.Id, user.Id),
                Update<User>.Set(x => x.OrganizationId, org.IdStr).PushAll(x => x.Roles, roles));
        }

        private List<string> CreateGroups(Organization org)
        {
            var roles = new List<string> {
                                             string.Format(Consts.OrgRoleStringFormat, org.IdStr),
                                             string.Format(Consts.OrgAdminRoleStringFormat, org.IdStr)
                                         };
            foreach (var defaultGroup in Consts.DefaultGroups)
            {
                roles.Add(string.Format(Consts.OrgGroupRoleStringFormat, org.IdStr, defaultGroup));

                _orgRepo.Update(
                    Query<Organization>.EQ(x => x.Id, org.Id),
                    Update<Organization>.Push(x => x.WallIds, Guid.NewGuid().ToString()));
            }
            return roles;
        }

        private Organization MapOrganization(NewOrganizationRequestDto dto, User user)
        {
            var urlname = UpdateSlugIfNecessary(dto.OrganizationUrlName);
            return new Organization
            {
                Name = dto.OrganizationName,
                NameLowered = dto.OrganizationName.ToLowerInvariant(),
                NameUrl = urlname,
                CreatedBy = user.IdStr,
                UpdatedBy = user.IdStr,
                UserId = user.IdStr,
                Users =
                    new List<UserSummary> {
                                                                      new UserSummary {
                                                                                          UserId = user.IdStr,
                                                                                          Initial = user.Initial,
                                                                                          Name = user.Name
                                                                                      }
                                                                  },
                WallIds = new List<string>(),
                Groups = Consts.DefaultGroups,
                CustomerType = "free",
                MaxUserCount = 2
            };
        }

        private User GetOrCreateUser(string email, string firstName, string lastName, string name, string initial, string password)
        {
            var user = _userRepo.AsQueryable().FirstOrDefault(x => x.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Name = name,
                    NameLowered = name.ToLowerInvariant(),
                    Initial = initial,
                    PasswordHash = this.HashPassword(password),
                    Roles = new List<string>()
                };

                _userRepo.Add(user);
            }

            return user;
        }

        private User GetUser(string email)
        {
            return _userRepo.AsQueryable().FirstOrDefault(x => x.Email == email);
        }
        #endregion


        public string CreateOrganizationWithUser(NewOrganizationRequestDto dto)
        {
            if (dto.IsNotValid)
            {
                return null;
            }

            var user = GetOrCreateUser(dto.UserEmail, dto.UserFirstName, dto.UserLastName, dto.UserName, dto.Initial, dto.UserPassword);

            if (IsUserDoesNotHaveSameOrganizaiton(user.IdStr, dto.OrganizationName))
            {
                var org = MapOrganization(dto, user);
                var result = _orgRepo.Add(org);
                if (result.Ok)
                {
                    CreateGroupWallsAndSetUsersRoles(org, user);

                    return org.IdStr;
                }
            }

            return null;
        }

        public string CreateOrganization(NewOrganizationRequestDto dto)
        {
            if (dto.IsNotValidForExistingUsersOrganizationCreation)
            {
                return null;
            }

            var user = GetUser(dto.UserEmail);
            if (user == null)
            {
                return null;
            }

            if (IsUserDoesNotHaveSameOrganizaiton(user.IdStr, dto.OrganizationName))
            {
                var org = MapOrganization(dto, user);
                var result = _orgRepo.Add(org);
                if (result.Ok)
                {
                    CreateGroupWallsAndSetUsersRoles(org, user);

                    return org.IdStr;
                }
            }

            return null;
        }
        

        public Organization GetOrganizationById(string id)
        {
            ObjectId oId;
            if (ObjectId.TryParse(id, out oId))
            {
                return _orgRepo.GetSingle(x => x.Id == oId);
            }

            return null;
        }

        public Organization GetOrganizationBySlug(string slug)
        {
            if (!string.IsNullOrEmpty(slug))
            {
                return _orgRepo.AsQueryable().FirstOrDefault(x => x.NameUrl == slug.Trim());
            }

            return null;
        }
    }
}