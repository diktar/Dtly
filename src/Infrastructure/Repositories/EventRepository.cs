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
        try
        {
            _context.Events.Update(calendarEvent);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                if (entry.Entity is Event)
                {
                    var databaseValues = await entry.GetDatabaseValuesAsync();
                    if (databaseValues == null)
                    {
                        throw new DbUpdateConcurrencyException("The event has been deleted.");
                    }

                    var databaseEntity = databaseValues.ToObject() as Event;
                    throw new DbUpdateConcurrencyException($"Concurrency conflict: {databaseEntity?.Title}");
                }
            }
        }
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