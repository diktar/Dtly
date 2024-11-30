using App.Dto;
using MediatR;

namespace App.Commands;

public record UpdateEventCommand(Guid EventId, EventDto EventDto) : IRequest<EventDto>;