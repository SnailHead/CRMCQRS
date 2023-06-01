using CRMCQRS.Identity.Endpoints.Permission.ViewModel;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;

namespace CRMCQRS.Identity.Endpoints.Permission.Queries;

public record class UpdateRequest : IRequest<CRMCQRS.Domain.Permission>
{
    public Guid Id { get; set; }
    public PermissionViewModel Model { get; set; }
    
    public  UpdateRequest (Guid id, PermissionViewModel model)
    {
        Id = id;
        Model = model;
    }
}

public class UpdateRequestHandler : IRequestHandler<UpdateRequest, CRMCQRS.Domain.Permission>
{
    private readonly ILogger<UpdateRequestHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public UpdateRequestHandler(ILogger<UpdateRequestHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<CRMCQRS.Domain.Permission> Handle(UpdateRequest request, CancellationToken cancellationToken)
    {
        var result = new CRMCQRS.Domain.Permission();
        var permissionReposytory = _unitOfWork.GetRepository<CRMCQRS.Domain.Permission>();
        
        var permission = await permissionReposytory.GetFirstOrDefaultAsync(
            predicate: x => x.Id == request.Id,
            disableTracking: false);
        if (permission is null)
        {
            return result;
        }
        
        permission.PolicyName = request.Model.PolicyName;
        permission.Description = request.Model.Description;
        permissionReposytory.Update(permission);
        await _unitOfWork.SaveChangesAsync();

        result = permission;
        return result;
    }
}