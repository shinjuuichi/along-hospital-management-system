using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SharedLibrary.Base.Services
{
    public abstract class BaseBackgroundService(IServiceProvider serviceProvider) : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        protected readonly IServiceProvider _serviceProvider = serviceProvider;

        protected T GetService<T>(IServiceScope scope) where T : notnull => scope.ServiceProvider.GetRequiredService<T>();

        protected abstract TimeSpan GetInterval();

        protected abstract Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken);

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async _ =>
            {
                using var scope = _serviceProvider.CreateScope();
                try
                {
                    await this.ExecuteTaskAsync(scope, cancellationToken);
                }
                catch (Exception ex)
                {
                    var logger = this.GetService<ILogger<BaseBackgroundService>>(scope);
                    logger.LogError(ex, "An error occurred while executing the background task.");
                }
            }, null, TimeSpan.Zero, GetInterval());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}