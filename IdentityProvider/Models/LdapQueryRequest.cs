namespace Peak.App.Web.IDP.Models
{
    public class LdapQueryRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
