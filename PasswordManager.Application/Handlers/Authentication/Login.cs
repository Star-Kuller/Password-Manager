using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;

namespace PasswordManager.Application.Handlers.Authentication;

public class Login
{
    public record Request(string Login, string Password): IRequest;
    
    public class Handler(
        IUnitOfWorkFactory uowFactory,
        ISessionManager sessionManager,
        ICryptographer cryptographer,
        ILogger<Handler> logger) : IRequestHandler<Request>
    {
        public async Task Handle(Request request, CancellationToken cancellationToken)
        {
            var uow = await uowFactory.CreateAsync(cancellationToken);
            var encryptedUser = await uow.Users.GetAsync(cryptographer.Encrypt(request.Login));
            if (encryptedUser is null)
                throw new ValidationException("Неверный логин или пароль");
            var user = encryptedUser.ToUser(cryptographer);
            if (BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.PasswordHash))
                await sessionManager.CreateSession(user.Id!.Value);
            else
                throw new ValidationException("Неверный логин или пароль");
        }
    }
}