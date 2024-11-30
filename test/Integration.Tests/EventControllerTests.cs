using System.Data.Common;
using System.Net.Http.Json;
using App.Dto;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Integration.Tests;

public class EventControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly AppDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public EventControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        var webApplicationFactory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Replace DbContext with in-memory SQLite for testing
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("Data Source=TestCalendar.db"));
            });
        });

        _client = webApplicationFactory.CreateClient();
        _serviceProvider = webApplicationFactory.Services;

        // Ensure database is initialized
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }

    [Fact]
    public async Task CreateEvent_ShouldAddEventToDatabase()
    {
        // Arrange
        var eventDto = new EventDto
        {
            Title = "Test Event",
            Description = "Test Description",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1),
            Attendees = new List<AttendeeDto>
            {
                new() { Name = "John Doe", Email = "john@example.com", IsAttending = true }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Event", eventDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdEvent = await response.Content.ReadFromJsonAsync<EventDto>();
        createdEvent.Should().NotBeNull();
        createdEvent!.Title.Should().Be(eventDto.Title);

        // Verify in database
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var dbEvent = await dbContext.Events.FirstOrDefaultAsync(e => e.Title == "Test Event");
            dbEvent.Should().NotBeNull();
            dbEvent!.Description.Should().Be(eventDto.Description);
        }
    }

    [Fact]
    public async Task GetEventById_ShouldReturnEvent()
    {
        // Arrange
        Event calendarEvent;
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            calendarEvent = new Event
            {
                Title = "Existing Event",
                Description = "Existing Description",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
            dbContext.Events.Add(calendarEvent);
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync($"/api/Event/{calendarEvent.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var eventDto = await response.Content.ReadFromJsonAsync<EventDto>();
        eventDto.Should().NotBeNull();
        eventDto!.Title.Should().Be("Existing Event");
    }

    [Fact]
    public async Task GetAllEvents_ShouldReturnAllEvents()
    {
        // Arrange
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Events.AddRange(
                new Event { Title = "Event 1", Description = "Desc 1", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1) },
                new Event { Title = "Event 2", Description = "Desc 2", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(2) }
            );
            await dbContext.SaveChangesAsync();
        }

        // Act
        var response = await _client.GetAsync("/api/Event");

        // Assert
        response.EnsureSuccessStatusCode();
        var events = await response.Content.ReadFromJsonAsync<List<EventDto>>();
        events.Should().NotBeNull();
        events!.Count.Should().Be(2);
    }

    [Fact]
    public async Task DeleteEvent_ShouldRemoveEventFromDatabase()
    {
        // Arrange
        Guid eventId;
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var calendarEvent = new Event
            {
                Title = "Event to Delete",
                Description = "Description",
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(1)
            };
            dbContext.Events.Add(calendarEvent);
            await dbContext.SaveChangesAsync();
            eventId = calendarEvent.Id;
        }

        // Act
        var response = await _client.DeleteAsync($"/api/Event/{eventId}");

        // Assert
        response.EnsureSuccessStatusCode();

        // Verify in database
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var dbEvent = await dbContext.Events.FindAsync(eventId);
            dbEvent.Should().BeNull();
        }
    }

    [Fact]
public async Task UpdateEvent_ShouldModifyEventInDatabase()
{
    // Arrange
    Guid eventId;
    using (var scope = _serviceProvider.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var calendarEvent = new Event
        {
            Title = "Original Event",
            Description = "Original Description",
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddHours(1)
        };
        dbContext.Events.Add(calendarEvent);
        await dbContext.SaveChangesAsync();
        eventId = calendarEvent.Id;
    }

    // Prepare updated DTO
    var updatedEventDto = new EventDto
    {
        Title = "Updated Event",
        Description = "Updated Description",
        StartTime = DateTime.UtcNow.AddDays(1), // Change StartTime
        EndTime = DateTime.UtcNow.AddDays(1).AddHours(1), // Change EndTime
        Attendees = new List<AttendeeDto>
        {
            new() { Name = "Jane Doe", Email = "jane@example.com", IsAttending = true }
        }
    };

    // Act
    var response = await _client.PutAsJsonAsync($"/api/Event/{eventId}", updatedEventDto);

    // Assert
    response.EnsureSuccessStatusCode();
    var updatedEvent = await response.Content.ReadFromJsonAsync<EventDto>();
    updatedEvent.Should().NotBeNull();
    updatedEvent!.Title.Should().Be("Updated Event");
    updatedEvent.Description.Should().Be("Updated Description");

    // Verify changes in the database
    using (var scope = _serviceProvider.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var dbEvent = await dbContext.Events
            .Include(@event => @event.Attendees)
            .FirstAsync(@event => @event.Id == eventId);
        dbEvent.Should().NotBeNull();
        dbEvent!.Title.Should().Be("Updated Event");
        dbEvent.Description.Should().Be("Updated Description");
        dbEvent.StartTime.Should().Be(updatedEventDto.StartTime);
        dbEvent.EndTime.Should().Be(updatedEventDto.EndTime);
        dbEvent.Attendees.Count.Should().Be(1);
        dbEvent.Attendees[0].Name.Should().Be("Jane Doe");
    }
}
}