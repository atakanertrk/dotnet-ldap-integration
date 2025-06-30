using Peak.App.Web.IDP.Abstraction;
using System.Net;
using System.DirectoryServices.Protocols;
using Peak.App.Web.IDP.Constants;
using Peak.App.Web.IDP.Extensions;

namespace Peak.App.Web.IDP.Services
{
    public class LdapQueryServiceSystemDirectoryWithCache : ILdapQueryService
    {
        private readonly IPlatformInformationService _platformInformation;
        private readonly IConfigurationHelper _configHelper;
        public LdapQueryServiceSystemDirectoryWithCache(IPlatformInformationService platformInformation, IConfigurationHelper configHelper)
        {
            _configHelper = configHelper;
            _platformInformation = platformInformation;
        }

        private static readonly object _lockObject = new object();
        private static LdapConnection _adminLdapConnection;
        private LdapConnection getBindedAdminLDAPConnection()
        {
            if (_adminLdapConnection is null)
            {
                lock (_lockObject)
                {
                    if (_adminLdapConnection is null)
                    {
                        var platformInfo = _platformInformation.GetPlatformInformation();
                        var identifier = new LdapDirectoryIdentifier(platformInfo.ConnectHost, platformInfo.ConnectPort);

                        _adminLdapConnection = new LdapConnection(identifier)
                        {
                            AuthType = AuthType.Negotiate
                        };

                        var credential = new NetworkCredential($"{platformInfo.LdapAdminUserId}@{platformInfo.LdapDomain}", platformInfo.LdapAdminUserPassword);
                        _adminLdapConnection.Bind(credential);
                    }
                }
            }

            return _adminLdapConnection;
        }

        public async Task<DateTime> GetPasswordExpirationDate(string userName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsUserExists(string userName)
        {
            var platformInfo = _platformInformation.GetPlatformInformation();
            var searchRequest = new SearchRequest(platformInfo.SearchBase,$"(sAMAccountName={userName})",SearchScope.Subtree,  null);

            try
            {
                var response = (SearchResponse)getBindedAdminLDAPConnection().SendRequest(searchRequest);
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
