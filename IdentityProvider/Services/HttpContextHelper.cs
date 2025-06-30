using Peak.App.Web.IDP.Abstraction;
using Peak.App.Web.IDP.Constants;
using Peak.App.Web.IDP.Enums;
using Peak.App.Web.IDP.Extensions;

namespace Peak.App.Web.IDP.Services
{
    public class HttpContextHelper : IHttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HttpContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string TryGetHeaderValue(string key, bool throwExceptionIfNotFound = false)
        {
            _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue(key, out var value);

            if (string.IsNullOrEmpty(value))
            {
                throw new BaseException($"Value {value} is not present in request headers!", ErrorCodeConstants.HEADER_VALUE_NOT_PRESENT);
            }

            return value;
        }

        public string GetEnvironmentVariable(string key, bool throwExceptionIfNotFound = false)
        {
            string value = Environment.GetEnvironmentVariable(key);
            if (throwExceptionIfNotFound && string.IsNullOrEmpty(value))
            {
                throw new BaseException($"Value {value} is not present in environment variable!", ErrorCodeConstants.ENVIRONMENT_VARIABLE_NOT_FOUND);
            }
            return value;
        }

        public ApplicationPlatforms GetApplicationPlatform() => Enum.Parse<ApplicationPlatforms>(TryGetHeaderValue("ApplicationPlatformName"), true);
        public DomainTypes GetDomainType() => Enum.Parse<DomainTypes>(TryGetHeaderValue("DomainType"), true);
    }
}
