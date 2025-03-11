using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;

namespace JAwelsAndDiamonds.Repositories
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">ID of the entity to retrieve</param>
        /// <returns>The entity if found, otherwise null</returns>
        T GetById(object id);

        /// <summary>
        /// Gets all entities of type T
        /// </summary>
        /// <returns>IEnumerable of all entities</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Finds entities based on a predicate
        /// </summary>
        /// <param name="predicate">The condition to filter entities</param>
        /// <returns>IEnumerable of filtered entities</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Adds a new entity to the repository
        /// </summary>
        /// <param name="entity">The entity to add</param>
        void Add(T entity);

        /// <summary>
        /// Updates an existing entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);

        /// <summary>
        /// Deletes an entity by its ID
        /// </summary>
        /// <param name="id">ID of the entity to delete</param>
        void Delete(object id);

        /// <summary>
        /// Deletes an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete(T entity);

        /// <summary>
        /// Saves changes to the database
        /// </summary>
        void SaveChanges();
    }
}