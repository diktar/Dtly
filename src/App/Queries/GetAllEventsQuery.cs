using App.Dto;
using MediatR;

namespace App.Queries;

public record GetAllEventsQuery() : IRequest<List<EventDto>>;