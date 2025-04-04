using FastEndpoints;
using MediatR;
using PasswordManager.API.Models;
using PasswordManager.Application.Handlers.Main.Accounts;

namespace PasswordManager.API.Endpoints.Main.Accounts;

public class CreateEndpoint(IMediator mediator) : Endpoint<Create.Request, IdResponse>
{
    public override void Configure()
    {
        Post("/account");
    }

    public override async Task HandleAsync(Create.Request req, CancellationToken ct)
    {
        var id = await mediator.Send(req, ct);
        await SendCreatedAtAsync<Directories.GetEndpoint>(new {}, new IdResponse(id), cancellation: ct);
    }
}