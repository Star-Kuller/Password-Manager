using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Authentication;

namespace PasswordManager.API.Endpoints.Authentication;

public class LoginEndpoint(IMediator mediator) : Endpoint<Login.Request, Login.Response>
{
    public override void Configure()
    {
        Post("/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Login.Request req, CancellationToken ct)
    {
        var response = await mediator.Send(req, ct);
        await SendAsync(response, cancellation: ct);
    }
}