using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;


namespace MusicStoreCore.Models
{
    public static class DbContextExtension
    {
        public static bool AllMigrationsApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            return !total.Except(applied).Any();
        }

        public static void EnsureSeeded(this MusicStoreDbContext context)
        {
            var artists = SampleData.Artists;
            var genres = SampleData.Genres;
            var albums = SampleData.GetAlbums(genres, artists);

            //Ensure we have some status
            if (!context.Genres.Any())
            {
                context.AddRange(genres);
                context.SaveChanges();

            }
            //Ensure we create initial Threat List
            if (!context.Artists.Any())
            {
                context.AddRange(artists);
                context.SaveChanges();
            }

            if (!context.Albums.Any())
            {
                context.AddRange(albums);
                context.SaveChanges();
            }

            
        }
    }
}
