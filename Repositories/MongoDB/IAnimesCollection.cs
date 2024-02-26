using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDBAPI.Models;

namespace anime_streaming.Repositories
{
    /// <summary>
    /// Interface for accessing and managing animes in different collections in MongoDB.
    /// </summary>
    public interface IAnimeCollection
    {
        /// <summary>
        /// Gets all the names of the collections.
        /// </summary>
        /// <returns>A list of names of all collections.</returns>
        Task<List<string>> GetCollectionNames();

        /// <summary>
        /// Gets all animes from all collections.
        /// </summary>
        /// <returns>A list of animes from all collections.</returns>
        Task<List<AnimesModel>> GetAllAnimesFromAllCollections();

        /// <summary>
        /// Gets all animes in a specific collection.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <returns>A list of animes in the specified collection.</returns>
        Task<List<AnimesModel>> GetAllAnimesByCollectionName(string collectionName);

        /// <summary>
        /// Get the anime that match the front-end searchbar query.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="input">The name of the anime</param>
        /// <returns>A list of animes in the specified query.</returns>
        Task<List<AnimesModel>> GetAnimesBySearchbar(string collectionName,string input);


        /// <summary>
        /// Gets a anime by collection name and title anime.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="title">The Title of the anime.</param>
        /// <returns>The anime with the specified Title in the specified collection.</returns>
        Task<AnimesModel> GetAnimesByCollectionAndTitle(string collectionName, string title);

        /// <summary>
        /// Inserts a new anime into the specified collection.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="anime">The anime to insert.</param>
        Task InsertAnime(string collectionName, AnimesModel anime);

        /// <summary>
        /// Updates an existing anime in the specified collection.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="anime">The updated anime.</param>
        Task UpdateAnime(string collectionName, AnimesModel anime);

        /// <summary>
        /// Deletes a anime by ID from the specified collection.
        /// </summary>
        /// <param name="collectionName">The name of the collection.</param>
        /// <param name="id">The ID of the anime to delete.</param>
        Task DeleteAnime(string collectionName, string id);
    }
}