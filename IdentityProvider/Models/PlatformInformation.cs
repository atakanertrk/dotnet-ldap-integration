using Peak.App.Web.IDP.Enums;

namespace Peak.App.Web.IDP.Models
{
    public class PlatformInformation
    {
        public string ConnectHost { get; set; } // example vakifbank.intra
        public int ConnectPort { get; set; } // default 389
        public int ConnectSSLPort { get; set; } // default 389
        public string SearchBase { get; set; } // example: dc=vakifbank,dc=intra
        public ApplicationPlatforms ApplicationPlatform { get; set; }
        public DomainTypes DomainType { get; set; }
        public string LdapDomain { get; set; }
        public string LdapAdminUserId { get; set; }
        public string LdapAdminUserPassword { get; set; }
    }
}
