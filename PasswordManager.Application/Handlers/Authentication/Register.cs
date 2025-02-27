using MediatR;

namespace PasswordManager.Application.Handlers.Authentication;

public class Register
{
    public record Request(string Secret, string Login, string Password): IRequest<Response>;
    public record Response();
    
    public class Handler : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            return new Response();
        }
    }
}