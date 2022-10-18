namespace management_api.Models
{
    public class Report
    {
        public DateTime Start { get; set; }
        
        public DateTime End { get; set; }
        
        public List<ReportMonthItem> Months { get; set; } = new List<ReportMonthItem>();
    }

    public class ReportMonthItem
    {
        public DateTime Start { get; set; }
        
        public DateTime End { get; set; }

        public List<ReportDepartmentItem> Departments { get; set; } = new List<ReportDepartmentItem>();
    }

    public class ReportDepartmentItem
    {
        public Department Department { get; set; }
        
        public int MonthTotal { get; set; }
    }
}
