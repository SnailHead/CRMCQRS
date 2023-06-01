using CRMCQRS.Domain;
using CRMCQRS.Identity.Endpoints.Permission.ViewModel;
using CRMCQRS.Infrastructure.UnitOfWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRMCQRS.Identity.Endpoints.Permission.Queries;

public record class RemoveInProfileRequest : IRequest<ProfilePermissionViewModel>
{
    public ProfilePermissionViewModel Model { get; set; }
    
    public RemoveInProfileRequest(ProfilePermissionViewModel model)
    {
        Model = model;
    }
}

public class RemoveInProfileRequestHandler : IRequestHandler<RemoveInProfileRequest, ProfilePermissionViewModel>
{
    private readonly ILogger<RemoveInProfileRequestHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    public RemoveInProfileRequestHandler(ILogger<RemoveInProfileRequestHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProfilePermissionViewModel> Handle(RemoveInProfileRequest request, CancellationToken cancellationToken)
    {
        var result = new ProfilePermissionViewModel();
        var permissionRepository = _unitOfWork.GetRepository<CRMCQRS.Domain.Permission>();
        var userRepository = _unitOfWork.GetRepository<User>();
        
        var user = await userRepository.GetFirstOrDefaultAsync(
            include: x => x.Include(i => i.Permissions),
            predicate: x => x.Id == request.Model.ProfileId,
            disableTracking: false);
        if (user is null)
        {
            var message = $"profile by id {request.Model.ProfileId} not found";
            _logger.LogError(message);
            return result;
        }

        var permission = await permissionRepository.GetFirstOrDefaultAsync(predicate: x =>
                x.Id == request.Model.PermissionId,
            disableTracking: false);
        if (permission is null)
        {
            var message = $"permission by id {request.Model.PermissionId} not found";
            _logger.LogError(message);
            return result;
        }
        
        var answer = user.Permissions.Remove(permission);
        if (!answer)
        {
            var message = $"permission {request.Model.PermissionId} dont remove in profile {request.Model.ProfileId}";
            _logger.LogError(message);
            return result;
        }
        
        result = request.Model;
        await _unitOfWork.SaveChangesAsync();

        return result;
    }
}