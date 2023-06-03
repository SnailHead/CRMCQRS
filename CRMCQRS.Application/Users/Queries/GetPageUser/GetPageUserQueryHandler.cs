using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using CRMCQRS.Infrastructure.Repository;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Application.Users.Queries.GetPageUser;

public class GetPageUserQueryHandler : IRequestHandler<GetPageUserQuery, IPagedList<UserViewModel>>

{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;


    public GetPageUserQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new NullReferenceException("Parameter unitOfWork is null");
        _mapper = mapper ?? throw new NullReferenceException("Parameter mapper is null");
        _userRepository = _unitOfWork.GetRepository<User>();
    }

    public async Task<IPagedList<UserViewModel>> Handle(GetPageUserQuery request,
        CancellationToken cancellationToken)
    {
        var users =
            await _userRepository.GetPagedListAsync(predicate: request.GetExpression(request), pageIndex: request.Page - 1,
                disableTracking: true, cancellationToken: cancellationToken, 
                selector: item => _mapper.Map<UserViewModel>(item));

        return users;
    }
}