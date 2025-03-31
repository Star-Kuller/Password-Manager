using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Authentication;

namespace PasswordManager.API.Endpoints.Authentication;

public class LoginEndpoint(IMediator mediator) : Endpoint<Login.Request>
{
    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Login.Request req, CancellationToken ct)
    {
        await mediator.Send(req, ct);
    }
}