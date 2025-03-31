using FastEndpoints;
using MediatR;

namespace PasswordManager.API.Endpoints.Main.Directories;

public class GetEndpoint(IMediator mediator) : Endpoint<object>
{
    public override void Configure()
    {
        Get("/directory");
    }

    public override async Task HandleAsync(object req, CancellationToken ct)
    {
       
    }
}