namespace ReportSvc.BLL.DTOs.DashboardSharedDTOs
{
    public class DashboardChartDTO
    {
        public List<string> Labels { get; set; } = [];
        public List<DashboardChartDatasetDTO> Datasets { get; set; } = [];
    }
}
