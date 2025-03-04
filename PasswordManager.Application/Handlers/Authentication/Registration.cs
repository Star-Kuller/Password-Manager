using MediatR;
using PasswordManager.Application.Exceptions;
using PasswordManager.Application.Interfaces;
using PasswordManager.Domain.Entities;

namespace PasswordManager.Application.Handlers.Authentication;

public class Registration
{
    public record Request(string Secret, string Email, string Password): IRequest<string>;
    
    public class Handler(
        IUserRepository userRepository,
        ICryptographer cryptographer,
        ISessionManager sessionManager) : IRequestHandler<Request, string>
    {
        public async Task<string> Handle(Request request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetAsync(request.Email);
            if (user is not null)
                throw new AlreadyExistException($"Пользователь с почтой {request.Email} уже зарегистрирован");
            
            user = new User
            {
                Email = cryptographer.Encrypt(request.Email),
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
                SecretKey = cryptographer.Encrypt(request.Secret)
            };
            var id = await userRepository.AddAsync(user);

            return sessionManager.CreateSession(id);
        }
    }
}