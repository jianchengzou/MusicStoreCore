using Microsoft.AspNetCore.Mvc;
using MusicStoreCore.Models;
using MusicStoreCore.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.Controllers
{
    public class HomeController : Controller
    {
        private IAlbumData _albumData;

        public HomeController(IAlbumData albumData)
        {
            _albumData = albumData;
        }

        public IActionResult Index()
        {
            // Get most popular albums
            var albums = GetTopSellingAlbums(5);

            return View(albums);
        }

        private List<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return _albumData.GetAll()
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList();
        }
    }
}
