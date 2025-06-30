using Microsoft.AspNetCore.Mvc;
using Peak.App.Web.IDP.Abstraction;
using Peak.App.Web.IDP.Models;
using Peak.App.Web.IDP.Services;
using System.Net;

namespace Peak.App.Web.IDP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LdapQueryController : ControllerBase
    {
        private readonly ILdapQueryService _ldapQueryService;
        private readonly IConfigurationHelper _configurationHelper;
        public LdapQueryController(ILdapQueryServiceFactory ldapFactory, IConfigurationHelper configurationHelper)
        {
            _configurationHelper = configurationHelper;
            _ldapQueryService = ldapFactory.GetService();
        }

        [HttpPost]
        public async Task<IActionResult> SearchUser([FromBody] LdapQueryRequest queryFilters)
        {
            bool isUserExists = await _ldapQueryService.IsUserExists(queryFilters.Username);
            return Ok(new LdapQueryResponse() { IsUserExists = isUserExists });
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUsernamePassword([FromBody] LdapQueryRequest queryFilters)
        {
            bool isValid = await _ldapQueryService.IsUsernamePasswordValid(queryFilters.Username, queryFilters.Password);
            return Ok(new LdapQueryResponse() { IsUsernamePasswordCorrect = isValid });
        }

        [HttpPost]
        public async Task<IActionResult> GetPasswordExpirationDate([FromBody] LdapQueryRequest queryFilters)
        {
            DateTime expiration = await _ldapQueryService.GetPasswordExpirationDate(queryFilters.Username);
            return Ok(new LdapQueryResponse() { PasswordExpirationDate = expiration });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] LdapQueryRequest queryFilters)
        {
            bool isSucceded = false;
            bool isValid = await _ldapQueryService.IsUsernamePasswordValid(queryFilters.Username, queryFilters.OldPassword);
            if (isValid)
            {
                isSucceded = await _ldapQueryService.SetPassword(queryFilters.Username,queryFilters.NewPassword);
            }

            return Ok(new LdapQueryResponse() { IsSucceded = isSucceded });
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword([FromBody] LdapQueryRequest queryFilters)
        {
            bool isSucceded = await _ldapQueryService.SetPassword(queryFilters.Username, queryFilters.NewPassword);
            return Ok(new LdapQueryResponse() { IsSucceded = isSucceded });
        }
    }
}
