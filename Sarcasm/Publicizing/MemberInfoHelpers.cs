using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Sarcasm.Publicizing
{
    internal static class MemberInfoHelpers
    {
        public static Func<T, TResult> CreateGetFuncByExpression<T, TResult>(FieldInfo fieldInfo)
        {
            Expression body;
            ParameterExpression[] parameters;

            if (fieldInfo.IsStatic)
            {
                body = Expression.Field(null, fieldInfo);
                parameters = Array.Empty<ParameterExpression>();
            }
            else
            {
                var instance = Expression.Parameter(typeof(T), "instance");
                body = Expression.Field(Expression.Convert(instance, fieldInfo.DeclaringType), fieldInfo);
                parameters = new[] { instance };
            }

            return Expression.Lambda<Func<T, TResult>>(body, parameters).Compile();
        }
    }
}
