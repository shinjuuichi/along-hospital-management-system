using SharedLibrary.Base.Mappers;
using WorkScheduleSvc.BLL.DTOs.ShiftDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs
{
    public class GetWorkScheduleDTO : MapFrom<WorkSchedule>
    {
        public int Id { get; set; }

        public DateOnly WorkDate { get; set; }

        public string? WorkScheduleStatus { get; set; }

        public int? WorkScheduleTemplateId { get; set; }

        public int ShiftId { get; set; }

        public GetShiftDTO? Shift { get; set; }

        public List<GetWorkScheduleAssignmentDTO> WorkScheduleAssignments { get; set; } = [];
    }
}