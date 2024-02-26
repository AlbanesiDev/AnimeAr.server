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
        // Get all data from all collections
        /// <inheritdoc/>
        public async Task<List<object>> GetAllAnimesFromAllCollections()
        {
            var allAnimes = new List<object>();

            foreach (var collectionName in _repository.CollectionNames)
            {
                var productsInCollection = await GetAllAnimesByCollectionName(collectionName);
                allAnimes.AddRange(productsInCollection);
            }
            return allAnimes;
        }

        //==========================================================================
        // Get all data in a collection
        /// <inheritdoc/>
        public async Task<List<object>> GetAllAnimesByCollectionName(string collectionName)
        {
            var collection = _repository.db.GetCollection<object>(collectionName);
            var filter = Builders<object>.Filter.Empty;
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
        //==========================================================================
        // Get animes according to searchbar
        public async Task<List<AnimesModel>> GetAnimesBySearchbar(string collectionName, string input)
        {
            var collection = _repository.db.GetCollection<AnimesModel>(collectionName);

            // Dividir la cadena de búsqueda en palabras
            var words = input.Split(' ');

            // Construir un filtro que coincida con todas las palabras
            var filterDefinition = Builders<AnimesModel>.Filter.Empty;
            foreach (var word in words)
            {
                // Crear un filtro para cada palabra y combinarlos con un operador AND
                filterDefinition &= Builders<AnimesModel>.Filter.Regex(s => s.title, new BsonRegularExpression($".*{word}.*", "i"));
            }

            // Ordenar los resultados para que las coincidencias exactas aparezcan primero
            var sortDefinition = Builders<AnimesModel>.Sort.Descending(s => s.title);

            // Ejecutar la consulta y ordenar los resultados
            var result = await collection.Find(filterDefinition).Sort(sortDefinition).ToListAsync();

            return result;
        }
    }
}
