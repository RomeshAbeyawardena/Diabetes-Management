using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T>  BuildQuery<T, TRequest>(this T model, TRequest request)
        {
            var modelType = typeof(T);
            var requestType = typeof(TRequest);
            Expression? parentExpression = default;
            Expression expression = Expression.Parameter(modelType);
            foreach (var property in requestType.GetProperties())
            {
                var value = property.GetValue(request); 
                var propertyExpression = Expression.Property(expression, property.Name);
                var constantExpression = Expression.Constant(value);
                var equalExpression = Expression.Equal(propertyExpression, constantExpression);

                if (parentExpression == null)
                {
                    parentExpression = equalExpression;
                }
                else
                {
                    parentExpression = Expression.And(parentExpression, equalExpression);
                }
            }

            return default!;
        }
    }
}
