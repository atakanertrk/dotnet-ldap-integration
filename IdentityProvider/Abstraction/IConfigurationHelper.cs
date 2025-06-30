using Peak.App.Web.IDP.Enums;
using Peak.App.Web.IDP.Models;

namespace Peak.App.Web.IDP.Abstraction
{
    public interface IConfigurationHelper
    {
        PlatformInformation GetPlatformInformation(ApplicationPlatforms platform, DomainTypes domainType);
        TracingInformation GetTracingInformation();
        LdapQueryServiceInformation GetLdapQueryServiceInfo();
    }
}