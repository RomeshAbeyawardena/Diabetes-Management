using System.Linq.Expressions;

namespace DiabetesManagement.Extensions.Extensions;

public static class ExpressionExtensions
{
	public static Expression<Func<T, bool>> BuildQuery<T, TRequest>(TRequest request)
	{
		var modelType = typeof(T);
		var requestType = typeof(TRequest);
		Expression? parentExpression = default;
		var expression = Expression.Parameter(modelType, modelType.Name);
		foreach (var property in requestType.GetProperties())
		{
			var value = property.GetValue(request);

			if (value == null)
			{
				continue;
			}

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

		return Expression.Lambda<Func<T, bool>>(parentExpression!, true, expression!);
	}
}
