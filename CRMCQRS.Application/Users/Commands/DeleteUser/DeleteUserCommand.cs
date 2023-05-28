using MediatR;

namespace CRMCQRS.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand :  IRequest<bool>
{
    public Guid Id { get; set; }
    
}