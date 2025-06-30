namespace Peak.App.Web.IDP
{
    public interface ITraceController
    {
        public void TraceDebug(string key, string message, params object[] args);

        public void TraceInformation(string key, string message, params object[] args);

        public void TraceError(string key, string message, params object[] args);
    }
}