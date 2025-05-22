using System.Linq.Expressions;

namespace Strategyo.Extensions.Conversions;

public static class ExpressionExtensions
{
    public static Expression<Func<TTo, bool>> Adapt<TFrom, TTo>(
        this Expression<Func<TFrom, bool>> from)
        => ConvertImpl<Func<TFrom, bool>, Func<TTo, bool>>(from);

    private static Expression<TTo> ConvertImpl<TFrom, TTo>(Expression<TFrom> from)
    {
        // figure out which types are different in the function-signature
        var fromTypes = from.Type.GetGenericArguments();
        var toTypes = typeof(TTo).GetGenericArguments();

        if (fromTypes.Length != toTypes.Length)
        {
            throw new NotSupportedException("Incompatible lambda function-type signatures");
        }

        var typeMap = new Dictionary<Type, Type>();
        for (var i = 0; i < fromTypes.Length; i++)
        {
            if (fromTypes[i] != toTypes[i])
            {
                typeMap[fromTypes[i]] = toTypes[i];
            }
        }

        // re-map all parameters that involve different types
        var parameterMap = new Dictionary<Expression, Expression>();
        var newParams = GenerateParameterMap(from, typeMap, parameterMap);

        // rebuild the lambda
        var body = new TypeConversionVisitor<TTo>(parameterMap).Visit(from.Body);

        if (body == null)
        {
            throw new NotSupportedException("Conversion returned null");
        }
        
        return Expression.Lambda<TTo>(body, newParams);
    }

    private static ParameterExpression[] GenerateParameterMap<TFrom>(
        Expression<TFrom> from,
        Dictionary<Type, Type> typeMap,
        Dictionary<Expression, Expression> parameterMap
    )
    {
        var newParams = new ParameterExpression[from.Parameters.Count];

        for (var i = 0; i < newParams.Length; i++)
        {
            if (typeMap.TryGetValue(from.Parameters[i].Type, out var newType))
            {
                parameterMap[from.Parameters[i]] = newParams[i] = Expression.Parameter(newType, from.Parameters[i].Name);
            }
        }

        return newParams;
    }


    private class TypeConversionVisitor<T>(Dictionary<Expression, Expression> parameterMap) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            // re-map the parameter
            if (!parameterMap.TryGetValue(node, out var found))
            {
                found = base.VisitParameter(node);
            }

            return found;
        }

        public override Expression? Visit(Expression? node)
        {
            if (node is LambdaExpression lambda &&
                !parameterMap.ContainsKey(lambda.Parameters.First()))
            {
                return new TypeConversionVisitor<T>(parameterMap).Visit(lambda.Body);
            }

            return base.Visit(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            // re-perform any member-binding
            var expr = Visit(node.Expression);

            if (expr == null || expr.Type == node.Type)
            {
                return base.VisitMember(node);
            }

            if (expr.Type.GetMember(node.Member.Name).Length == 0)
            {
                return base.VisitMember(node);
            }

            var newMember = expr.Type.GetMember(node.Member.Name).Single();
            return Expression.MakeMemberAccess(expr, newMember);
        }
    }
}