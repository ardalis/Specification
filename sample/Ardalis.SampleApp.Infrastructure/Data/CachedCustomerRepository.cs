using Ardalis.SampleApp.Core.Entitites.CustomerAggregate;
using Ardalis.SampleApp.Core.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ardalis.SampleApp.Infrastructure.Data
{
    public class CachedRepository<T> : IRepository<T> where T : class, IAggregateRoot
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<CachedRepository<T>> _logger;
        private readonly MyRepository<T> _sourceRepository;
        private MemoryCacheEntryOptions _cacheOptions;

        public CachedRepository(IMemoryCache cache,
            ILogger<CachedRepository<T>> logger,
            MyRepository<T> sourceRepository)
        {
            _cache = cache;
            _logger = logger;
            _sourceRepository = sourceRepository;

            _cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(relative: TimeSpan.FromSeconds(10));
        }

        public Task<T> AddAsync(T entity)
        {
            return _sourceRepository.AddAsync(entity);
        }

        public Task<int> CountAsync(Specification.ISpecification<T> specification)
        {
            // TODO: Add Caching
            return _sourceRepository.CountAsync(specification);
        }

        public Task DeleteAsync(T entity)
        {
            return _sourceRepository.DeleteAsync(entity);
        }

        public Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            return _sourceRepository.DeleteRangeAsync(entities);
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _sourceRepository.GetByIdAsync(id);
        }

        public Task<T> GetByIdAsync<TId>(TId id)
        {
            return _sourceRepository.GetByIdAsync(id);
        }

        public Task<T> GetBySpecAsync(Specification.ISpecification<T> specification)
        {
            if(specification.CacheEnabled)
            {
                string key = $"{specification.CacheKey}-GetBySpecAsync";
                _logger.LogInformation("Checking cache for " + key);
                return _cache.GetOrCreate(key, entry =>
                {
                    entry.SetOptions(_cacheOptions);
                    _logger.LogWarning("Fetching source data for " + key);
                    return _sourceRepository.GetBySpecAsync(specification);
                });
            }
            return _sourceRepository.GetBySpecAsync(specification);
        }

        public Task<TResult> GetBySpecAsync<TResult>(Specification.ISpecification<T, TResult> specification)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ListAsync()
        {
            string key = $"{nameof(T)}-ListAsync";
            return _cache.GetOrCreate(key, entry =>
            {
                entry.SetOptions(_cacheOptions);
                return _sourceRepository.ListAsync();
            });
        }

        public Task<List<T>> ListAsync(Specification.ISpecification<T> specification)
        {
            if (specification.CacheEnabled)
            {
                string key = $"{specification.CacheKey}-ListAsync";
                _logger.LogInformation("Checking cache for " + key);
                return _cache.GetOrCreate(key, entry =>
                {
                    entry.SetOptions(_cacheOptions);
                    _logger.LogWarning("Fetching source data for " + key);
                    return _sourceRepository.ListAsync(specification);
                });
            }
            return _sourceRepository.ListAsync(specification);
        }

        public Task<List<TResult>> ListAsync<TResult>(Specification.ISpecification<T, TResult> specification)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            return _sourceRepository.SaveChangesAsync();
        }

        public Task UpdateAsync(T entity)
        {
            return _sourceRepository.UpdateAsync(entity);
        }
    }
}
