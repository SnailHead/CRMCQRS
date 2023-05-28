using AutoMapper;
using CRMCQRS.Application.Users.Commands.UpdateUser;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler :IRequestHandler<UpdateUserCommand, bool>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _userRepository = _unitOfWork.GetRepository<User>();
    }

    public async Task<bool> Handle(UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        bool exists = await _userRepository.ExistsAsync(item => item.Id == request.Id, cancellationToken);
        if (!exists)
        {
            throw new ArgumentException($"User with ID {request.Id} not found");
        }

        var user = _mapper.Map<User>(request);
        
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
        
        return true;
    }
}