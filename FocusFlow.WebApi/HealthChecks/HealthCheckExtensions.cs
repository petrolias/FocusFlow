using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FocusFlow.WebApi.HealthChecks
{
    public static class HealthCheckExtensions
    {
        public enum HealthCheckType
        {
            Health,
            Readiness,
            Liveness,
            Startup
        }

        public static WebApplication AddHealthChecks(this WebApplication app)
        {
            MapTaggedHealthCheck(app, "/health", HealthCheckType.Health);
            MapTaggedHealthCheck(app, "/readiness", HealthCheckType.Readiness);
            MapTaggedHealthCheck(app, "/liveness", HealthCheckType.Liveness);
            MapTaggedHealthCheck(app, "/startup", HealthCheckType.Startup);
            MapTaggedHealthCheck(app, "/startup", HealthCheckType.Startup);

            return app;
        }

        private static void MapTaggedHealthCheck(WebApplication app, string path, HealthCheckType type)
        {
            app.MapHealthChecks(path, new HealthCheckOptions
            {
                Predicate = check => check.Tags.Contains(type.ToString()),
                ResponseWriter = CreateResponseWriter(type)
            });
        }

        private static Func<HttpContext, HealthReport, Task> CreateResponseWriter(HealthCheckType type)
        {
            return async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                // Customize response based on health check type
                object response = type switch
                {
                    HealthCheckType.Health => new
                    {
                        endpoint = type.ToString(),
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description
                        })
                    },

                    HealthCheckType.Readiness => new
                    {
                        endpoint = type.ToString(),
                        isReady = report.Status == HealthStatus.Healthy,
                        checks = report.Entries.Select(entry => new
                        {
                            name = entry.Key,
                            status = entry.Value.Status.ToString(),
                            description = entry.Value.Description
                        })
                    },

                    HealthCheckType.Liveness => new
                    {
                        endpoint = type.ToString(),
                        alive = report.Status == HealthStatus.Healthy
                    },

                    HealthCheckType.Startup => new
                    {
                        endpoint = type.ToString(),
                        startupOk = report.Status == HealthStatus.Healthy
                    },

                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                await context.Response.WriteAsJsonAsync(response);
            };
        }
    }
}