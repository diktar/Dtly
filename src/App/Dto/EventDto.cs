namespace App.Dto;

public class EventDto
{
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public List<AttendeeDto> Attendees { get; set; } = new();
    
    public byte[] RowVersion { get; set; }
}