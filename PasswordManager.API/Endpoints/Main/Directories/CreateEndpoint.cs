using FastEndpoints;
using MediatR;
using PasswordManager.API.Models;
using PasswordManager.Application.Handlers.Main.Directories;

namespace PasswordManager.API.Endpoints.Main.Directories;

public class CreateEndpoint(IMediator mediator) : Endpoint<Create.Request, IdResponse>
{
    public override void Configure()
    {
        Post("/directory");
    }

    public override async Task HandleAsync(Create.Request req, CancellationToken ct)
    {
        var id = await mediator.Send(req, ct);
        await SendCreatedAtAsync<GetEndpoint>(new {}, new IdResponse(id), cancellation: ct);
    }
}