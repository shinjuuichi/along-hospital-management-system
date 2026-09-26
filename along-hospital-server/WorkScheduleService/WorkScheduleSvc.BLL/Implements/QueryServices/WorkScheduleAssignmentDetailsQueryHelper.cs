using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Gateways;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.BLL.Implements.QueryServices
{
    public class WorkScheduleAssignmentDetailsQueryHelper(IWorkScheduleAssignmentGateway workScheduleAssignmentGateway)
    {
        private readonly IWorkScheduleAssignmentGateway _workScheduleAssignmentGateway = workScheduleAssignmentGateway;

        public async Task PopulateAssignmentDetailsAsync(List<GetWorkScheduleAssignmentDTO> assignmentDTOs)
        {
            if (assignmentDTOs.Count == 0)
            {
                return;
            }

            var staffIds = assignmentDTOs
                .Select(x => x.StaffId)
                .Distinct()
                .ToList();
            var roomIds = assignmentDTOs
                .Where(x => string.Equals(x.LocationType, nameof(LocationTypeEnum.Room), StringComparison.OrdinalIgnoreCase))
                .Select(x => x.LocationId)
                .Distinct()
                .ToList();
            var teleRoomIds = assignmentDTOs
                .Where(x => string.Equals(x.LocationType, nameof(LocationTypeEnum.TeleRoom), StringComparison.OrdinalIgnoreCase))
                .Select(x => x.LocationId)
                .Distinct()
                .ToList();

            var staffTasks = staffIds.ToDictionary(
                x => x,
                x => _workScheduleAssignmentGateway.GetStaffAsync(x));
            var roomTasks = roomIds.ToDictionary(
                x => x,
                x => _workScheduleAssignmentGateway.GetRoomAsync(x));
            var teleRoomTasks = teleRoomIds.ToDictionary(
                x => x,
                x => _workScheduleAssignmentGateway.GetTeleRoomAsync(x));

            await Task.WhenAll(staffTasks.Values);
            await Task.WhenAll(roomTasks.Values);
            await Task.WhenAll(teleRoomTasks.Values);

            var staffLookup = staffTasks.ToDictionary(x => x.Key, x => x.Value.Result);
            var roomLookup = roomTasks.ToDictionary(x => x.Key, x => x.Value.Result);
            var teleRoomLookup = teleRoomTasks.ToDictionary(x => x.Key, x => x.Value.Result);

            foreach (var assignmentDTO in assignmentDTOs)
            {
                if (staffLookup.TryGetValue(assignmentDTO.StaffId, out var staffDTO))
                {
                    assignmentDTO.Staff = staffDTO;
                }

                if (string.Equals(assignmentDTO.LocationType, nameof(LocationTypeEnum.Room), StringComparison.OrdinalIgnoreCase))
                {
                    if (roomLookup.TryGetValue(assignmentDTO.LocationId, out var roomDTO))
                    {
                        assignmentDTO.Room = roomDTO;
                    }
                    continue;
                }

                if (teleRoomLookup.TryGetValue(assignmentDTO.LocationId, out var teleRoomDTO))
                {
                    assignmentDTO.TeleRoom = teleRoomDTO;
                }
            }
        }
    }
}