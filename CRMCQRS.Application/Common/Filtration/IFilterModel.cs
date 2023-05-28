using System.Linq.Expressions;

namespace CRMCQRS.Application.Common.Filtration;

public interface IFilterModel<in T, T1>
{
    public  Expression<Func<T1, bool>> GetExpression(T filterModel);
}