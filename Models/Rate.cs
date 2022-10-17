namespace management_api.Models
{
    public class Rate
    {
        public int Id { get; set; }
        
        public int PositionId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public int Amount { get; set; }
    }
}
