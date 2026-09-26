using MassTransit;
using MessageBroker.Abstractions;
using SharedLibrary.Utils;

namespace SharedLibrary.Base.MessageBuses
{
    public interface IMessageBus
    {
        /// <summary>
        /// Sử dụng Publish khi muốn gửi một sự kiện (event) đến tất cả các queue 
        /// đã đăng ký lắng nghe loại Event đó.
        /// <para>
        /// LƯU Ý:
        ///   Các consumer chỉ cần lắng nghe đúng loại Event (message type), 
        ///   không yêu cầu tên consumer phải khớp với tên Event.
        /// </para>
        /// </summary>
        Task PublishAsync<T>(T @event) where T : BaseEvent;

        /// <summary>
        /// Sử dụng Send khi muốn gửi message tới <b>một queue cụ thể</b>.
        /// <para>
        /// LƯU Ý:
        ///   Consumer bắt buộc phải đặt tên theo quy tắc.
        ///   Ví dụ: Event "SendMailEvent" → Consumer "SendMailConsumer".
        /// </para>
        /// </summary>
        Task SendAsync<T>(T command) where T : BaseEvent;

        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : BaseEvent
            where TResponse : BaseContract;
    }

    public class MessageBus(
        IPublishEndpoint publishEndpoint,
        ISendEndpointProvider sendEndpointProvider,
        IScopedClientFactory scopedClientFactory) : IMessageBus
    {
        public async Task PublishAsync<T>(T @event) where T : BaseEvent
        {
            await publishEndpoint.Publish(@event);
        }

        public async Task SendAsync<T>(T command) where T : BaseEvent
        {
            var queueName = typeof(T).Name.ToQueueName();
            var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queueName}"));
            await endpoint.Send(command);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : BaseEvent
            where TResponse : BaseContract
        {
            try
            {
                var client = scopedClientFactory.CreateRequestClient<TRequest>();
                var response = await client.GetResponse<TResponse>(request);
                var message = response.Message;
                message.EnsureSuccess();
                return message;
            }
            catch (RequestTimeoutException ex)
            {
                throw new TimeoutException(
                    $"Request to service handling '{typeof(TRequest).Name}' timed out. " +
                    $"The service may be unavailable or taking too long to respond.",
                    ex);
            }
            catch (RequestFaultException ex)
            {
                throw new InvalidOperationException(
                    $"Request to service handling '{typeof(TRequest).Name}' failed. " +
                    $"Error: {ex.Message}",
                    ex);
            }
        }
    }
}
