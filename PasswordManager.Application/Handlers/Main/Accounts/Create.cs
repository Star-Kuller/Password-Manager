using MediatR;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Handlers.Main.Accounts;

public class Create
{
    public record Request(
        string Title,
        string Url,
        string Login, 
        string Password, 
        long DirectoryId): IRequest<long>;

    public class Handler(
        IUnitOfWorkFactory uowFactory,
        ICryptographer cryptographer) : IRequestHandler<Request, long>
    {
        public async Task<long> Handle(Request request, CancellationToken cancellationToken)
        {
            await using var uow = await uowFactory.CreateAsync(cancellationToken);
            var account = new Account()
            {

            };
            throw new NotImplementedException();
        }
    }
}