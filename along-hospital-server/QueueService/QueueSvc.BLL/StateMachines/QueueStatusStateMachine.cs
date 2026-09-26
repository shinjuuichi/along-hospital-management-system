using QueueSvc.BLL.Interfaces;
using QueueSvc.DAL.Enums;
using QueueSvc.DAL.Models;
using Stateless;

namespace QueueSvc.BLL.StateMachines
{
    public class QueueStatusStateMachine
    {
        private const int MAX_CALLS_BEFORE_END_OF_QUEUE = 10;

        private readonly Queue _queue;
        private readonly IQueueQueryService _queueQueryService;
        private readonly IQueueMessageBusService _queueMessageBusService;
        private readonly StateMachine<QueueStatusEnum, QueueStatusEnum> _stateMachine;

        public QueueStatusStateMachine(Queue queue,
            IQueueQueryService queueQueryService,
            IQueueMessageBusService queueMessageBusService)
        {
            _queue = queue;
            _queueQueryService = queueQueryService;
            _queueMessageBusService = queueMessageBusService;
            _stateMachine = new StateMachine<QueueStatusEnum, QueueStatusEnum>(
                () => _queue.QueueStatus,
                s => _queue.QueueStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(QueueStatusEnum.Waiting)
                .Permit(QueueStatusEnum.Called, QueueStatusEnum.Called)
                .Permit(QueueStatusEnum.Cancelled, QueueStatusEnum.Cancelled)
                .OnEntryAsync(async transition =>
                {
                    if (transition.Source == QueueStatusEnum.Called)
                    {
                        /* Recalculate QueueOrder based on the number of calls and the current position in the queue
                         * Formula: newOrder = currentOrder + ((endOrder - currentOrder) * (numberOfCalls / MAX_CALLS_BEFORE_END_OF_QUEUE))
                         * Example: currentOrder = 2, endOrder = 10, numberOfCalls = 2 => newOrder = 2 + ((10 - 2) * (2 / 10)) = 2 + (8 * 0.2) = 2 + 1.6 = 3.6 => newOrder = 4
                         */
                        (_, var maxOrder) = await _queueQueryService.GetQueueAggregatesTodayAsync(_queue.RoomId);
                        var endOrder = Math.Max(maxOrder, _queue.QueueOrder);

                        var calls = Math.Clamp(_queue.NumberOfCalls, 1, MAX_CALLS_BEFORE_END_OF_QUEUE);
                        var ratio = calls / (double)MAX_CALLS_BEFORE_END_OF_QUEUE;
                        var projectedOrder = _queue.QueueOrder + (int)Math.Ceiling((endOrder - _queue.QueueOrder) * ratio);

                        _queue.Priority = 0;
                        _queue.QueueOrder = Math.Max(1, projectedOrder);
                        return;
                    }

                    if (transition.Source == QueueStatusEnum.AwaitingResults)
                    {
                        // When a queue is moved back to Waiting from AwaitingResults, increase its priority and move it to the end of the new priority level
                        var newPriority = _queue.Priority + 1;
                        var lastPriorityOrder = await _queueQueryService.GetLastQueueOrderByPriorityAsync(_queue.RoomId, newPriority);

                        _queue.NumberOfCalls = 0;
                        _queue.Priority = newPriority;
                        _queue.QueueOrder = lastPriorityOrder != 0 && lastPriorityOrder != _queue.QueueOrder
                                    ? lastPriorityOrder + 1
                                    : _queue.QueueOrder;
                        _queue.IsReEntry = true;
                    }
                });

            _stateMachine.Configure(QueueStatusEnum.Called)
                .Permit(QueueStatusEnum.InProgress, QueueStatusEnum.InProgress)
                .Permit(QueueStatusEnum.Waiting, QueueStatusEnum.Waiting)
                .Permit(QueueStatusEnum.Cancelled, QueueStatusEnum.Cancelled)
                .OnEntry(() =>
                {
                    _queue.NumberOfCalls++;
                });

            _stateMachine.Configure(QueueStatusEnum.InProgress)
                .Permit(QueueStatusEnum.AwaitingResults, QueueStatusEnum.AwaitingResults)
                .Permit(QueueStatusEnum.Completed, QueueStatusEnum.Completed)
                .Permit(QueueStatusEnum.Cancelled, QueueStatusEnum.Cancelled)
                .OnEntryAsync(async () =>
                {
                    if (!_queue.RoomId.HasValue || !_queue.MedicalHistoryId.HasValue)
                    {
                        return;
                    }

                    var doctorId = await _queueMessageBusService.GetTodayWorkingDoctorByRoomIdAsync(_queue.RoomId.Value);
                    if (!doctorId.HasValue)
                    {
                        return;
                    }

                    await _queueMessageBusService.AssignDoctorToMedicalHistoryAsync(_queue.MedicalHistoryId.Value, doctorId.Value);
                });

            _stateMachine.Configure(QueueStatusEnum.AwaitingResults)
                .Permit(QueueStatusEnum.Waiting, QueueStatusEnum.Waiting)
                .Permit(QueueStatusEnum.Cancelled, QueueStatusEnum.Cancelled);
        }

        public bool CanFire(QueueStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public async Task FireAsync(QueueStatusEnum trigger)
        {
            await _stateMachine.FireAsync(trigger);
        }
    }
}
