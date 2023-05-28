using CRMCQRS.Application.Users.Commands.DeleteUser;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler :IRequestHandler<DeleteUserCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;

    public DeleteUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _userRepository = _unitOfWork.GetRepository<User>();
    }

    public async Task<bool> Handle(DeleteUserCommand request,
        CancellationToken cancellationToken)
    {

        var dbUser = await _userRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == request.Id);
        
        if (dbUser is null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found");
        }
        
        _userRepository.Delete(dbUser);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}