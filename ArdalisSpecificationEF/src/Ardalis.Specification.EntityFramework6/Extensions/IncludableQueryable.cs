using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Ardalis.Specification.EntityFramework6
{
    internal class IncludableQueryable<T> : IIncludableQueryable<T>, IOrderedQueryable<T>, IDbAsyncEnumerable<T>
    {
        private readonly IQueryable<T> _queryable;
        internal IQueryable<T> IQueryable => _queryable;
        public IncludableQueryable(IQueryable<T> queryable, string propertyName)
        {
            _queryable = queryable;
            PropertyName = propertyName;
        }
        
        public string PropertyName { get; set; }
        public Expression Expression => _queryable.Expression;
        public Type ElementType => _queryable.ElementType;
        public IQueryProvider Provider => _queryable.Provider;

        public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new DbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }
    }
}