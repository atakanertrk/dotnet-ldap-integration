//namespace Peak.App.Web.IDP.Middlewares
//{
//    public class RequestResponseLoggingMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ITraceController _traceController;

//        public RequestResponseLoggingMiddleware(RequestDelegate next, ITraceController traceController)
//        {
//            _next = next;
//            _traceController = traceController;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            try
//            {
//                // Log Request
//                context.Request.EnableBuffering(); // allows reading the body multiple times
//                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
//                context.Request.Body.Position = 0;

//                _logger.LogInformation("Incoming Request: {method} {url} \nHeaders: {headers}\nBody: {body}",
//                    context.Request.Method,
//                    context.Request.Path,
//                    context.Request.Headers,
//                    requestBody);

//                // Capture response
//                var originalBodyStream = context.Response.Body;
//                using var responseBody = new MemoryStream();
//                context.Response.Body = responseBody;

//                await _next(context); // continue the pipeline

//                // Read and log response
//                context.Response.Body.Seek(0, SeekOrigin.Begin);
//                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
//                context.Response.Body.Seek(0, SeekOrigin.Begin);

//                _logger.LogInformation("Response: {statusCode}\nBody: {body}",
//                    context.Response.StatusCode,
//                    responseText);

//                // Copy response back to original stream
//                await responseBody.CopyToAsync(originalBodyStream);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "An unhandled exception occurred while processing the request.");
//                throw;
//            }
//        }
//    }


//}
