using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.API.Endpoints.Authentication;

public class LogoutEndpoint(ISessionManager sessionManager) : Endpoint<Registration.Request>
{
    public override void Configure()
    {
        Post("/logout");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Registration.Request req, CancellationToken ct)
    {
        await sessionManager.EndSession();
        await SendNoContentAsync(ct);
    }
}

