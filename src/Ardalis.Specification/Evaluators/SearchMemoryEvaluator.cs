using System.Diagnostics;

namespace Ardalis.Specification;

public class SearchMemoryEvaluator : IInMemoryEvaluator
{
    private SearchMemoryEvaluator() { }
    public static SearchMemoryEvaluator Instance { get; } = new SearchMemoryEvaluator();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        if (specification is Specification<T> spec)
        {
            if (spec.OneOrManySearchExpressions.IsEmpty) return query;

            if (spec.OneOrManySearchExpressions.SingleOrDefault is { } searchExpression)
            {
                return new SpecSingleLikeIterator<T>(query, searchExpression);
            }

            // The search expressions are already sorted by SearchGroup.
            return new SpecLikeIterator<T>(query, spec.OneOrManySearchExpressions.List);
        }

        return query;
    }

    private sealed class SpecSingleLikeIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly SearchExpressionInfo<TSource> _searchExpression;

        private IEnumerator<TSource>? _enumerator;

        public SpecSingleLikeIterator(IEnumerable<TSource> source, SearchExpressionInfo<TSource> searchExpression)
        {
            _source = source;
            _searchExpression = searchExpression;
        }

        public override Iterator<TSource> Clone()
            => new SpecSingleLikeIterator<TSource>(_source, _searchExpression);

        public override void Dispose()
        {
            if (_enumerator is not null)
            {
                _enumerator.Dispose();
                _enumerator = null;
            }
            base.Dispose();
        }

        public override bool MoveNext()
        {
            switch (_state)
            {
                case 1:
                    _enumerator = _source.GetEnumerator();
                    _state = 2;
                    goto case 2;
                case 2:
                    Debug.Assert(_enumerator is not null);
                    var searchExpression = _searchExpression;
                    while (_enumerator!.MoveNext())
                    {
                        TSource sourceItem = _enumerator.Current;
                        if (searchExpression.SelectorFunc(sourceItem)?.Like(searchExpression.SearchTerm) ?? false)
                        {
                            _current = sourceItem;
                            return true;
                        }
                    }

                    Dispose();
                    break;
            }

            return false;
        }
    }

    private sealed class SpecLikeIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly List<SearchExpressionInfo<TSource>> _searchExpressions;

        private IEnumerator<TSource>? _enumerator;

        public SpecLikeIterator(IEnumerable<TSource> source, List<SearchExpressionInfo<TSource>> searchExpressions)
        {
            _source = source;
            _searchExpressions = searchExpressions;
        }

        public override Iterator<TSource> Clone()
            => new SpecLikeIterator<TSource>(_source, _searchExpressions);

        public override void Dispose()
        {
            if (_enumerator is not null)
            {
                _enumerator.Dispose();
                _enumerator = null;
            }
            base.Dispose();
        }

        public override bool MoveNext()
        {
            switch (_state)
            {
                case 1:
                    _enumerator = _source.GetEnumerator();
                    _state = 2;
                    goto case 2;
                case 2:
                    Debug.Assert(_enumerator is not null);
                    var searchExpressions = _searchExpressions;
                    while (_enumerator!.MoveNext())
                    {
                        TSource sourceItem = _enumerator.Current;
                        if (IsValid(sourceItem, searchExpressions))
                        {
                            _current = sourceItem;
                            return true;
                        }
                    }

                    Dispose();
                    break;
            }

            return false;
        }

        // This would be simpler using Span<SearchExpressionInfo<TSource>>
        // but CollectionsMarshal.AsSpan is not available in .NET Standard 2.0
        private static bool IsValid<T>(T sourceItem, List<SearchExpressionInfo<T>> list)
        {
            var groupStart = 0;
            for (var i = 1; i <= list.Count; i++)
            {
                // If we reached the end of the list or the group has changed, we slice and process the group.
                if (i == list.Count || list[i].SearchGroup != list[groupStart].SearchGroup)
                {
                    if (IsValidInOrGroup(sourceItem, list, groupStart, i) is false)
                    {
                        return false;
                    }
                    groupStart = i;
                }
            }
            return true;

            static bool IsValidInOrGroup(T sourceItem, List<SearchExpressionInfo<T>> list, int from, int to)
            {
                var validOrGroup = false;
                for (int i = from; i < to; i++)
                {
                    if (list[i].SelectorFunc(sourceItem)?.Like(list[i].SearchTerm) ?? false)
                    {
                        validOrGroup = true;
                        break;
                    }
                }
                return validOrGroup;
            }
        }
    }
}
