using MediatR;

namespace PasswordManager.Application.Handlers.Authentication;

public class Login
{
    public record Request(string Login, string Password): IRequest<Response>;
    public record Response(string SessionId);
    
    public class Handler : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            return new Response("");
        }
    }
}