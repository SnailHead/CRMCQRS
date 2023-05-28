using System.Linq.Expressions;

namespace CRMCQRS.Application.Common.Filtration;

public static class ExpressionBuilder
{
    public static Expression<Func<T, bool>> CompareExpression<T>(Expression<Func<T, bool>> firstExp, Expression<Func<T, bool>> secondExp) where T : class
    {
        var visitor = new ParameterUpdateVisitor(firstExp.Parameters.First(), secondExp.Parameters.First());
        // replace the parameter in the expression just created
        firstExp = visitor.Visit(firstExp) as Expression<Func<T, bool>>;

        // now you can and together the two expressions
        var binExp = Expression.And(secondExp.Body, firstExp.Body);
        return Expression.Lambda<Func<T, bool>>(binExp, firstExp.Parameters);
    }
}
class ParameterUpdateVisitor : ExpressionVisitor
{
    private ParameterExpression _oldParameter;
    private ParameterExpression _newParameter;

    public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (object.ReferenceEquals(node, _oldParameter))
            return _newParameter;

        return base.VisitParameter(node);
    }
}