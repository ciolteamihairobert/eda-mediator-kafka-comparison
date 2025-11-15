using MediatR;

public record PingCommand() : IRequest<string>;
