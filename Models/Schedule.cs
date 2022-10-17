namespace management_api.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        
        public int DepartmentId { get; set; }
        
        public int PositionId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public int Quantity { get; set; }
    }
}
