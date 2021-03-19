using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace Ardalis.Specification.EntityFramework6
{
    public class DbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
		private readonly IEnumerator<T> _inner;

		public DbAsyncEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}

		public void Dispose()
		{
			_inner.Dispose();
		}

		public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(_inner.MoveNext());
		}

		public T Current
		{
			get { return _inner.Current; }
		}

		object IDbAsyncEnumerator.Current
		{
			get { return Current; }
		}
	}
}