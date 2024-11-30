using App.Commands;
using Domain.Interfaces;
using MediatR;

namespace App.Handlers;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public DeleteEventCommandHandler(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        await _eventRepository.DeleteAsync(request.EventId);
    }
}