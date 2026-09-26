namespace ReportSvc.BLL.FilterDTOs
{
    public class DashboardDateRangeFilterDTO
    {
        private DateOnly _fromDate = new(DateTime.Today.Year, 1, 1);
        private DateOnly _toDate = new(DateTime.Today.Year, 12, 31);

        public DateOnly FromDate
        {
            get => _fromDate <= _toDate ? _fromDate : _toDate;
            init => _fromDate = value;
        }

        public DateOnly ToDate
        {
            get => _fromDate <= _toDate ? _toDate : _fromDate;
            init => _toDate = value;
        }
    }
}
