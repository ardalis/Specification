﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ardalis.Specification
{
  public class InMemorySpecificationEvaluator : IInMemorySpecificationEvaluator
  {
    // Will use singleton for default configuration. Yet, it can be instantiated if necessary, with default or provided evaluators.
    public static InMemorySpecificationEvaluator Default { get; } = new InMemorySpecificationEvaluator();

    private readonly List<IInMemoryEvaluator> evaluators = new List<IInMemoryEvaluator>();

    public InMemorySpecificationEvaluator()
    {
      this.evaluators.AddRange(new IInMemoryEvaluator[]
      {
                WhereEvaluator.Instance,
                SearchEvaluator.Instance,
                OrderEvaluator.Instance,
                PaginationEvaluator.Instance
      });
    }
    public InMemorySpecificationEvaluator(IEnumerable<IInMemoryEvaluator> evaluators)
    {
      this.evaluators.AddRange(evaluators);
    }

    public virtual IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification)
    {
      if (specification.Selector is null && specification.SelectorMany is null) throw new SelectorNotFoundException();
      if (specification.Selector != null && specification.SelectorMany != null) throw new ConcurrentSelectorsException();

      var baseQuery = Evaluate(source, (ISpecification<T>)specification);

      var resultQuery = specification.Selector != null
        ? baseQuery.Select(specification.Selector.Compile())
        : baseQuery.SelectMany(specification.SelectorMany!.Compile());

      return specification.PostProcessingAction == null
          ? resultQuery
          : specification.PostProcessingAction(resultQuery);
    }

    public virtual IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification)
    {
      foreach (var evaluator in evaluators)
      {
        source = evaluator.Evaluate(source, specification);
      }

      return specification.PostProcessingAction == null
          ? source
          : specification.PostProcessingAction(source);
    }
  }
}
