namespace Peak.App.Web.IDP.Extensions
{
    public class BaseException : Exception
    {
        public string ErrorCode { get; }

        public BaseException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
