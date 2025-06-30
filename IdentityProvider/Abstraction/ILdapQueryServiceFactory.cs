using Peak.App.Web.IDP.Services;

namespace Peak.App.Web.IDP.Abstraction
{
    public interface ILdapQueryServiceFactory
    {
        ILdapQueryService GetService();
    }
}