﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
  public class IncludableSpecificationBuilder<T, TProperty> : IIncludableSpecificationBuilder<T, TProperty> where T : class
  {
    public Specification<T> Specification { get; }
    public bool IsChainDiscarded { get; set; }

    public IncludableSpecificationBuilder(Specification<T> specification)
        : this(specification, false)
    {
    }

    public IncludableSpecificationBuilder(Specification<T> specification, bool isChainDiscarded)
    {
      this.Specification = specification;
      this.IsChainDiscarded = isChainDiscarded;
    }
  }
}
