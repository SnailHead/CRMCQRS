using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Identity.Endpoints.Permission.Queries;

public record class GetByIdRequest : IRequest<CRMCQRS.Domain.Permission>
{ 
    public Guid Model { get; }

    public GetByIdRequest(Guid model) => Model = model;
}

public class GetByIdRequestHandler : IRequestHandler<GetByIdRequest, CRMCQRS.Domain.Permission>
{
    private readonly ILogger<GetByIdRequestHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public GetByIdRequestHandler(ILogger<GetByIdRequestHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<CRMCQRS.Domain.Permission> Handle(GetByIdRequest request, CancellationToken cancellationToken)
    {
        var result = new CRMCQRS.Domain.Permission();
        var permissionReposytory = _unitOfWork.GetRepository<CRMCQRS.Domain.Permission>();
        result = await permissionReposytory.GetFirstOrDefaultAsync(predicate: x=> x.Id == request.Model );
        return result;
    }
}