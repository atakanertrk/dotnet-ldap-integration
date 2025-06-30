using Novell.Directory.Ldap;
using Peak.App.Web.IDP.Abstraction;
using Peak.App.Web.IDP.Constants;
using Peak.App.Web.IDP.Extensions;
using Peak.App.Web.IDP.Models;
using System.Text;

namespace Peak.App.Web.IDP.Services
{
    public class LdapQueryServiceNovell : ILdapQueryService
    {
        private readonly IPlatformInformationService _platformInformationService;
        private readonly IConfigurationHelper _configHelper;
        public LdapQueryServiceNovell(IPlatformInformationService platformInformationService, IConfigurationHelper configHelper)
        {
            _configHelper = configHelper;   
            _platformInformationService = platformInformationService;
        }

        public async Task<bool> IsUserExists(string userName)
        {
            var platformInfo = _platformInformationService.GetPlatformInformation();
            using var cn = new LdapConnection();
            await cn.ConnectAsync(platformInfo.ConnectHost, platformInfo.ConnectPort);
            await cn.BindAsync(
                $"{platformInfo.LdapAdminUserId}@{platformInfo.LdapDomain}", platformInfo.LdapAdminUserPassword);

            var results = await cn.SearchAsync(platformInfo.SearchBase, 2, $"(sAMAccountName={userName})", [], false);
            bool hasEntry = false;
            while (await results.HasMoreAsync())
            {
                try
                {
                    await results.NextAsync();
                    return true;
                }
                catch (LdapException ex)
                {
                    continue;
                }
            }
            return hasEntry;
        }

        public async Task<bool> IsUsernamePasswordValid(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                throw new BaseException("Username and password must be set!",ErrorCodeConstants.USERNAME_PASSWORD_EMPTY);
            }
            var platformInfo = _platformInformationService.GetPlatformInformation();
            using var cn = new LdapConnection();
            await cn.ConnectAsync(platformInfo.ConnectHost, platformInfo.ConnectPort);
            await cn.BindAsync($"{userName}@{platformInfo.LdapDomain}", password);

            return true;
        }

        public async Task<bool> SetPassword(string userName, string newPassword)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(newPassword))
            {
                throw new BaseException("Username and newPassword must be set!", ErrorCodeConstants.USERNAME_PASSWORD_EMPTY);
            }

            string userDn = string.Empty;
            var platformInfo = _platformInformationService.GetPlatformInformation();

            var options = new LdapConnectionOptions().ConfigureRemoteCertificateValidationCallback((sender, certificate, chain, SslPolicyErrors) => { return true; });
            using var cn = new LdapConnection(options);
            cn.SecureSocketLayer = true;
            await cn.ConnectAsync(platformInfo.ConnectHost, platformInfo.ConnectSSLPort);
            await cn.BindAsync(
                $"{platformInfo.LdapAdminUserId}@{platformInfo.LdapDomain}", platformInfo.LdapAdminUserPassword);

            var results = await cn.SearchAsync(platformInfo.SearchBase, 2, $"(sAMAccountName={userName})", [], false);
            while (await results.HasMoreAsync())
            {
                try
                {
                    var entry = await results.NextAsync();
                    userDn = entry.Dn;
                    break;
                }
                catch (LdapException)
                {
                    continue;
                }
            }

            if (string.IsNullOrEmpty(userDn))
            {
                throw new BaseException($"User {userName} not found!", ErrorCodeConstants.USER_NOT_FOUND);
            }

            string quotedPassword = $"\"{newPassword}\"";
            byte[] encodedPassword = Encoding.Unicode.GetBytes(quotedPassword);

            var modification = new LdapModification(
                LdapModification.Replace,
                new LdapAttribute("unicodePwd", encodedPassword));

            try
            {
                await cn.ModifyAsync(userDn, new LdapModification[] { modification });
                return true;
            }
            catch (LdapException ex)
            {
                return false;
            }
        }

        public async Task<DateTime> GetPasswordExpirationDate(string userName)
        {
            var platformInfo = _platformInformationService.GetPlatformInformation();
            using var cn = new LdapConnection();
            await cn.ConnectAsync(platformInfo.ConnectHost, platformInfo.ConnectPort);
            await cn.BindAsync(
                $"{platformInfo.LdapAdminUserId}@{platformInfo.LdapDomain}", platformInfo.LdapAdminUserPassword);

            var searchResults = await cn.SearchAsync(platformInfo.SearchBase, 2, $"(sAMAccountName={userName})", new string[] { "msDS-UserPasswordExpiryTimeComputed" }, false);

            while (await searchResults.HasMoreAsync())
            {
                try
                {
                    var entry = await searchResults.NextAsync();
                    string userPasswordExpiryTimeComputed = entry.GetAttributeSet().FirstOrDefault().Value.StringValue; // since we have only one attribute, first one is the UserPasswordExpiryTimeComputed
                    return DateTime.FromFileTimeUtc(long.Parse(userPasswordExpiryTimeComputed)).ToLocalTime();
                }
                catch (LdapException)
                {
                    continue;
                }
            }
            throw new BaseException($"Search does not has entry for operation GetPasswordExpirationDate for user {userName}", ErrorCodeConstants.SEARCH_DOES_NOT_HAVE_ENTRY);

        }
    }
}
