using MediatR;

namespace CRMCQRS.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<UserViewModel>
{
    public Guid Id { get; set; }
}