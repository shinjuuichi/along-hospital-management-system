using MessageBroker.Contracts.AppointmentContracts;
using QueueSvc.BLL.Interfaces;
using SharedLibrary.Services.Interfaces;
using System.Text.Json;

namespace QueueSvc.BLL.Implements
{
    public class QueueCacheService(
        ICacheService cacheService)
            : IQueueCacheService
    {
        private readonly ICacheService _cacheService = cacheService;

        public async Task<List<GetAppointmentContract>?> GetAppointmentsCacheAsync(DateOnly date)
        {
            var cacheKey = this.GetAppointmentDateCacheKey(date);
            var cachedValue = await _cacheService.GetAsync(cacheKey);

            if (string.IsNullOrWhiteSpace(cachedValue))
            {
                return null;
            }

            return JsonSerializer.Deserialize<List<GetAppointmentContract>>(cachedValue) ?? [];
        }

        public async Task CreateAppointmentsCacheAsync(DateOnly date, List<GetAppointmentContract> appointmentCaches)
        {
            var cacheKey = this.GetAppointmentDateCacheKey(date);
            var payload = JsonSerializer.Serialize(appointmentCaches);

            await _cacheService.SetAsync(cacheKey, payload, TimeSpan.FromHours(24));
        }

        public async Task UpdateAppointmentInCacheAsync(DateOnly date, int appointmentId, GetAppointmentContract appointment)
        {
            var cacheKey = this.GetAppointmentDateCacheKey(date);
            var appointments = await this.GetAppointmentsCacheAsync(date);
            if (appointments == null)
            {
                return;
            }

            var appointmentIndex = appointments.FindIndex(a => a.Id == appointmentId);
            if (appointmentIndex < 0)
            {
                appointments.Add(appointment);
            }
            else
            {
                appointments[appointmentIndex] = appointment;
            }

            var payload = JsonSerializer.Serialize(appointments);
            await _cacheService.SetAsync(cacheKey, payload, TimeSpan.FromHours(24));
        }

        private string GetAppointmentDateCacheKey(DateOnly date)
        {
            return $"queue:appointments:{date:yyyy-MM-dd}";
        }
    }
}
