using library_of_movies.Models;
using library_of_movies.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_of_movies.Controllers
{

    public class MovieController : Controller
    {
        private readonly ApplicationDbcontext _context;
        public MovieController(ApplicationDbcontext context)
        {
            _context = context;
        }
        public async Task<IActionResult> index()
        {
            var movie = await _context.Movies.ToListAsync();
            return View(movie);
        }
        public async Task<IActionResult> create()
        {
            var MovieForm = new MovieFormViewModel
            {

                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            };
            return View("movie Form", MovieForm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(MovieFormViewModel model)
        {

            model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();


            var files = Request.Form.Files;

            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select movie poster!");
                return View(model);
            }

            var poster = files.FirstOrDefault();
            using var dataStream = new MemoryStream();

            await poster.CopyToAsync(dataStream);

            var movies = new Movie
            {
                Title = model.Title,
                GenreId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                StoryLine = model.StoryLine,
                Poster = dataStream.ToArray(),

            };

            _context.Movies.Add(movies);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound();
            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                Year = movie.Year,
                Rate = movie.Rate,
                StoryLine = movie.StoryLine,
                Poster = movie.Poster,
                Title = movie.Title,
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            };
            return View("movie Form", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {

            model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
            var movie = await _context.Movies.FindAsync(model.Id);


            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                using var dataStream = new MemoryStream();
                await poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }
            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.StoryLine = model.StoryLine;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) 
                return BadRequest();

            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return Ok();
        }
    }


}
