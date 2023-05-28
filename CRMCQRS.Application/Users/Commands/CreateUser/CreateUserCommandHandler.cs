using AutoMapper;
using CRMCQRS.Application.Users.Commands.CreateUser;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler :IRequestHandler<CreateUserCommand, Guid>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _userRepository = _unitOfWork.GetRepository<User>();
    }

    public async Task<Guid> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request);
        await _userRepository.InsertAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync();
        
        return user.Id;
    }
}