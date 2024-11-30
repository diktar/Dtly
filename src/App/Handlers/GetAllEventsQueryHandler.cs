using App.Dto;
using App.Queries;
using AutoMapper;
using Domain.Interfaces;
using MediatR;

namespace App.Handlers;

public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<EventDto>>
{
    private readonly IEventRepository _eventRepository;
    private readonly IMapper _mapper;

    public GetAllEventsQueryHandler(IEventRepository eventRepository, IMapper mapper)
    {
        _eventRepository = eventRepository;
        _mapper = mapper;
    }

    public async Task<List<EventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
    {
        var events = await _eventRepository.GetAllAsync();
        
        return _mapper.Map<List<EventDto>>(events);
    }
}