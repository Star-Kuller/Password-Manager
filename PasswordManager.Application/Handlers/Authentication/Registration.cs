using MediatR;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.Application.Handlers.Authentication;

public class Registration
{
    public record Request(string Secret, string Email, string Password): IRequest<Response>;
    public record Response(string SessionId);
    
    public class Handler(
        IUserRepository userRepository,
        ICryptographer cryptographer,
        ISessionManager sessionManager) : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            return new Response("");
        }
    }
}