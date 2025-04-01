using FluentValidation;
using MediatR;
using PasswordManager.Application.Exceptions;
using PasswordManager.Application.Interfaces;
using PasswordManager.Application.Interfaces.Database;
using PasswordManager.Application.Models.Encrypted;
using PasswordManager.Domain.Entities;
using Directory = PasswordManager.Domain.Entities.Directory;

namespace PasswordManager.Application.Handlers.Main.Directories;

public class Create
{
    public record Request(
        string Title,
        long ParentId): IRequest<long>;

    public class Handler(
        ISessionManager sessionManager,
        IUnitOfWorkFactory uowFactory,
        ICryptographer cryptographer) : IRequestHandler<Request, long>
    {
        public async Task<long> Handle(Request request, CancellationToken cancellationToken)
        {
            await using var uow = await uowFactory.CreateAsync(cancellationToken);
            
            var parent = await GetParent(uow, request.ParentId);
            if (parent.OwnerId != sessionManager.GetCurrentUserId())
                throw new ForbiddenException("Нет доступа к родительской директории");
                    
            var directory = new Directory()
            {
                Title = request.Title,
                OwnerId = sessionManager.GetCurrentUserId()!.Value,
                ParentId = request.ParentId
            };
            var id = await uow.Directories.AddAsync(new EncryptedDirectory(directory, cryptographer));
            
            await uow.CommitAsync(cancellationToken);
            return id;
        }

        private async Task<Directory> GetParent(IUnitOfWork uow, long id)
        {
            var parent = await uow.Directories.GetAsync(id);
            if (parent is null)
                throw new NotFoundException("Не найдена родительская директория");
            return parent.ToEntity(cryptographer);
        }
    }
}