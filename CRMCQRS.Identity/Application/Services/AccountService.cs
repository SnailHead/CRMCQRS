using System.Security.Claims;
using AutoMapper;
using CRMCQRS.Domain;
using CRMCQRS.Identity.Endpoints.Account.ViewModel;
using CRMCQRS.Infrastructure.Database;
using CRMCQRS.Infrastructure.Roles;
using CRMCQRS.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace CRMCQRS.Identity.Application.Services;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork<DefaultDbContext> _unitOfWork;
    private readonly ILogger<AccountService> _logger;
    private readonly ApplicationUserClaimsPrincipalFactory _claimsFactory;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public AccountService(
        UserManager<User> userManager,
        RoleManager<Role> roleManager, 
        IUnitOfWork<DefaultDbContext> unitOfWork,
        ILogger<AccountService> logger,
        ApplicationUserClaimsPrincipalFactory claimsFactory,
        IHttpContextAccessor httpContext,
        IMapper mapper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _claimsFactory = claimsFactory;
        _httpContext = httpContext;
        _mapper = mapper;
    }
    
    public Guid GetCurrentUserId()
    {
        var identity = _httpContext.HttpContext?.User.Identity;
        var identitySub = (identity is ClaimsIdentity claimsIdentity ? claimsIdentity.FindFirst("sub") : (Claim)null)
                          ?? throw new InvalidOperationException("sub claim is missing");
        
        Guid result;
        Guid.TryParse(identitySub.Value, out result);
        return result;
    }

    public async Task<UserAccountViewModel> RegisterAsync(RegisterViewModel model, CancellationToken cancellationToken)
    {
        var result = new UserAccountViewModel();
        //todo проблема с маппингом посмотреть в будущем
        var user = new User()
        {
            Email = model.Email,
            UserName = model.UserName,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            BirthDate = model.BirthDate,
            PhoneNumber = model.PhoneNumber,
        };
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        var createResult = await _userManager.CreateAsync(user, model.Password);
        const string role = Roles.User;

        if (createResult.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                return result;
            }
            var roleResult = await _userManager.AddToRoleAsync(user, role);
            if (!roleResult.Succeeded)
            {
                var errorsRole = roleResult.Errors.Select(x => $"{x.Code}: {x.Description}").ToList();
            }
            //await AddClaimsToUser(user, role);


            var permission = new Permission()
            {
                PolicyName = "User:View",
                Description = "Standart user polycy for minimal view"
            };
            
            var permissionRepository = _unitOfWork.GetRepository<Permission>();

            var currentPermission =
                await permissionRepository.GetFirstOrDefaultAsync(predicate: x =>
                    x.PolicyName == permission.PolicyName, disableTracking: false);

            if (currentPermission is null)
            {
                await permissionRepository.InsertAsync(permission, cancellationToken);
            }
            
            
            await _unitOfWork.SaveChangesAsync();
            if (_unitOfWork.LastSaveChangesResult.IsOk)
            {
                user.Permissions.Add(permission);
                await _userManager.UpdateAsync(user);
                
                var principal = await _claimsFactory.CreateAsync(user);
                result = _mapper.Map<UserAccountViewModel>(principal.Identity);
              
                await transaction.CommitAsync(cancellationToken);
                _logger.LogInformation($"User registration: email:{model.Email} | {_unitOfWork.LastSaveChangesResult.Exception} ");
                return result;
            }
        }
        await transaction.RollbackAsync(cancellationToken);
        
        var errors = createResult.Errors.Select(x => $"{x.Code}: {x.Description}").ToList();
        _logger.LogInformation($"User dont register: email:{model.Email} | errors: {string.Join(", ", errors)} | {_unitOfWork.LastSaveChangesResult.Exception} ");

        return result;
    }

    public async Task<ClaimsPrincipal> GetPrincipalByIdAsync(string identifier)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentNullException(nameof(identifier));
        }
        
        var user = await _userManager.FindByIdAsync(identifier);
        if (user == null)
        {
            throw new Exception("user not found");
        }

        var defaultClaims = await _claimsFactory.CreateAsync(user);
        return defaultClaims;
    }

    public Task<ClaimsPrincipal> GetPrincipalForUserAsync(User user) => _claimsFactory.CreateAsync(user);

    public async Task<User> GetByIdAsync(Guid id)
    {
        var result = new User();
        var user = await _userManager.FindByIdAsync(id.ToString());
        result = user;
        
        return result;
    }

    public async Task<User> GetCurrentUserAsync()
    {
        var userId = GetCurrentUserId();
        var user = await _userManager.FindByIdAsync(userId.ToString());
        return user;
    }

    public async Task<IEnumerable<User>> GetUsersByEmailsAsync(IEnumerable<string> emails)
    {
        var userManager = _userManager;
        var result = new List<User>();
        foreach (var email in emails)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null && !result.Contains(user))
            {
                result.Add(user);
            }
        }
        return await Task.FromResult(result);
    }

    public async Task<IEnumerable<User>> GetUsersInRoleAsync(string roleName)
    {
        var userManager = _userManager;
        return await userManager.GetUsersInRoleAsync(roleName);
    }
    
    private async Task AddClaimsToUser(User user, string role)
    {
        await _userManager.AddClaimAsync(user, new Claim(OpenIddictConstants.Claims.Name, user.UserName));
        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.Firstname));
        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.Lastname));
        await _userManager.AddClaimAsync(user, new Claim(OpenIddictConstants.Claims.Email, user.Email));
        await _userManager.AddClaimAsync(user, new Claim(OpenIddictConstants.Claims.Role, role));
    }
}