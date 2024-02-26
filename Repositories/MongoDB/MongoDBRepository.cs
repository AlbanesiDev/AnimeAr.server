using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace anime_streaming.Repositories
{
    /// <summary>
    /// MongoDBRepository class responsible for connecting to MongoDB and providing access to collections.
    /// </summary>
    public class MongoDBRepository
    {
        private readonly IConfiguration _configuration;
        public MongoClient client;
        public IMongoDatabase db;

        // List to store collection names
        public List<string> CollectionNames { get; private set; } = [];

        /// <summary>
        /// Initializes a new instance of the MongoDBRepository class.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        public MongoDBRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            // Get MongoDB connection string from configuration
            string password = _configuration["MongoDB:AnimeStreaming"] ?? "";

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Error 404: MongoDB password not found in configuration.");
            }

            // Create MongoClient using the connection string
            client = new MongoClient(password);

            // Get the MongoDB database
            db = client.GetDatabase("anime");

            // Initialize the collection names
            InitializeCollectionNames();
        }

        /// <summary>
        /// Initializes the CollectionNames property with the names of all collections in the database.
        /// </summary>
        private void InitializeCollectionNames()
        {
            var filter = new BsonDocument();
            var options = new ListCollectionNamesOptions { Filter = filter };

            // Use ListCollectionNames to get the names of all collections in the database
            using var cursor = db.ListCollectionNames(options);
            CollectionNames = cursor.ToList();
        }

        public async Task<List<string>> GetCollectionNames()
        {
            var filter = new BsonDocument();
            var options = new ListCollectionNamesOptions { Filter = filter };

            // Use ListCollectionNamesAsync to get the names of all collections in the database asynchronously
            using var cursor = await db.ListCollectionNamesAsync(options);
            return cursor.ToList();
        }
    }
}
