using Peak.App.Web.IDP.Abstraction;
using System.Net;
using System.DirectoryServices.Protocols;
using Novell.Directory.Ldap.Utilclass;
using Peak.App.Web.IDP.Constants;
using Peak.App.Web.IDP.Extensions;

namespace Peak.App.Web.IDP.Services
{
    public class LdapQueryServiceSystemDirectory : ILdapQueryService
    {
        private readonly IPlatformInformationService _platformInformation;
        private readonly IConfigurationHelper _configHelper;
        public LdapQueryServiceSystemDirectory(IPlatformInformationService platformInformation, IConfigurationHelper configHelper)
        {
            _configHelper = configHelper;
            _platformInformation = platformInformation;
        }
        public async Task<DateTime> GetPasswordExpirationDate(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserExists(string userName)
        {
            var platformInfo = _platformInformation.GetPlatformInformation();
            var identifier = new LdapDirectoryIdentifier(platformInfo.ConnectHost, platformInfo.ConnectPort);

            using var cn = new LdapConnection(identifier)
            {
                AuthType = AuthType.Negotiate
            };

            var credential = new NetworkCredential($"{platformInfo.LdapAdminUserId}@{platformInfo.LdapDomain}", platformInfo.LdapAdminUserPassword);
            cn.Bind(credential);

            var searchRequest = new SearchRequest(platformInfo.SearchBase,$"(sAMAccountName={userName})",SearchScope.Subtree,  null);

            try
            {
                var response = (SearchResponse)cn.SendRequest(searchRequest);
                return response?.Entries?.Count > 0;
            }
            catch (DirectoryOperationException ex)
            {
                Console.WriteLine($"LDAP search failed: {ex.Message}");
                throw new BaseException($"LDAP search failed for user {userName} message:{ex.Message} ", ErrorCodeConstants.SEARCH_DOES_NOT_HAVE_ENTRY);
            }
        }

        public async Task<bool> IsUsernamePasswordValid(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetPassword(string userName, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}
