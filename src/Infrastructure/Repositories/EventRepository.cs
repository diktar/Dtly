using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Event> GetByIdAsync(Guid id) 
        => await _context.Events.FindAsync(id);

    public async Task<List<Event>> GetAllAsync() 
        => await _context.Events.ToListAsync();

    public async Task AddAsync(Event calendarEvent)
    {
        await _context.Events.AddAsync(calendarEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event calendarEvent)
    {
        _context.Events.Update(calendarEvent);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var calendarEvent = await GetByIdAsync(id);
        
        if (calendarEvent != null)
        {
            _context.Events.Remove(calendarEvent);
            await _context.SaveChangesAsync();
        }
    }
}