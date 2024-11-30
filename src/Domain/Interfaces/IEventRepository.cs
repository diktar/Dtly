using Domain.Entities;

namespace Domain.Interfaces;

public interface IEventRepository
{
    Task<Event> GetByIdAsync(Guid id);
    
    Task<List<Event>> GetAllAsync();
    
    Task AddAsync(Event calendarEvent);
    
    Task UpdateAsync(Event calendarEvent);
    
    Task DeleteAsync(Guid id);
}