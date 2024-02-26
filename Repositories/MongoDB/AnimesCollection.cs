using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBAPI.Models;

namespace anime_streaming.Repositories
{
    /// <summary>
    /// Repository for accessing and managing anime collections in MongoDB.
    /// </summary>
    public class AnimeCollection : IAnimeCollection
    {
        internal MongoDBRepository _repository;

        /// <summary>
        /// Initializes a new instance of the AnimeCollection class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public AnimeCollection(IConfiguration configuration)
        {
            _repository = new MongoDBRepository(configuration);
        }

        //==========================================================================
        // Get the name of collections
        /// <inheritdoc/>
        public async Task<List<string>> GetCollectionNames()
        {
            return await _repository.GetCollectionNames();
        }

        //==========================================================================
        // Get all animes from all collections
        /// <inheritdoc/>
        public async Task<List<AnimesModel>> GetAllAnimesFromAllCollections()
        {
            var allAnimes = new List<AnimesModel>();

            foreach (var collectionName in _repository.CollectionNames)
            {
                var productsInCollection = await GetAllAnimesByCollectionName(collectionName);
                allAnimes.AddRange(productsInCollection);
            }
            return allAnimes;
        }

        //==========================================================================
        // Get all animes in a collection
        /// <inheritdoc/>
        public async Task<List<AnimesModel>> GetAllAnimesByCollectionName(string collectionName)
        {
            var collection = _repository.db.GetCollection<AnimesModel>(collectionName);
            var filter = Builders<AnimesModel>.Filter.Empty;
            return await collection.Find(filter).ToListAsync();
        }

        //==========================================================================
        // Get animes according to collection and title
        /// <inheritdoc/>
        public async Task<AnimesModel> GetAnimesByCollectionAndTitle(string collectionName, string title)
        {
            var collection = _repository.db.GetCollection<AnimesModel>(collectionName);

            var words = title.Split(' ');

            var filterDefinition = Builders<AnimesModel>.Filter.Empty;
            foreach (var word in words)
            {
                filterDefinition &= Builders<AnimesModel>.Filter.Regex(s => s.title, new BsonRegularExpression($".*{word}.*", "i"));
            }

            return await collection.Find(filterDefinition).FirstOrDefaultAsync();
        }

        //==========================================================================
        // Insert anime according to collection
        /// <inheritdoc/>
        public async Task InsertAnime(string collectionName, AnimesModel anime)
        {
            var collection = _repository.db.GetCollection<AnimesModel>(collectionName);
            await collection.InsertOneAsync(anime);
        }

        //==========================================================================
        // Update anime according to collection and id
        /// <inheritdoc/>
        public async Task UpdateAnime(string collectionName, AnimesModel anime)
        {
            var collection = _repository.db.GetCollection<AnimesModel>(collectionName);
            var filter = Builders<AnimesModel>.Filter.Eq(s => s.Id, anime.Id);
            await collection.ReplaceOneAsync(filter, anime);
        }

        //==========================================================================
        // Delete anime according to collection and id
        /// <inheritdoc/>
        public async Task DeleteAnime(string collectionName, string id)
        {
            var collection = _repository.db.GetCollection<AnimesModel>(collectionName);
            var filter = Builders<AnimesModel>.Filter.Eq(s => s.Id, new ObjectId(id));
            await collection.DeleteOneAsync(filter);
        }
    }
}
