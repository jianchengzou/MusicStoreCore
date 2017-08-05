using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicStoreCore.Models;
using MusicStoreCore.Services;
using Microsoft.AspNetCore.Authorization;

namespace MusicStoreCore.Controllers
{
    [Authorize(Roles ="Admins")]
    public class StoreManagerController : Controller
    {
        private IAlbumData _albumData;
        private IArtistData _artistData;
        private IGenreData _genreData;

        public StoreManagerController(IAlbumData albumData, IArtistData artistData, IGenreData genreData)
        {
            _albumData = albumData;
            _artistData = artistData;
            _genreData = genreData;
        }

        // GET: StoreManager
        public async Task<IActionResult> Index()
        {
            var musicStoreDbContext = _albumData.GetAll().Include(a => a.Artist).Include(a => a.Genre);
            return View(await musicStoreDbContext.ToListAsync());
        }

        // GET: StoreManager/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumData.GetAll()
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .SingleOrDefaultAsync(m => m.AlbumId == id);

            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // GET: StoreManager/Create
        public IActionResult Create()
        {
            ViewData["ArtistId"] = new SelectList(_artistData.GetAll(), nameof(Artist.ArtistId), "Name");
            ViewData["GenreId"] = new SelectList(_genreData.GetAll(), nameof(Genre.GenreId), "Name");
            
            return View();
        }

        // POST: StoreManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (ModelState.IsValid)
            {
                _albumData.Add(album);
                return RedirectToAction("Index");
            }
            //var aa = album.Artist.Name;
            //var bb = album.Genre.Name;
            ViewData["ArtistId"] = new SelectList(_artistData.GetAll(), nameof(Artist.ArtistId), "Name");
            ViewData["GenreId"] = new SelectList(_genreData.GetAll(), nameof(Genre.GenreId), "Name");
            return View(album);
        }

        // GET: StoreManager/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumData.GetAll().SingleOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["ArtistId"] = new SelectList(_artistData.GetAll(), nameof(Artist.ArtistId), "Name");
            ViewData["GenreId"] = new SelectList(_genreData.GetAll(), nameof(Genre.GenreId), "Name");
            return View(album);
        }

        // POST: StoreManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumId,GenreId,ArtistId,Title,Price,AlbumArtUrl")] Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _albumData.Update(album);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            ViewData["ArtistId"] = new SelectList(_artistData.GetAll(), nameof(Artist.ArtistId), "Name");
            ViewData["GenreId"] = new SelectList(_genreData.GetAll(), nameof(Genre.GenreId), "Name");
            return View(album);
        }

        // GET: StoreManager/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _albumData.GetAll()
                .Include(a => a.Artist)
                .Include(a => a.Genre)
                .SingleOrDefaultAsync(m => m.AlbumId == id);

            return (album == null) ? (IActionResult)NotFound() : View(album);
        }

        // POST: StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _albumData.GetAll().SingleOrDefaultAsync(m => m.AlbumId == id);
            _albumData.Remove(album);
            return RedirectToAction("Index");
        }

        private bool AlbumExists(int id)
        {
            return _albumData.GetAll().Any(e => e.AlbumId == id);
        }
    }
}
