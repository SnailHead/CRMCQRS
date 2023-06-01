using System.Security.Claims;
using CRMCQRS.Identity.Endpoints.Account.ViewModel;
using MediatR;

namespace CRMCQRS.Identity.Endpoints.Account.Queries;

public record GetClaimsRequest : IRequest<List<ClaimsViewModel>>;

public class GetClaimsRequestHandler : IRequestHandler<GetClaimsRequest, List<ClaimsViewModel>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<GetClaimsRequestHandler> _logger;
    
    public GetClaimsRequestHandler(IHttpContextAccessor httpContextAccessor, ILogger<GetClaimsRequestHandler> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public Task<List<ClaimsViewModel>> Handle(GetClaimsRequest request, CancellationToken cancellationToken)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var claims = ((ClaimsIdentity)user.Identity!).Claims;
        var resultClaims = claims.Select(x => new ClaimsViewModel { Type = x.Type, ValueType = x.ValueType, Value = x.Value });
        var result = new List<ClaimsViewModel>(resultClaims);
        
        _logger.LogInformation($"Current user {user.Identity.Name} have following climes {result}");
        return Task.FromResult(result);
    }
}