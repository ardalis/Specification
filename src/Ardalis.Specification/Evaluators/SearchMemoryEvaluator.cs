using System.Buffers;
using System.Diagnostics;

namespace Ardalis.Specification;

public class SearchMemoryEvaluator : IInMemoryEvaluator
{
    private SearchMemoryEvaluator() { }
    public static SearchMemoryEvaluator Instance { get; } = new SearchMemoryEvaluator();

    public IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification)
    {
        if (specification.SearchCriterias is List<SearchExpressionInfo<T>> list && list.Count > 0)
        {
            return new SpecLikeIterator<T>(query, list);
        }

        return query;
    }

    private sealed class SpecLikeIterator<TSource> : Iterator<TSource>
    {
        private readonly IEnumerable<TSource> _source;
        private readonly List<SearchExpressionInfo<TSource>> _list;
        private SearchExpressionInfo<TSource>[]? _states;

        private IEnumerator<TSource>? _enumerator;

        public SpecLikeIterator(IEnumerable<TSource> source, List<SearchExpressionInfo<TSource>> list)
        {
            _source = source;
            _list = list;

            _states = ArrayPool<SearchExpressionInfo<TSource>>.Shared.Rent(list.Count);
            FillSorted(list, _states.AsSpan().Slice(0, list.Count));
        }

        public override Iterator<TSource> Clone()
            => new SpecLikeIterator<TSource>(_source, _list);

        public override void Dispose()
        {
            if (_states is not null)
            {
                ArrayPool<SearchExpressionInfo<TSource>>.Shared.Return(_states);
                _states = null;
            }
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
                    Debug.Assert(_states is not null);
                    var states = _states.AsSpan().Slice(0, _list.Count);
                    while (_enumerator!.MoveNext())
                    {
                        TSource item = _enumerator.Current;
                        if (IsValid(item, states))
                        {
                            _current = item;
                            return true;
                        }
                    }

                    Dispose();
                    break;
            }

            return false;
        }

        private static void FillSorted<T>(List<SearchExpressionInfo<T>> list, Span<SearchExpressionInfo<T>> span)
        {
            var i = 0;
            foreach (var state in list)
            {
                // Find the correct insertion point
                var j = i;
                while (j > 0 && span[j - 1].SearchGroup > state.SearchGroup)
                {
                    span[j] = span[j - 1];
                    j--;
                }

                // Insert the current state in the sorted position
                span[j] = state;
                i++;
            }
        }

        private static bool IsValid<T>(T item, Span<SearchExpressionInfo<T>> span)
        {
            var valid = true;
            int start = 0;

            for (int i = 1; i <= span.Length; i++)
            {
                if (i == span.Length || span[i].SearchGroup != span[start].SearchGroup)
                {
                    var validOrGroup = IsValidInOrGroup(item, span.Slice(start, i - start));
                    if ((valid = valid && validOrGroup) is false)
                    {
                        break;
                    }
                    start = i;
                }
            }

            return valid;

            static bool IsValidInOrGroup(T item, Span<SearchExpressionInfo<T>> span)
            {
                var validOrGroup = false;
                foreach (var state in span)
                {
                    if (state.SelectorFunc(item)?.Like(state.SearchTerm) ?? false)
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
