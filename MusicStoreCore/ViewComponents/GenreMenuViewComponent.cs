using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MusicStoreCore.Services;

namespace MusicStoreCore.ViewComponents
{
    public class GenreMenuViewComponent : ViewComponent
    {
        private IGenreData _genreData;

        public GenreMenuViewComponent(IGenreData genreData)
        {
            _genreData = genreData;
        }

        public IViewComponentResult Invoke()
        {
            var genres = _genreData.GetAll().ToList();
            return View("GenreMenu", genres);
        }
    }
}
