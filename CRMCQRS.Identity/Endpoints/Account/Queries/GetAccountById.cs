using CRMCQRS.Domain;
using CRMCQRS.Identity.Application.Services;
using MediatR;

namespace CRMCQRS.Identity.Endpoints.Account.Queries;

public record class GetAccountByIdRequest : IRequest<User>
{ 
    public string Model { get; }

    public GetAccountByIdRequest(string model) => Model = model;
}

public class GetAccountByIdRequestHandler : IRequestHandler<GetAccountByIdRequest, User>
{
    private readonly IAccountService _accountService;

    public GetAccountByIdRequestHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task<User> Handle(GetAccountByIdRequest request, CancellationToken cancellationToken)
        => await _accountService.GetByIdAsync(Guid.Parse(request.Model));
}