namespace AgileWall.Domain.Conract.RequestDto
{
    public class NewOrganizationRequestDto
    {
        private string _organizationName;
        private string _organizationUrlName;
        private string _userFirstName;
        private string _userLastName;
        private string _userEmail;
        private string _userPassword;

        public string OrganizationName
        {
            get { return _organizationName; }
            set
            {
                if (value != null)
                {
                    _organizationName = value.Trim();
                }
                _organizationName = value;
            }
        }

        public string OrganizationUrlName
        {
            get { return _organizationUrlName; }
            set
            {
                if (value != null)
                {
                    _organizationUrlName = value.Trim();
                }
                _organizationUrlName = value;
            }
        }

        public string UserFirstName
        {
            get { return _userFirstName; }
            set
            {
                if (value != null)
                {
                    _userFirstName = value.Trim();
                }
                _userFirstName = value;
            }
        }

        public string UserLastName
        {
            get { return _userLastName; }
            set
            {
                if (value != null)
                {
                    _userLastName = value.Trim();
                }
                _userLastName = value;
            }
        }

        public string UserEmail
        {
            get { return _userEmail; }
            set
            {
                if (value != null)
                {
                    _userEmail = value.Trim();
                }
                _userEmail = value;
            }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set
            {
                if (value != null)
                {
                    _userPassword = value.Trim();
                }
                _userPassword = value;
            }
        }
    }
}