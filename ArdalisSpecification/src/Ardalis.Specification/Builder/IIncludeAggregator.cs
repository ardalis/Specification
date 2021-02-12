using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    /// <summary>
    /// Aggregates navigation properties to include in the query.
    /// </summary>
    public interface IIncludeAggregator
    {
        /// <summary>
        /// Adds a navigation property to the <see cref="IncludeString"/> for the query.
        /// <para>
        /// Further navigation properties to be included can be appended,
        /// by calling this method multiple times.
        /// </para>
        /// </summary>
        /// <param name="navigationPropertyName">The name of the property to include as a navigation.</param>
        void AddNavigationPropertyName(string? navigationPropertyName);

        /// <summary>
        /// A string of '.' separated navigation property names to be included.
        /// </summary>
        string IncludeString { get; }
    }
}
