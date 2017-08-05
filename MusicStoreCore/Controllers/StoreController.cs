using Microsoft.AspNetCore.Mvc;
using MusicStoreCore.Models;
using MusicStoreCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MusicStoreCore.Controllers
{
    //[Authorize]
    public class StoreController : Controller
    {
        private IGenreData _genreData;
        private IAlbumData _albumData;

        public StoreController(IGenreData genreData, IAlbumData albumData)
        {
            _genreData = genreData;
            _albumData = albumData;                
        }
        //
        // GET: /Store/
        //[AllowAnonymous]
        public IActionResult Index()
        {
            //var genres = new List<Genre>
            //{
            //    new Genre { Name = "Disco"},
            //    new Genre { Name = "Jazz"},
            //    new Genre { Name = "Rock"}
            //};
            return View(_genreData.GetAll().ToList()); 
        }
        //
        // GET: /Store/Browse
        public IActionResult Browse(string genre)
        {
            var genreModel = _genreData.GetAll().Include("Albums").Single(g => g.Name == genre);
            return View(genreModel);
        }
        //
        // GET: /Store/Details
        public IActionResult Details(int id)
        {
            var album = _albumData.Get(id);
            return View(album);
        }

    }
}

