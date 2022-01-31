namespace WebApi.Modules.Common;

using Prometheus;

public static class HttpMetricsExtensions
{
    public static IApplicationBuilder UseCustomHttpMetrics(this IApplicationBuilder appBuilder)
    {
        appBuilder
          .UseMetricServer()
          .UseHttpMetrics(opt =>
          {
              opt.RequestDuration.Enabled = true;
              opt.InProgress.Enabled = true;
              opt.RequestCount.Enabled = true;
          });

        return appBuilder;
    }
}
