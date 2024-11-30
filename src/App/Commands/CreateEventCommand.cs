using App.Dto;
using MediatR;

namespace App.Commands;

public record CreateEventCommand(EventDto EventDto) : IRequest<EventDto>;