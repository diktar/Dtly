namespace Domain.Entities;

public class Attendee
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public bool IsAttending { get; set; }
    
    public Guid EventId { get; set; }
    public Event Event { get; set; }
}