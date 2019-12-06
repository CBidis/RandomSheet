using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Timesheets.Domain.Models;

namespace Timesheets.Persistence.Extensions
{
    /// <summary>
    /// Common Repository Extensions for Pagination, etc
    /// </summary>
	public static class RepositoryExtensions
    {
        /// <summary>
        /// Pagination Extension Method
        /// </summary>
        /// <typeparam name="TModel">TModel type derived from IEntity</typeparam>
        /// <typeparam name="TKey">Tkey type of Primary key column</typeparam>
        /// <param name="query">IQuerable query to execute</param>
        /// <param name="orderColumn">ordering column</param>
        /// <param name="skipNumber">number of rows to skip</param>
        /// <param name="takeNumber">number of rows to take</param>
        /// <returns>IQuerable object</returns>
        public static IQueryable<TModel> Paging<TModel, TKey>(this IQueryable<TModel> query, string orderColumn = "Id", int skipNumber = 0, int takeNumber = 0) 
                    where TModel : class, IEntity<TKey> where TKey : struct
            => takeNumber > 0 ? query.OrderByCustom<TModel, TKey>(orderColumn).Skip(skipNumber).Take(takeNumber) : query.OrderByCustom<TModel, TKey>(orderColumn);

        /// <summary>
        /// Order Extension Method
        /// </summary>
        /// <typeparam name="TModel">TModel type derived from IEntity</typeparam>
        /// <param name="source">IQuerable source to execute the ordering</param>
        /// <param name="orderByProperty">property to apply dynamic order</param>
        /// <returns>IQuerable Ordered query to execute</returns>
        public static IQueryable<TModel> OrderByCustom<TModel, TKey>(this IQueryable<TModel> source, string orderByProperty) 
                    where TModel : class, IEntity<TKey> where TKey : struct
        {
            var command = orderByProperty.StartsWith("-") ? "OrderByDescending" : "OrderBy";
            orderByProperty = orderByProperty.Replace("-", string.Empty);

            Type type = typeof(TModel);
            PropertyInfo property = type.GetProperty(orderByProperty);
            ParameterExpression parameter = Expression.Parameter(type, "p");

            if (orderByProperty.Contains('.'))
            {
                ParameterExpression param = Expression.Parameter(typeof(TModel), "p");
                var parts = orderByProperty.Split('.');

                Expression parent = parts.Aggregate<string, Expression>(param, Expression.Property);

                LambdaExpression lamdaConversion = Expression.Lambda(parent, param);

                MethodCallExpression resultExpressionParts = Expression.Call(typeof(Queryable), command, new Type[] { type, typeof(string) },
                              source.Expression, Expression.Quote(lamdaConversion));

                return source.Provider.CreateQuery<TModel>(resultExpressionParts);
            }

            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                                          source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TModel>(resultExpression);
        }
    }
}
