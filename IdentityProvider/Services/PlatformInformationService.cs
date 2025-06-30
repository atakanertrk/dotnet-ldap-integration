using Peak.App.Web.IDP.Abstraction;
using Peak.App.Web.IDP.Constants;
using Peak.App.Web.IDP.Enums;
using Peak.App.Web.IDP.Extensions;
using Peak.App.Web.IDP.Models;

namespace Peak.App.Web.IDP.Services
{
    public class PlatformInformationService : IPlatformInformationService
    {
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IConfigurationHelper _configHelper;
        public PlatformInformationService(IHttpContextHelper httpContextHelper, IConfigurationHelper configHelper)
        {
            _httpContextHelper = httpContextHelper;
            _configHelper = configHelper;
        }

        public PlatformInformation GetPlatformInformation()
        {
            DomainTypes domainType = _httpContextHelper.GetDomainType();
            ApplicationPlatforms platform = _httpContextHelper.GetApplicationPlatform();
            switch (platform)
            {
                case ApplicationPlatforms.VitwebApi:
                    switch (domainType)
                    {
                        case DomainTypes.VakifExtra:
                            return getVitwebVakifExtraPlatformInfo();
                    }
                    break;
            case ApplicationPlatforms.SpikeWebApi:
                    switch (domainType)
                    {
                        case DomainTypes.SpikeIntra:
                            return getSpikeIntraPlatformInfo();
                    }
                    break;
            }
            throw new BaseException($"Domain: {domainType} Platform: {platform} is not implemented.",ErrorCodeConstants.PLATFORM_NOT_IMPLEMENTED);
        }

        private PlatformInformation getSpikeIntraPlatformInfo()
        {
            var platformInfo = _configHelper.GetPlatformInformation(ApplicationPlatforms.SpikeWebApi,DomainTypes.SpikeIntra);
            platformInfo.LdapAdminUserId = _httpContextHelper.GetEnvironmentVariable(EnvironmentVariableConstants.LdapAdminUserId_SpikeIntra, true);
            platformInfo.LdapAdminUserPassword = _httpContextHelper.GetEnvironmentVariable(EnvironmentVariableConstants.LdapAdminUserPassword_SpikeIntra, true);
            return platformInfo;
        }

        private PlatformInformation getVitwebVakifExtraPlatformInfo()
        {
            var platformInfo = _configHelper.GetPlatformInformation(ApplicationPlatforms.VitwebApi, DomainTypes.VakifExtra);
            platformInfo.LdapAdminUserId = _httpContextHelper.GetEnvironmentVariable(EnvironmentVariableConstants.LdapAdminUserId_VakifExtra, true);
            platformInfo.LdapAdminUserPassword = _httpContextHelper.GetEnvironmentVariable(EnvironmentVariableConstants.LdapAdminUserPassword_VakifExtra, true);
            return platformInfo;
        }
    }
}
