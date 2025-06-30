using Peak.App.Web.IDP.Abstraction;

namespace Peak.App.Web.IDP.Services
{
    public class LdapQueryServiceFactory : ILdapQueryServiceFactory
    {
        private readonly IConfigurationHelper _configHelper;
        private readonly IPlatformInformationService _platformInformation;
        public LdapQueryServiceFactory(IConfigurationHelper configHelper, IPlatformInformationService platformInformation)
        {
            _configHelper = configHelper;
            _platformInformation = platformInformation;
        }

        public ILdapQueryService GetService()
        {
            switch (_configHelper.GetLdapQueryServiceInfo().LdapServiceType)
            {
                case Enums.LdapServiceTypes.Novell:
                    return new LdapQueryServiceNovell(_platformInformation, _configHelper);
                case Enums.LdapServiceTypes.NovellWithAdminCache:
                    return new LdapQueryServiceNovellWithCache(_platformInformation, _configHelper);
                case Enums.LdapServiceTypes.SystemDirectoryService:
                    return new LdapQueryServiceSystemDirectory(_platformInformation, _configHelper);
                case Enums.LdapServiceTypes.SystemDirectoryServiceWithAdminCache:
                    return new LdapQueryServiceSystemDirectoryWithCache(_platformInformation, _configHelper);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
