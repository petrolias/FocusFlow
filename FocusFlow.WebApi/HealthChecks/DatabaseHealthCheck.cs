using FocusFlow.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace FocusFlow.WebApi.HealthChecks
{
    public class DatabaseHealthCheck(Context context) : IHealthCheck
    {
        private readonly Context _context = context;

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
                return canConnect
                    ? HealthCheckResult.Healthy("Database connection is healthy.")
                    : HealthCheckResult.Unhealthy("Cannot connect to the database.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Exception when checking DB health.", ex);
            }
        }
    }
}
