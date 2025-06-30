using Peak.App.Web.IDP.Models;

namespace Peak.App.Web.IDP.Abstraction
{
    public interface ILdapQueryService
    {
        Task<bool> IsUserExists(string userName);
        Task<bool> IsUsernamePasswordValid(string userName, string password);
        Task<DateTime> GetPasswordExpirationDate(string userName);
        Task<bool> SetPassword(string userName, string newPassword);
    }
}