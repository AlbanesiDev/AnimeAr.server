using System;
using System.Threading.Tasks;
using anime_streaming.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDBAPI.Models;

namespace anime_streaming.Controllers.Animes
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnimesController : ControllerBase
    {
        private readonly IAnimeCollection db;

        public AnimesController(IConfiguration configuration)
        {
            db = new AnimeCollection(configuration);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimesFromAllCollections()
        {
            try
            {
                var allAnimes = await db.GetAllAnimesFromAllCollections();
                return Ok(allAnimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }

        [HttpGet("collection-names")]
        public async Task<ActionResult<List<string>>> GetCollectionNames()
        {
            try
            {
                var collectionNames = await db.GetCollectionNames();
                return Ok(collectionNames);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{collectionName}")]
        public async Task<IActionResult> GetAllAnimesByCollectionName(string collectionName)
        {
            try
            {
                var product = await db.GetAllAnimesByCollectionName(collectionName);

                if (product == null)
                {
                    return NotFound($"Collection with name {collectionName} not found");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }

        [HttpGet("{collectionName}/search")]
        public async Task<IActionResult> GetAnimesBySearchbar(string collectionName, [FromQuery] string searchbar)
        {
            try
            {
                var result = await db.GetAnimesBySearchbar(collectionName, searchbar);

                if (result == null)
                {
                    return NotFound($"Animes with name {searchbar} not found");
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }


        [HttpGet("{collectionName}/{title}")]
        public async Task<IActionResult> GetAnimesByCollectionAndTitle(string collectionName, string title)
        {
            try
            {
                var product = await db.GetAnimesByCollectionAndTitle(collectionName, title);

                if (product == null)
                {
                    return NotFound($"Animes with name {title} not found");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }

        [HttpPost("{collectionName}")]
        public async Task<IActionResult> CreateAnime(string collectionName, [FromBody] AnimesModel product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }
                await db.InsertAnime(collectionName, product);
                return Created("Create", true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }

        [HttpPut("{collectionName}/{id}")]
        public async Task<IActionResult> UpdateAnime(string collectionName, [FromBody] AnimesModel product, string id)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest();
                }

                product.Id = new MongoDB.Bson.ObjectId(id);
                await db.UpdateAnime(collectionName, product);

                return Created("Update", true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }

        [HttpDelete("{collectionName}/{id}")]
        public async Task<IActionResult> DeleteAnime(string collectionName, string id)
        {
            try
            {
                await db.DeleteAnime(collectionName, id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error 500: Internal server error {ex.Message}");
            }
        }
    }
}
