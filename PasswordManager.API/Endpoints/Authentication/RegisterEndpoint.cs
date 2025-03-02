using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Authentication;

namespace PasswordManager.API.Endpoints.Authentication;

public class RegisterEndpoint(IMediator mediator) : Endpoint<Registration.Request, Registration.Response>
{
    public override void Configure()
    {
        Post("/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Registration.Request req, CancellationToken ct)
    {
        var response = await mediator.Send(req, ct);
        await SendAsync(response, StatusCodes.Status201Created, ct);
    }
}

