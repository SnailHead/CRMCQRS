using System.Linq.Expressions;
using CRMCQRS.Application.Common.Filtration;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using MediatR;

namespace CRMCQRS.Application.Users.Queries.GetPageUser;

public class GetPageUserQuery : IRequest<IPagedList<UserViewModel>>, IFilterModel<GetPageUserQuery, User>
{
    public string Query { get; set; }
    public int Page { get; set; }

    public Expression<Func<User, bool>> GetExpression(GetPageUserQuery filterModel)
    {
        Expression<Func<User, bool>> globalExpression = c => c != null;
        if (!string.IsNullOrEmpty(filterModel.Query))
        {
            Expression<Func<User, bool>> exp = c =>
                c.Firstname.Contains(filterModel.Query.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                c.Lastname.Contains(filterModel.Query.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                (c.Middlename != null &&
                 c.Middlename.Contains(filterModel.Query.ToLower(), StringComparison.OrdinalIgnoreCase)
                );
            globalExpression = ExpressionBuilder.CompareExpression(globalExpression, exp);
        }

        return globalExpression;
    }
}