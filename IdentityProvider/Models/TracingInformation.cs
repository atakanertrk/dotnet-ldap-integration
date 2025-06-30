namespace Peak.App.Web.IDP.Models
{
    public class TracingInformation
    {
        public bool RequestResponseTracingEnableForAllPaths { get; set; }
        public string[] RequestResponseTracingEnabledPaths { get; set; } = new string[] { }; 
    }
}
