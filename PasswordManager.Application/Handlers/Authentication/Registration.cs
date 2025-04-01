using MediatR;
using Microsoft.Extensions.Logging;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Domain.Entities;
using Directory = PasswordManager.Domain.Entities.Directory;

namespace PasswordManager.Application.Handlers.Authentication;

public class Registration
{
    public record Request(string Secret, string Login, string Password): IRequest;
    
    public class Handler(
        IUnitOfWorkFactory uowFactory,
        ISessionManager sessionManager,
        ICryptographer cryptographer,
        ILogger<Handler> logger) : IRequestHandler<Request>
    {
        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            await using var uow = await uowFactory.CreateAsync(cancellationToken);
            var user = new User
            {
                Login = request.Login,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                SecretKey = request.Secret
            };
            
            user.Id = await uow.Users.AddAsync(new EncryptedUser(user, cryptographer));
            
            var directory = new Directory
            {
                Title = "root",
                OwnerId = user.Id.Value,
                ParentId = null
            };
            user.RootDirectoryId = await uow.Directories.AddAsync(new EncryptedDirectory(directory, cryptographer));

            await uow.Users.UpdateAsync(new EncryptedUser(user, cryptographer));
            
            await uow.CommitAsync(cancellationToken);
            await sessionManager.CreateSession(user.Id.Value);
            logger.LogInformation("{UserLogin} зарегистрировался в системе", user.Login);
        }
    }
}