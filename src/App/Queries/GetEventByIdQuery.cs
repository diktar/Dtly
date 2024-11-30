using App.Dto;
using MediatR;

namespace App.Queries;

public record GetEventByIdQuery(Guid EventId) : IRequest<EventDto>;