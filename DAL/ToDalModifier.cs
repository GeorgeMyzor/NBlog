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
                parameters = VisitAndConvert<ParameterExpression>(node.Parameters, "VisitLambda");
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

        public static Func<TTarget, bool> Convert<TSource, TTarget>(Expression<Func<TSource, bool>> root)
        {
            var visitor = new ParameterTypeVisitor<TSource, TTarget>();
            var expression = (Expression<Func<TTarget, bool>>)visitor.Visit(root);
            return expression.Compile();
        }
    }
}
