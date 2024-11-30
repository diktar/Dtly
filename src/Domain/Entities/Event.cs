namespace Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public List<Attendee> Attendees { get; set; } = new();
}