namespace ReportSvc.BLL.DTOs.DashboardSharedDTOs
{
    public class DashboardChartDatasetDTO
    {
        public string? Label { get; set; }
        public List<double> Data { get; set; } = [];
    }
}
