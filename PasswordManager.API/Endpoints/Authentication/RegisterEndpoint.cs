using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Authentication;

namespace PasswordManager.API.Endpoints.Authentication;

public class RegisterEndpoint(IMediator mediator) : Endpoint<Registration.Request>
{
    public override void Configure()
    {
        Post("/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Registration.Request req, CancellationToken ct)
    {
        await mediator.Send(req, ct);
        
        await SendNoContentAsync(ct);
    }
}

