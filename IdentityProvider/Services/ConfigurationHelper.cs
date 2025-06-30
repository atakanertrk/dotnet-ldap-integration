using Microsoft.Extensions.Options;
using Peak.App.Web.IDP.Abstraction;
using Peak.App.Web.IDP.Constants;
using Peak.App.Web.IDP.Enums;
using Peak.App.Web.IDP.Extensions;
using Peak.App.Web.IDP.Models;

namespace Peak.App.Web.IDP.Services
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        private readonly IOptionsMonitor<List<PlatformInformation>> _platformInformations;
        private readonly IOptionsMonitor<TracingInformation> _tracingInformation;
        private readonly IOptionsMonitor<LdapQueryServiceInformation> _ldapInfo;
        public ConfigurationHelper(IOptionsMonitor<List<PlatformInformation>> platformInformations, IOptionsMonitor<TracingInformation> tracingInformation, IOptionsMonitor<LdapQueryServiceInformation> ldapInfo)
        {
            _ldapInfo = ldapInfo;
            _tracingInformation = tracingInformation;
            _platformInformations = platformInformations;
        }

        public PlatformInformation GetPlatformInformation(ApplicationPlatforms platform, DomainTypes domainType)
        {
            var platformInfo = _platformInformations.CurrentValue?.FirstOrDefault(x => x.ApplicationPlatform == platform && x.DomainType == domainType);
            if (platformInfo is null)
            {
                throw new BaseException($"Platform: {platform} DomainType: {domainType} not found in settings file!", ErrorCodeConstants.APPLICATION_PLATFORM_SETTING_NOT_FOUND);
            }
            return platformInfo;
        }

        public TracingInformation GetTracingInformation() => _tracingInformation.CurrentValue;
        public LdapQueryServiceInformation GetLdapQueryServiceInfo() => _ldapInfo.CurrentValue;
    }
}
