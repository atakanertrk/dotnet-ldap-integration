using Peak.App.Web.IDP.Enums;

namespace Peak.App.Web.IDP.Abstraction
{
    public interface IHttpContextHelper
    {
        string TryGetHeaderValue(string key, bool throwExceptionIfNotFound = false);
        string GetEnvironmentVariable(string key, bool throwExceptionIfNotFound = false);
        ApplicationPlatforms GetApplicationPlatform();
        DomainTypes GetDomainType();
    }
}