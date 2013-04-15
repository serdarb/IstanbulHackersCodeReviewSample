namespace AgileWall.Utils
{
    using System.Globalization;
    using System.Collections.Generic;

    public static class Consts
    {
        public static List<string> DefaultGroups = new List<string> { "Main", "Marketing", "IT" };

        public static string OrgRoleStringFormat = "org-{0}";
        public static string OrgAdminRoleStringFormat = "org-admin-{0}";
        public static string OrgGroupRoleStringFormat = "org-{0}-group-{1}";

        public const string DBName = "AgileWall";

        public const string System = "System";


        private static CultureInfo _cultureTR;
        public static CultureInfo CultureTR
        {
            get { return _cultureTR ?? (_cultureTR = new CultureInfo("tr-TR")); }
        }

        private static CultureInfo _cultureEN;
        public static CultureInfo CultureEN
        {
            get { return _cultureEN ?? (_cultureEN = new CultureInfo("en-US")); }
        }
    }
}
