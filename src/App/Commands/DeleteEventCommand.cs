using MediatR;

namespace App.Commands;

public record DeleteEventCommand(Guid EventId) : IRequest;