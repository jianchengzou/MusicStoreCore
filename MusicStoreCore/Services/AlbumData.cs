using Microsoft.EntityFrameworkCore;
using MusicStoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.Services
{
    public interface IAlbumData
    {
        DbSet<Album> GetAll();
        Album Get(int albumId);
        Album Add(Album newAlbum);
        void Update(Album album);
        void Remove(Album album);
    }

    public class SqlAlbumData : IAlbumData
    {
        private MusicStoreDbContext _context;

        public SqlAlbumData(MusicStoreDbContext context)
        {
            _context = context;
        }

        public Album Add(Album newAlbum)
        {
            _context.Add(newAlbum);
            _context.SaveChanges();
            return newAlbum;
        }

        public Album Get(int albumId)
        {
            return _context.Albums.Include("Genre").Include("Artist")
                .FirstOrDefault(a => a.AlbumId == albumId);
        }

        public DbSet<Album> GetAll()
        {
            return _context.Albums;
        }

        public void Remove(Album album)
        {
            _context.Albums.Remove(album);
            _context.SaveChangesAsync();
        }

        public void Update(Album album)
        {
            _context.Update(album);
            _context.SaveChangesAsync();
        }
    }
}
