using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HazGo.BuildingBlocks.Persistence.EF.SearchRepository
{
    public static class SearchOptionsExtensions
    {
        public static async Task<(IList<T> Values, int Total)> SearchAsync<T>(this SearchOptions<T> options, IQueryable<T> query)
        {
            query = query.ApplyFilter(options);
            int total = await EntityFrameworkQueryableExtensions.CountAsync(query);
            query = query.ApplyOrderBy(options);
            return (await query.Skip(options.Skip).Take(options.Top).ToListAsync(), total);
        }

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, SearchOptions<T> options)
        {
            if (!string.IsNullOrEmpty(options.Filter))
            {
                var filters = PrepareFilterString(options.Filter);

                List<Expression<Func<T, bool>>> filterExpressions = new List<Expression<Func<T, bool>>>();
                Expression<Func<T, bool>> previousExpression = null;
                foreach (var filter in filters)
                {
                    var filterExpresson = ToExpression<T>(filter.FilterOperator.ToUpper(), filter.Property, filter.Operation, filter.Value, previousExpression);
                    previousExpression = filterExpresson;
                    filterExpressions.Add(previousExpression);
                }

                query = query.Where(previousExpression);
            }

            return query;
        }

        public static IQueryable<T> ApplyOrderBy<T>(this IQueryable<T> query, SearchOptions<T> options)
        {
            if (options.OrderBy != null)
            {
                query = (options.OrderByDirection == OrderByDirection.Descending) ? query.OrderByDescending(PrepareOrderBy<T>(options.OrderBy)) : query.OrderBy(PrepareOrderBy<T>(options.OrderBy));
            }

            return query;
        }

        public static Expression<Func<T, bool>> ToExpression<T>(string andOrOperator, string propName, string opr, string value, Expression<Func<T, bool>> expr = null)
        {
            Expression<Func<T, bool>> func = null;
            try
            {
                ParameterExpression paramExpr = Expression.Parameter(typeof(T));
                var arrProp = propName.Split('.').ToList();
                Expression binExpr = null;
                string partName = string.Empty;
                arrProp.ForEach(x =>
                {
                    Expression tempExpr = null;
                    partName = string.IsNullOrEmpty(partName) ? x : partName + "." + x;
                    if (partName == propName)
                    {
                        var member = NestedExprProp(paramExpr, partName);
                        var type = member.Type.Name == "Nullable`1" ? Nullable.GetUnderlyingType(member.Type) : member.Type;
                        tempExpr = PrepareApplyFilter(opr, member, Expression.Convert(ToExprConstant(type, value), member.Type));
                    }
                    else
                        tempExpr = PrepareApplyFilter("!=", NestedExprProp(paramExpr, partName), Expression.Constant(null));
                    if (binExpr != null)
                        binExpr = Expression.AndAlso(binExpr, tempExpr);
                    else
                        binExpr = tempExpr;
                });
                Expression<Func<T, bool>> innerExpr = Expression.Lambda<Func<T, bool>>(binExpr, paramExpr);
                if (expr != null)
                    innerExpr = (string.IsNullOrEmpty(andOrOperator) || andOrOperator == "AND" || andOrOperator == "&&") ? innerExpr.And(expr) : innerExpr.Or(expr);
                func = innerExpr;
            }
            catch { }
            return func;
        }

        public static Expression<Func<T, TResult>> And<T, TResult>(this Expression<Func<T, TResult>> expr1, Expression<Func<T, TResult>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, TResult>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        private static Expression<Func<T, object>> PrepareOrderBy<T>(string propName)
        {
            ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            var tempExpr = NestedExprProp(paramExpr, propName);
            return Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Lambda(tempExpr, paramExpr).Body, typeof(object)), paramExpr);

        }

        private static MemberExpression NestedExprProp(Expression expr, string propName)
        {
            string[] arrProp = propName.Split('.');
            int arrPropCount = arrProp.Length;
            return (arrPropCount > 1) ? Expression.Property(NestedExprProp(expr, arrProp.Take(arrPropCount - 1).Aggregate((a, i) => a + "." + i)), arrProp[arrPropCount - 1]) : Expression.Property(expr, propName);
        }

        private static Expression PrepareApplyFilter(string opr, Expression left, Expression right)
        {
            Expression InnerLambda = null;
            switch (opr.ToUpper())
            {
                case "==":
                case "=":
                    InnerLambda = Expression.Equal(left, right);
                    break;
                case "<":
                    InnerLambda = Expression.LessThan(left, right);
                    break;
                case ">":
                    InnerLambda = Expression.GreaterThan(left, right);
                    break;
                case ">=":
                    InnerLambda = Expression.GreaterThanOrEqual(left, right);
                    break;
                case "<=":
                    InnerLambda = Expression.LessThanOrEqual(left, right);
                    break;
                case "!=":
                    InnerLambda = Expression.NotEqual(left, right);
                    break;
                case "&&":
                    InnerLambda = Expression.And(left, right);
                    break;
                case "||":
                    InnerLambda = Expression.Or(left, right);
                    break;
                case "LIKE":
                    InnerLambda = Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right);
                    break;
                case "NOTLIKE":
                    InnerLambda = Expression.Not(Expression.Call(left, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), right));
                    break;
            }
            return InnerLambda;
        }

        private static IEnumerable<RawFilterParts> PrepareFilterString(string filterString)
        {
            string filterOperator = string.Empty;

            if (filterString == null)
            {
                throw new ArgumentNullException(nameof(filterString));
            }

            foreach (string filterStringSegment in filterString.Split(' '))
            {
                var filterParts = filterStringSegment.Split('~');

                if (filterParts.Length % 3 != 0)
                {
                    if (filterParts.Length == 1)
                    {
                        filterOperator = filterParts[0];
                        continue;
                    }

                    throw new ArgumentException($"Invalid Filter String Segment: {filterStringSegment}");
                }

                yield return new RawFilterParts
                {
                    Property = filterParts[0],
                    Operation = filterParts[1],
                    Value = filterParts[2],
                    FilterOperator = filterOperator
                };
            }
        }

        private static Expression ToExprConstant(Type prop, string value)
        {
            if (string.IsNullOrEmpty(value))
                return Expression.Constant(value);
            object val = null;
            switch (prop.FullName)
            {
                case "System.Guid":
                    val = new Guid(value);
                    break;
                default:
                    val = Convert.ChangeType(value, Type.GetType(prop.FullName));
                    break;
            }
            return Expression.Constant(val);
        }
    }
}
