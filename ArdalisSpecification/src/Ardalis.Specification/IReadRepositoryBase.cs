using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ardalis.Specification
{
    /// <summary>
    /// <para>
    /// A <see cref="IRepositoryBase{T}" /> can be used to query instances of <typeparamref name="T" />.
    /// An <see cref="ISpecification{T}"/> (or derived) is used to encapsulate the LINQ queries against the database.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of entity being operated on by this repository.</typeparam>
    public interface IReadRepositoryBase<T> where T : class
    {
        /// <summary>
        /// Finds an entity with the given primary key value.
        /// </summary>
        /// <typeparam name="TId">The type of primary key.</typeparam>
        /// <param name="id">The value of the primary key for the entity to be found.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
        /// </returns>
        Task<T?> GetByIdAsync<TId>(TId id);

        /// <summary>
        /// Finds an entity that matches the encapsulated query logic of the <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="T" />, or <see langword="null"/>.
        /// </returns>
        Task<T?> GetBySpecAsync<Spec>(Spec specification) where Spec : ISingleResultSpecification, ISpecification<T>;

        /// <summary>
        /// Finds an entity that matches the encapsulated query logic of the <paramref name="specification"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the <typeparamref name="TResult" />.
        /// </returns>
        Task<TResult> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" /> from the database.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<T>> ListAsync();

        /// <summary>
        /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<T>> ListAsync(ISpecification<T> specification);

        /// <summary>
        /// Finds all entities of <typeparamref name="T" />, that matches the encapsulated query logic of the
        /// <paramref name="specification"/>, from the database.
        /// <para>
        /// Projects each entity into a new form, being <typeparamref name="TResult" />.
        /// </para>
        /// </summary>
        /// <typeparam name="TResult">The type of the value returned by the projection.</typeparam>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a <see cref="List{TResult}" /> that contains elements from the input sequence.
        /// </returns>
        Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification);

        /// <summary>
        /// Returns a number that represents how many entities satisfy the encapsulated query logic
        /// of the <paramref name="specification"/>.
        /// </summary>
        /// <param name="specification">The encapsulated query logic.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// number of elements in the input sequence.
        /// </returns>
        Task<int> CountAsync(ISpecification<T> specification);
    }
}
