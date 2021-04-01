using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameLandWebApplication;
using ClosedXML.Excel;
using ClosedXML.Attributes;
using ClosedXML.Utils;
using System.IO;

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

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var games = _context.Games.Include(g => g.GamesGenres).ThenInclude(g => g.Genre).Include(g => g.GamesPlatforms).ThenInclude(g => g.Platform).
                    Include(g=>g.Developer).Include(g => g.Publisher).ToList();
                
                    var worksheet = workbook.Worksheets.Add("AllGames");

                    worksheet.Cell("A1").Value = "GameName";
                    worksheet.Cell("B1").Value = "Developer";
                    worksheet.Cell("C1").Value = "Publisher";
                    worksheet.Cell("D1").Value = "LinkOnSteam";
                    worksheet.Cell("E1").Value = "RatingByRedaction";
                    worksheet.Cell("F1").Value = "Description";
                    worksheet.Cell("G1").Value = "PhotoLink";
                    worksheet.Cell("H1").Value = "Genre 1";
                    worksheet.Cell("I1").Value = "Genre 2";
                    worksheet.Cell("J1").Value = "Genre 3";
                    worksheet.Cell("K1").Value = "Genre 4";
                    worksheet.Cell("L1").Value = "Genre 5";
                    worksheet.Cell("M1").Value = "Genre 6";
                    worksheet.Cell("N1").Value = "Genre 7";
                    worksheet.Cell("O1").Value = "Genre 8";
                    worksheet.Cell("P1").Value = "Genre 9";
                    worksheet.Cell("Q1").Value = "Genre 10";
                    worksheet.Cell("R1").Value = "Genre 11";
                    worksheet.Cell("S1").Value = "Genre 12";
                    worksheet.Cell("T1").Value = "Platform 1";
                    worksheet.Cell("U1").Value = "Platform 2";
                    worksheet.Cell("V1").Value = "Platform 3";
                    worksheet.Cell("W1").Value = "Platform 4";
                    worksheet.Cell("X1").Value = "Platform 5";
                    worksheet.Cell("Y1").Value = "Platform 6";
                    worksheet.Cell("Z1").Value = "Platform 7";
                    worksheet.Row(1).Style.Font.Bold = true;
                
                    for (int i = 0; i < games.Count; i++)
                    {
                    worksheet.Cell(i + 2, 1).Value = games[i].GameName;
                    worksheet.Cell(i + 2, 2).Value = games[i].Developer.DeveloperName;
                    worksheet.Cell(i + 2, 3).Value = games[i].Publisher.PublisherName;
                    worksheet.Cell(i + 2, 4).Value = games[i].LinkOnSteam;
                    worksheet.Cell(i + 2, 5).Value = games[i].RatingByRedaction;
                    worksheet.Cell(i + 2, 6).Value = games[i].Description;
                    worksheet.Cell(i + 2, 7).Value = games[i].Photo;

                    worksheet.Row(2).AdjustToContents();
                    worksheet.Cell(i + 2, 6).Style.Alignment.WrapText = false;
                    worksheet.Column(1).AdjustToContents();
                    worksheet.Column(2).AdjustToContents();
                    worksheet.Column(3).AdjustToContents();
                    worksheet.Column(5).AdjustToContents();

                    var ab = _context.GamesGenres.Where(a => a.GameId == games[i].GameId).Include("Genre").ToList();
       
                        int j = 8;
                        foreach (var a in ab)
                        {
                            if (j < 20)
                            {
                                worksheet.Cell(i + 2, j).Value = a.Genre.GenreName;
                                worksheet.Column(j).AdjustToContents();
                                j++;
                            }
                        }
                    var bb = _context.GamesPlatforms.Where(a => a.GameId == games[i].GameId).Include("Platform").ToList();
       
                    j = 20;
                    foreach (var a in bb)
                    {
                        if (j < 27)
                        {
                            worksheet.Cell(i + 2, j).Value = a.Platform.PlatformName;
                            worksheet.Column(j).AdjustToContents();
                            j++;
                        }
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };

                }
            }            
        }
    }
}
