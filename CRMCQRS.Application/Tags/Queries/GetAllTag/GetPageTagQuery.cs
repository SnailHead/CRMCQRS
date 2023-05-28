using System.Linq.Expressions;
using CRMCQRS.Application.Common;
using CRMCQRS.Application.Common.Filtration;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using MediatR;

namespace CRMCQRS.Application.Tags.Queries.GetAllTag;

public class GetPageTagQuery :  IRequest<IPagedList<TagViewModel>>, IFilterModel<GetPageTagQuery, Tag>
{
    public string Title { get; set; }
    public int Page { get; set; }
    public Expression<Func<Tag, bool>> GetExpression(GetPageTagQuery filterModel)
    {
        Expression<Func<Tag, bool>> globalExpression = c => c != null;
        if (!string.IsNullOrEmpty(filterModel.Title))
        {
            Expression<Func<Tag, bool>> exp = c => c.Title.ToLower().Contains(filterModel.Title.ToLower());
            globalExpression = ExpressionBuilder.CompareExpression(globalExpression, exp);
        }

        return globalExpression;
    }
}