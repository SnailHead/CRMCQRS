using System.Linq.Expressions;
using CRMCQRS.Application.Common.Filtration;
using CRMCQRS.Application.Missions.Queries;
using CRMCQRS.Domain;
using CRMCQRS.Infrastructure.Pages;
using MediatR;

namespace CRMCQRS.Application.Missions.Queries.GetPageMission;

public class GetPageMissionQuery :  IRequest<IPagedList<MissionViewModel>>, IFilterModel<GetPageMissionQuery, Mission>
{
    public string Title { get; set; }
    public int Page { get; set; }
    public Expression<Func<Mission, bool>> GetExpression(GetPageMissionQuery filterModel)
    {
        Expression<Func<Mission, bool>> globalExpression = c => c != null;
        if (!string.IsNullOrEmpty(filterModel.Title))
        {
            Expression<Func<Mission, bool>> exp = c => c.Title.ToLower().Contains(filterModel.Title.ToLower());
            globalExpression = ExpressionBuilder.CompareExpression(globalExpression, exp);
        }

        return globalExpression;
    }
}