using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLandWebApplication;

namespace GameLandWebApplication.Controllers
{
    public class GamesController : Controller
    {
        private readonly GameLandContext _context;

        public GamesController(GameLandContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Top(string? criteria)
        {
            var gameLandContext = _context.Games.Include(g => g.GamesGenres).ThenInclude(g => g.Genre).Include(g => g.GamesPlatforms).ThenInclude(g => g.Platform)
            .Include(g => g.GamesUsers).ThenInclude(g => g.User);
            var games = await gameLandContext.ToListAsync();

            switch (criteria)
            {
                case "by date":
                    games = games.OrderByDescending(item => games.Count != 0 ? item.GamesPlatforms.Min(x => x.ReleaseDate) : new DateTime()).ToList();
                    break;
                case "by name":
                    games = games.OrderBy(item => item.GameName).ToList();
                    break;
                case "by redaction rating":
                    games = games.OrderByDescending(item => item.RatingByRedaction).ToList();
                    break;
                case "by user rating":
                    games = games.OrderByDescending(m => m.GamesUsers.Count == 0 ? 0 : m.GamesUsers.Average(g => g.Rate)).ToList();
                    break;
                default:
                    games = games.OrderByDescending(m => m.RatingByRedaction).ThenByDescending(m => m.GamesUsers.Count == 0 ? 0 : m.GamesUsers.Average(g => g.Rate)).ThenBy(m => m.GameName).ToList();
                    break;
            }
            return View(games);
        }

        // GET: Games/GamesFilters/
        [Route("Games/GamesFilters/{id?}")]
        public async Task<IActionResult> GamesFilters(string id)
        {
            var gameLandContext = _context.Games.Include(g => g.GamesGenres).ThenInclude(g => g.Genre).Include(g => g.GamesPlatforms).ThenInclude(g => g.Platform)
            .Include(g => g.GamesUsers).ThenInclude(g => g.User);
            var games = await gameLandContext.ToListAsync();
            switch (id)
            {
                case "by_date": 
                    games = games.OrderByDescending(item => games.Count != 0 ? item.GamesPlatforms.Min(x => x.ReleaseDate) : new DateTime()).ToList() ;
                    break;
                case "by_name":
                    games = games.OrderBy(item => item.GameName).ToList();
                    break;
                case "by_redaction_rating":
                    games = games.OrderByDescending(item => item.RatingByRedaction).ThenByDescending(m => m.GamesUsers.Count == 0 ? 0 : m.GamesUsers.Average(g => g.Rate)).ThenBy(m => m.GameName).ToList();
                    break;
                case "by_user_rating":
                    games = games.OrderByDescending(m => m.GamesUsers.Count == 0 ? 0 : m.GamesUsers.Average(g => g.Rate)).ThenByDescending(m=>m.RatingByRedaction).ThenBy(m=>m.GameName).ToList();
                    break;
                default:
                    games = games.OrderByDescending(m => m.RatingByRedaction).ThenByDescending(m => m.GamesUsers.Count == 0 ? 0 : m.GamesUsers.Average(g => g.Rate)).ThenBy(m => m.GameName).ToList();
                    break;
            }

            return View(games);
        }

        // GET: Games/GameInfo/5
        public async Task<IActionResult> GameInfo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .Include(g => g.GamesGenres).ThenInclude(g => g.Genre)
                .Include(g => g.GamesPlatforms).ThenInclude(g => g.Platform)
                .Include(g => g.GamesSystemRequirements).ThenInclude(g => g.SystemRequirement)
                .Include(g => g.GamesUsers).ThenInclude(g => g.User)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        //GET: Games/Statistics
        public async Task<IActionResult> Statistics()
        {
            var gameLandContext = _context.Games.Include(g => g.GamesGenres).ThenInclude(g => g.Genre).Include(g => g.GamesPlatforms).ThenInclude(g => g.Platform)
            .Include(g => g.GamesUsers).ThenInclude(g => g.User);
            var games = await gameLandContext.ToListAsync();
            return View(games);
        }

            // GET: Games/Create
            public IActionResult Create()
        {
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperName");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,GameName,DeveloperId,PublisherId,LinkOnSteam,RatingByRedaction,RatingByUsers,Description,Photo")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Top));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperName", game.DeveloperId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", game.PublisherId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperName", game.DeveloperId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", game.PublisherId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,GameName,DeveloperId,PublisherId,LinkOnSteam,RatingByRedaction,RatingByUsers,Description,Photo")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Top));
            }
            ViewData["DeveloperId"] = new SelectList(_context.Developers, "DeveloperId", "DeveloperName", game.DeveloperId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "PublisherId", "PublisherName", game.PublisherId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .Include(g => g.Publisher)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Top));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
