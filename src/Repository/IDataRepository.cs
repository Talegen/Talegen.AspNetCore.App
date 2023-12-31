/*
 * Talegen ASP.net Core App Library
 * (c) Copyright Talegen, LLC.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * http://www.apache.org/licenses/LICENSE-2.0
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
*/
namespace Talegen.AspNetCore.App.Repository
{
    /// <summary>
    /// This interface is used to handle all data related interactions.
    /// </summary>
    /// <typeparam name="T">Contains the type of the object to interact with in the repository.</typeparam>
    public interface IDataRepository<T> where T : class
    {
        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">Contains the entry to add.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Contains the result of the repository action.</returns>
        Task<RepositoryResult<T>> AddAsync(T entity, CancellationToken cancellationToken = default);


        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="id">Contains the entity identity to delete.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Contains the result of the repository action.</returns>
        Task<RepositoryResult<T>> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">Contains the entity to delete.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Contains the result of the repository action.</returns>
        Task<RepositoryResult<T>> DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">Contains the entity to update.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Contains the result of the repository action.</returns>
        Task<RepositoryResult<T>> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets an entity from the repository.
        /// </summary>
        /// <param name="id">Contains the identity of the entity to return.</param>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Contains the result of the repository action.</returns>
        Task<RepositoryResult<T>> GetAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Contains a queryable result of the repository action.</returns>
        Task<RepositoryResult<IQueryable<T>>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves all changes to the repository.
        /// </summary>
        /// <param name="cancellationToken">Contains an optional cancellation token.</param>
        /// <returns>Returns a value indicating whether the save changes was successful.</returns>
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
