namespace Peak.App.Web.IDP.Models
{
    public class LdapQueryResponse
    {
        public bool IsUsernamePasswordCorrect { get; set; }
        public bool IsUserExists { get; set; }
        public DateTime PasswordExpirationDate { get; set; }
        public bool IsSucceded { get; set; }
        public ErrorDetails ErrorDetails { get; set; }
    }
}
