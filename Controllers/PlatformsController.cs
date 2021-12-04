using CacheService.Data;
using CacheService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CacheService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;

        public PlatformsController(IPlatformRepo repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Platform>> GetPlatforms()
        {
            return Ok(_repository.GetAllPlatforms());
        }

        [HttpGet("{id}", Name="GetPlatformById")]
        public ActionResult<IEnumerable<Platform>> GetPlatformById(string id)
        {
            
            var platform = _repository.GetPlatformById(id);
            
            if (platform != null)
            {
                return Ok(platform);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult <Platform> CreatePlatform(Platform platform)
        {
            _repository.CreatePlatform(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platform.Id}, platform);
        }
    }
}
