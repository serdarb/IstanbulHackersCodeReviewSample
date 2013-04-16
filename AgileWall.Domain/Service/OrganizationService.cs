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

        public string CreateOrganization(NewOrganizationRequestDto dto)
        {
            if (!string.IsNullOrEmpty(dto.UserEmail)
                && !string.IsNullOrEmpty(dto.OrganizationName)
                && !string.IsNullOrEmpty(dto.OrganizationUrlName)

                && dto.UserEmail.IsEmail())
            {
                var user = _userRepo.AsQueryable().FirstOrDefault(x => x.Email == dto.UserEmail);
                if (user == null)
                {
                    if (string.IsNullOrEmpty(dto.UserFirstName)
                        || string.IsNullOrEmpty(dto.UserLastName)
                        || string.IsNullOrEmpty(dto.UserPassword))
                    {
                        return null;
                    }

                    var name = string.Format("{0} {1}", dto.UserFirstName, dto.UserLastName);
                    user = new User
                        {
                            Email = dto.UserEmail,
                            FirstName = dto.UserFirstName,
                            LastName = dto.UserLastName,
                            Name = name,
                            NameLowered = name.ToLowerInvariant(),
                            Initial = string.Format("{0}{1}", dto.UserFirstName[0], dto.UserLastName[0]),
                            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword, BCrypt.Net.BCrypt.GenerateSalt(12)),
                            Roles = new List<string>()
                        };

                    _userRepo.Add(user);
                }

                if (!_orgRepo.AsQueryable().Any(x => x.UserId == user.IdStr && x.Name == dto.OrganizationName))
                {
                    var urlname = UpdateSlugIfNecessary(dto.OrganizationUrlName);

                    var org = new Organization
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

                    var result = _orgRepo.Add(org);
                    if (result.Ok)
                    {
                        var roles = new List<string> { string.Format(Consts.OrgRoleStringFormat, org.IdStr), string.Format(Consts.OrgAdminRoleStringFormat, org.IdStr) };
                        foreach (var defaultGroup in Consts.DefaultGroups)
                        {
                            roles.Add(string.Format(Consts.OrgGroupRoleStringFormat, org.IdStr, defaultGroup));

                            _orgRepo.Update(Query<Organization>.EQ(x => x.Id, org.Id),
                                            Update<Organization>.Push(x => x.WallIds, Guid.NewGuid().ToString()));
                        }

                        _userRepo.Update(Query<User>.EQ(x => x.Id, user.Id),
                                         Update<User>.Set(x => x.OrganizationId, org.IdStr)
                                                     .PushAll(x => x.Roles, roles));

                        return org.IdStr;
                    }
                }
            }

            return null;
        }

        private string UpdateSlugIfNecessary(string slug)
        {
            if (_orgRepo.AsQueryable().Any(x => x.NameUrl == slug))
            {
                slug = string.Format("{0}-{1}", slug, _orgRepo.AsQueryable().Count(x => x.NameUrl.StartsWith(slug)) + 1);
            }

            return slug;
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