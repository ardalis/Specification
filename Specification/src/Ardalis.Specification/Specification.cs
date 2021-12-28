using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ardalis.Specification
{
    /// <inheritdoc cref="ISpecification{T, TResult}"/>
    public abstract class Specification<T, TResult> : Specification<T>, ISpecification<T, TResult>
    {
        protected new virtual ISpecificationBuilder<T, TResult> Query { get; }

        protected Specification()
            : this(InMemorySpecificationEvaluator.Default)
        {
        }

        protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator)
            : base(inMemorySpecificationEvaluator)
        {
            this.Query = new SpecificationBuilder<T, TResult>(this);
        }

        public new virtual IEnumerable<TResult> Evaluate(IEnumerable<T> entities)
        {
            return Evaluator.Evaluate(entities, this);
        }

        /// <inheritdoc/>
        public Expression<Func<T, TResult>>? Selector { get; internal set; }

        /// <inheritdoc/>
        public new Func<IEnumerable<TResult>, IEnumerable<TResult>>? PostProcessingAction { get; internal set; } = null;
    }

    /// <inheritdoc cref="ISpecification{T}"/>
    public abstract class Specification<T> : ISpecification<T>
    {
        protected IInMemorySpecificationEvaluator Evaluator { get; }
        protected virtual ISpecificationBuilder<T> Query { get; }

        protected Specification()
            : this(InMemorySpecificationEvaluator.Default)
        {
        }

        protected Specification(IInMemorySpecificationEvaluator inMemorySpecificationEvaluator)
        {
            this.Evaluator = inMemorySpecificationEvaluator;
            this.Query = new SpecificationBuilder<T>(this);
        }

        /// <inheritdoc/>
        public virtual IEnumerable<T> Evaluate(IEnumerable<T> entities)
        {
            return Evaluator.Evaluate(entities, this);
        }

        /// <inheritdoc/>
        public IEnumerable<WhereExpressionInfo<T>> WhereExpressions { get; } = new List<WhereExpressionInfo<T>>();

        public IEnumerable<OrderExpressionInfo<T>> OrderExpressions { get; } = new List<OrderExpressionInfo<T>>();

        /// <inheritdoc/>
        public IEnumerable<IncludeExpressionInfo> IncludeExpressions { get; } = new List<IncludeExpressionInfo>();

        /// <inheritdoc/>
        public IEnumerable<string> IncludeStrings { get; } = new List<string>();

        /// <inheritdoc/>
        public IEnumerable<SearchExpressionInfo<T>> SearchCriterias { get; } = new List<SearchExpressionInfo<T>>();

        /// <inheritdoc/>
        public int? Take { get; internal set; } = null;

        /// <inheritdoc/>
        public int? Skip { get; internal set; } = null;

        /// <inheritdoc/>
        public bool IsPagingEnabled { get; internal set; } = false;

        /// <inheritdoc/>
        public Func<IEnumerable<T>, IEnumerable<T>>? PostProcessingAction { get; internal set; } = null;

        /// <inheritdoc/>
        public string? CacheKey { get; internal set; }

        /// <inheritdoc/>
        public bool CacheEnabled { get; internal set; }

        /// <inheritdoc/>
        public bool AsNoTracking { get; internal set; } = false;

        /// <inheritdoc/>
        public bool AsSplitQuery { get; internal set; } = false;

        /// <inheritdoc/>
        public bool AsNoTrackingWithIdentityResolution { get; internal set; } = false;
    }
}