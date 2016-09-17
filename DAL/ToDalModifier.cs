using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL
{
    public static class Modifier
    {
        /// <summary>
        /// Class that modifying experession with type <typeparam name="TSource"></typeparam> 
        /// in <typeparam name="TTarget"></typeparam>
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        public class ParameterTypeVisitor<TSource, TTarget> : ExpressionVisitor
        {
            private ReadOnlyCollection<ParameterExpression> parameters;

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return parameters?.FirstOrDefault(p => p.Name == node.Name) ??
                    (node.Type == typeof(TSource) ? Expression.Parameter(typeof(TTarget), node.Name) : node);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                parameters = VisitAndConvert(node.Parameters, "VisitLambda");
                return Expression.Lambda(Visit(node.Body), parameters);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Member.DeclaringType == typeof(TSource))
                {
                    return Expression.Property(Visit(node.Expression), node.Member.Name);
                }
                return base.VisitMember(node);
            }
        }

        /// <summary>
        /// Method that modifying experession with type <typeparam name="TSource"></typeparam> 
        /// in <typeparam name="TTarget"></typeparam>
        /// </summary>
        /// <typeparam name="TSource">Source type.</typeparam>
        /// <typeparam name="TTarget">Target type.</typeparam>
        public static Func<TTarget, bool> Convert<TSource, TTarget>(Expression<Func<TSource, bool>> expression)
        {
            var visitor = new ParameterTypeVisitor<TSource, TTarget>();
            var newExpression = (Expression<Func<TTarget, bool>>)visitor.Visit(expression);
            return newExpression.Compile();
        }
    }
}
