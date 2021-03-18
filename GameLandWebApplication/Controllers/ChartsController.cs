using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GameLandWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly GameLandContext _context;

        public ChartsController(GameLandContext context)
        {
            _context = context;
        }
        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var platforms = _context.Platforms.Include(g => g.GamesPlatforms).ToList();

            List<object> platformGames = new List<object>();

            platformGames.Add(new[] { "Platform", "Amount of games" });    
            
            foreach(var c in platforms)
            {
                platformGames.Add(new object[] { c.PlatformName, c.GamesPlatforms.Count() });
            }
            return new JsonResult(platformGames);
        }
        [HttpGet("JsonDota")]
        public JsonResult JsonDota()
        {
            var genres = _context.Genres.Include(g => g.GamesGenres).OrderByDescending(g => g.GamesGenres.Count()).ToList().Take(10);

            List<object> genreGames = new List<object>();

            genreGames.Add(new[] { "Genre", "Amount of games" });

            foreach (var c in genres)
            {
                genreGames.Add(new object[] { c.GenreName, c.GamesGenres.Count() });
            }
            return new JsonResult(genreGames);
        }
    }
}
