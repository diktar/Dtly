using App.Dto;

namespace App.Interfaces;

public interface IEventService
{
    Task<EventDto> CreateEventAsync(EventDto eventDto);
    
    Task<EventDto> UpdateEventAsync(Guid id, EventDto eventDto);
    
    Task DeleteEventAsync(Guid id);
    
    Task<List<EventDto>> GetAllEventsAsync();
    
    Task<EventDto> GetEventByIdAsync(Guid id);
}