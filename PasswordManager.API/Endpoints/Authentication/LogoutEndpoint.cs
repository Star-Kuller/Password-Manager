using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Authentication;
using PasswordManager.Application.Interfaces;

namespace PasswordManager.API.Endpoints.Authentication;

public class LogoutEndpoint(ISessionManager sessionManager) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Put("/logout");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await sessionManager.EndSession();
        await SendNoContentAsync(ct);
    }
}

