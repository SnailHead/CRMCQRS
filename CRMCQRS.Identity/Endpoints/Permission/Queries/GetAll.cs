using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Identity.Endpoints.Permission.Queries;

public record class GetAllRequest : IRequest<IList<CRMCQRS.Domain.Permission>>;

public class GetAllRequestHandler : IRequestHandler<GetAllRequest, IList<CRMCQRS.Domain.Permission>>
{
    private readonly ILogger<GetAllRequestHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetAllRequestHandler(ILogger<GetAllRequestHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<IList<CRMCQRS.Domain.Permission>> Handle(GetAllRequest request, CancellationToken cancellationToken)
    {
        var result = new List<CRMCQRS.Domain.Permission>();
        var permissionReposytory = _unitOfWork.GetRepository<CRMCQRS.Domain.Permission>();
        result = (await permissionReposytory.GetAllAsync(true)).ToList();
        
        return result;
    }
}