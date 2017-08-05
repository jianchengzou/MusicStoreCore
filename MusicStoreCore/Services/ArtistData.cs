using Microsoft.EntityFrameworkCore;
using MusicStoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.Services
{
    public interface IArtistData
    {
        DbSet<Artist> GetAll();
        Artist Get(int artistId);
        Artist Add(Artist newArtist);
    }

    public class SqlArtistData : IArtistData
    {
        private MusicStoreDbContext _context;

        public SqlArtistData(MusicStoreDbContext context)
        {
            _context = context;
        }

        public Artist Add(Artist newArtist)
        {
            _context.Add(newArtist);
            _context.SaveChanges();
            return newArtist;
        }

        public Artist Get(int artistId)
        {
            return _context.Artists.FirstOrDefault(a => a.ArtistId == artistId);
        }

        public DbSet<Artist> GetAll()
        {
            return _context.Artists;
        }
    }
}
