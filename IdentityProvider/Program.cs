
using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using Peak.App.Web.IDP.Abstraction;
using Peak.App.Web.IDP.Models;
using Peak.App.Web.IDP.Services;

namespace Peak.App.Web.IDP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<List<PlatformInformation>>(builder.Configuration.GetSection("Platforms"));
            builder.Services.Configure<TracingInformation>(builder.Configuration.GetSection("Tracing"));
            builder.Services.Configure<LdapQueryServiceInformation>(builder.Configuration.GetSection("LdapQueryService"));

            builder.Services.AddHttpClient();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Logging.ClearProviders();
            builder.Host.UseNLog();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IHttpContextHelper, HttpContextHelper>();
            builder.Services.AddTransient<ILdapQueryServiceFactory, LdapQueryServiceFactory>();
            builder.Services.AddTransient<IPlatformInformationService, PlatformInformationService>();
            builder.Services.AddTransient<IConfigurationHelper, ConfigurationHelper>();
            builder.Services.AddTransient<ITraceController, TraceController>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
