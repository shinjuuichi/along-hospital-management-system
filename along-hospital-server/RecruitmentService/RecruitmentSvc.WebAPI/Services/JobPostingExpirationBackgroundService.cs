using RecruitmentSvc.BLL.Interfaces;
using SharedLibrary.Base.Services;

namespace RecruitmentSvc.WebAPI.Services
{
    public class JobPostingExpirationBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(1);
        }

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var jobPostingService = this.GetService<IJobPostingService>(scope);
            await jobPostingService.CloseExpiredJobPostingsAsync();
        }
    }
}
