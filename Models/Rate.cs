namespace management_api.Models
{
    public class Rate
    {
        public int Id { get; set; }
        
        public int PositionId { get; set; }
        
        public Position? Position { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public int Amount { get; set; }
    }

    public class IntervalRate
    {
        public int PositionId { get; set; }
        
        public Position? Position { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public int Amount { get; set; }
    }
}
