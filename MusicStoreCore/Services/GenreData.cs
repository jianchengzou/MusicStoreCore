using Microsoft.EntityFrameworkCore;
using MusicStoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.Services
{
    public interface IGenreData
    {
        DbSet<Genre> GetAll();
        Genre Get(int genreId);
        Genre Add(Genre newGenre);
    }

    public class SqlGenreData : IGenreData
    {
        private MusicStoreDbContext _context;

        public SqlGenreData(MusicStoreDbContext context)
        {
            _context = context;
        }

        public Genre Add(Genre newGenre)
        {
            _context.Add(newGenre);
            _context.SaveChanges();
            return newGenre;
        }

        public Genre Get(int genreId)
        {
            return _context.Genres.FirstOrDefault(a => a.GenreId == genreId);
        }

        public DbSet<Genre> GetAll()
        {
            return _context.Genres;
        }
    }
}
