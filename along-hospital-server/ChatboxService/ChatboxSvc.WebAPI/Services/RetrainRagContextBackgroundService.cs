using ChatboxSvc.WebAPI.Interfaces;
using SharedLibrary.Base.Services;

namespace ChatboxSvc.WebAPI.Services
{
    public class RetrainRagContextBackgroundService(IServiceProvider serviceProvider) : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromHours(24);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var ragIndexBuilderService = this.GetService<IRagIndexBuilderService>(scope);
            await ragIndexBuilderService.BuildAllAsync();
        }
    }
}
