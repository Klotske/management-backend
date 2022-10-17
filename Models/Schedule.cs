namespace management_api.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        
        public int DepartmentId { get; set; }
        
        public Department? Department { get; set; }
        
        public int PositionId { get; set; }
        
        public Position? Position { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public int Quantity { get; set; }
    }
}
