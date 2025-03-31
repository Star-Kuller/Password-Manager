using FastEndpoints;
using MediatR;
using PasswordManager.Application.Handlers.Main.Accounts;

namespace PasswordManager.API.Endpoints.Main.Accounts;

public class CreateEndpoint(IMediator mediator) : Endpoint<Create.Request>
{
    public override void Configure()
    {
        Post("/account");
    }

    public override async Task HandleAsync(Create.Request req, CancellationToken ct)
    {
        var id = await mediator.Send(req, ct);
        await SendCreatedAtAsync<Directories.GetEndpoint>(new {}, id, cancellation: ct);
    }
}