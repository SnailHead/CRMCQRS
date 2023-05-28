using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserViewModel>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;


    public GetUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _userRepository = _unitOfWork.GetRepository<User>();
    }

    public async Task<UserViewModel> Handle(GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var dbUser = await _userRepository.GetFirstOrDefaultAsync(predicate: item => item.Id == request.Id);
        if (dbUser is null)
        {
            throw new ArgumentException($"User with ID {request.Id} not found");
        }

        return _mapper.Map<UserViewModel>(dbUser);
    }
}