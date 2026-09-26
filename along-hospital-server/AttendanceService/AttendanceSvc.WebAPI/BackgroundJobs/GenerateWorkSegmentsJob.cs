using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Utils;

namespace AttendanceSvc.WebAPI.BackgroundJobs
{
    public class GenerateWorkSegmentsJob(IServiceProvider serviceProvider) : BaseBackgroundService(serviceProvider)
    {
        private const int RunDayOfMonth = 25;
        private const int PeriodDays = 30;
        private static readonly TimeSpan RunTime = new(0, 5, 0);
        private static readonly TimeSpan CheckInterval = TimeSpan.FromDays(1);

        private DateOnly? _lastRunDate;

        protected override TimeSpan GetInterval() => CheckInterval;

        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var nowLocal = DateTime.UtcNow.ConvertTimeToTimeZone();
            if (nowLocal.Day != RunDayOfMonth)
            {
                return;
            }

            if (nowLocal.TimeOfDay < RunTime)
            {
                return;
            }

            var currentDate = DateOnly.FromDateTime(nowLocal);
            if (_lastRunDate == currentDate)
            {
                return;
            }

            _lastRunDate = currentDate;

            var periodEnd = new DateTime(nowLocal.Year, nowLocal.Month, RunDayOfMonth, 0, 0, 0);
            var periodStart = periodEnd.AddDays(-PeriodDays);

            var messageBus = this.GetService<IMessageBus>(scope);
            await messageBus.PublishAsync(new GenerateWorkSegmentsEvent
            {
                PeriodStart = periodStart,
                PeriodEnd = periodEnd,
                TriggeredAt = nowLocal
            });
        }

    }
}