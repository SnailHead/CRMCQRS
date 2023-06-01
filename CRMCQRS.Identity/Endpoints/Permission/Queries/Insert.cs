using AutoMapper;
using CRMCQRS.Identity.Endpoints.Permission.ViewModel;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Identity.Endpoints.Permission.Queries;

public record class InsertRequest : IRequest<CRMCQRS.Domain.Permission>
{
    public PermissionViewModel Model { get; set; }

    public InsertRequest(PermissionViewModel model)
    {
        Model = model;
    }
}

public class InsertRequestHandler : IRequestHandler<InsertRequest, CRMCQRS.Domain.Permission>
{
    private readonly ILogger<InsertRequestHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public InsertRequestHandler(ILogger<InsertRequestHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CRMCQRS.Domain.Permission> Handle(InsertRequest request, CancellationToken cancellationToken)
    {
        var result = new CRMCQRS.Domain.Permission();
        var permissionReposytory = _unitOfWork.GetRepository<CRMCQRS.Domain.Permission>();
        var perrmision = await permissionReposytory.InsertAsync(_mapper.Map<CRMCQRS.Domain.Permission>(request.Model));
        await _unitOfWork.SaveChangesAsync();
        result = perrmision.Entity;

        return result;
    }
}