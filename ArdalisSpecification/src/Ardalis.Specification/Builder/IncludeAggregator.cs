using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification
{
    /// <inheritdoc/>
    public class IncludeAggregator : IIncludeAggregator
    {
        private readonly List<string> navigationPropertyNames = new List<string>();

        public IncludeAggregator(string? navigationPropertyName)
        {
            AddNavigationPropertyName(navigationPropertyName);
        }

        /// <inheritdoc/>
        public void AddNavigationPropertyName(string? navigationPropertyName)
        {
            if (!string.IsNullOrEmpty(navigationPropertyName))
            {
                navigationPropertyNames.Add(navigationPropertyName!);
            }
        }

        /// <inheritdoc/>
        public string IncludeString
        {
            get
            {
                string output = string.Empty;

                for (int i = 0; i < navigationPropertyNames.Count; i++)
                {
                    output = i == 0 ? navigationPropertyNames[i] : $"{output}.{navigationPropertyNames[i]}";
                }

                return output;
            }
        }
    }
}
