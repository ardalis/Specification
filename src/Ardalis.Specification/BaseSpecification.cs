using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    public class BaseSpecification<T>
    {
        protected Expression<Func<T, bool>> _criteria;
        public Expression<Func<T, bool>> Criteria => _criteria;

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();
        public void AddInclude(Expression<Func<T, object>> includeExpression) => Includes.Add(includeExpression);

        public string CacheKey { get; protected set; }
        public bool CacheEnabled { get; private set; }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            _criteria = criteria;
        }

        /// <summary>
        /// Must be called after specifying criteria
        /// </summary>
        /// <param name="specificationName"></param>
        /// <param name="args">Any arguments used in defining the specification</param>
        protected void EnableCache(string specificationName, params object[] args)
        {
            Guard.Against.NullOrEmpty(specificationName, nameof(specificationName));
            Guard.Against.Null(Criteria, nameof(Criteria));

            CacheKey = $"{specificationName}-{string.Join("-", args)}";

            CacheEnabled = true;
        }
    }
}
