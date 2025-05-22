using System.Linq.Expressions;
using System.Reflection;

namespace Strategyo.Extensions;

public static class PropertiesExtensions
{
    public static string GetPropertyName<TProperty>(this Expression<Func<TProperty>> propertyExpression)
        => propertyExpression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression { Operand: MemberExpression operand } => operand.Member
                                                                            .Name,
            _ => throw new ArgumentException("Invalid expression")
        };
    
    public static PropertyInfo GetPropertyInfo<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            return (PropertyInfo)memberExpression.Member;
        }
        throw new ArgumentException("Invalid expression");
    }
}