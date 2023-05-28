using System.Linq.Expressions;
using CRMCQRS.Application.Common.Filtration;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using MediatR;

namespace CRMCQRS.Application.Projects.Queries.GetPageProject;

public class GetPageProjectQuery :  IRequest<IPagedList<ProjectViewModel>>, IFilterModel<GetPageProjectQuery, Project>
{
    public string Title { get; set; }
    public int Page { get; set; }
    public Expression<Func<Project, bool>> GetExpression(GetPageProjectQuery filterModel)
    {
        Expression<Func<Project, bool>> globalExpression = c => c != null;
        if (!string.IsNullOrEmpty(filterModel.Title))
        {
            Expression<Func<Project, bool>> exp = c => c.Title.ToLower().Contains(filterModel.Title.ToLower());
            globalExpression = ExpressionBuilder.CompareExpression(globalExpression, exp);
        }

        return globalExpression;
    }
}