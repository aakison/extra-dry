#nullable enable

using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazor.ExtraDry.Internal {

    public static class MethodInfoHelper {
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return expression.Body is MethodCallExpression member ? 
                member.Method : 
                throw new ArgumentException("Expression is not a method", nameof(expression));
        }
    }
}
