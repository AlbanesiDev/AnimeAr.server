using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBAPI.Models
{
    public class AnimesModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string CollectionName { get; set; }

        [BsonElement("title")]
        public string title { get; set; }

        [BsonElement("type")]
        public string type { get; set; }

        [BsonElement("txtAlt")]
        public string[] txtAlt { get; set; }

        [BsonElement("genres")]
        public string[] genres { get; set; }

        [BsonElement("synopsis")]
        public string synopsis { get; set; }

        [BsonElement("cover")]
        public string cover { get; set; }

        [BsonElement("status")]
        public string status { get; set; }

        [BsonElement("related_animes")]
        public List<RelatedAnime> relatedAnimes { get; set; }
        
        [BsonElement("episodes")]
        public object? episodes { get; set; }

        public AnimesModel()
        {
            CollectionName = string.Empty;
            title = string.Empty;
            type = string.Empty;
            txtAlt = [];
            genres = [];
            synopsis = string.Empty;
            cover = string.Empty;
            status = string.Empty;
            relatedAnimes = new List<RelatedAnime>();
            episodes = null;
        }

        public class RelatedAnime
        {
            [BsonElement("title")]
            public string title { get; set; }
            [BsonElement("relation")]
            public string relation { get; set; }

            public RelatedAnime()
            {
                title = string.Empty;
                relation = string.Empty;
            }
        }
    }
}