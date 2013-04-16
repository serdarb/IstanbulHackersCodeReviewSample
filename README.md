IstanbulHackersCodeReviewSample
===============================

a sample project that was used as code review sample at meet up of istanbul hackers at 13th April 2013


-------------------------------

there is a view /org/new that posts to OrgController to create new organization.
OrgController's New action calls _organizationService.CreateOrganization.

OrganizationService has dependencies to IBaseRepo<Organization> orgRepo, IBaseRepo<User> userRepo.
