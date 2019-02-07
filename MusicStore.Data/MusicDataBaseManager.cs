using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Core;

namespace MusicStore.Data
{
    public  class MusicDataBaseManager
    {
        #region -Genre-
        public static IEnumerable<Genre> GetAllGenreList()
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Database.SqlQuery<Genre>("GetAllGenres").ToList();

                //return db.Genres.ToList<Genre>();
            }

        }

        public static IEnumerable<Genre> GetTopSellingGenres(int count)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Database.SqlQuery<Genre>("GetAllGenres").OrderByDescending(a => a.GenreId)
            .Take(count)
            .ToList();;

            }
        }
        // Retrieve Genre and its Associated Albums from database
        public static Genre BrowseGenre(string genre)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                //return db.Database.SqlQuery<Genre>("GetAllGenres").Include("Albums").Single(g => g.Name == genre);
                return db.Genres.Include("Albums").Single(g => g.Name == genre);

            }
        }
        #endregion

        #region -Album-
        public static Album BrowseAlbum(int id)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Albums.Find(id);
            }
            //MusicStoreEntities db = new MusicStoreEntities();

            //return db.Albums.Find(id);

        }
        public static IEnumerable<Album> GetAllAlbumList()
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Database.SqlQuery<Album>("GetAllAlbums").ToList();

            }


            //MusicStoreEntities db = new MusicStoreEntities();
            //return db.Albums.ToList<Album>();
        }
        public static Album FindAlbum(int id)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Albums.Find(id);
            }
            //MusicStoreEntities db = new MusicStoreEntities();
            //return db.Albums.Find(id);
        }

        public static void CreateAlbum(Album album)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                SqlParameter GenreId = new SqlParameter("@GenreId", album.GenreId);
                SqlParameter ArtistId = new SqlParameter("@ArtistId", album.ArtistId);
                SqlParameter Title = new SqlParameter("@Title", album.Title);
                SqlParameter Price = new SqlParameter("@Price", album.Title);
                SqlParameter AlbumArtUrl = new SqlParameter("@AlbumArtUrl", album.AlbumArtUrl);

                db.Database.ExecuteSqlCommand("InsertAlbum @GenreId,@ArtistId,@Title,@Price,@AlbumArtUrl", GenreId, ArtistId, Title, Price, AlbumArtUrl);

            }

            //db.Albums.Add(album);
            //db.SaveChanges();
        }
        public static void EditAlbum(Album album)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                db.Entry(album).State = EntityState.Modified;
                db.SaveChanges();
            }
            //MusicStoreEntities db = new MusicStoreEntities();
            //db.Entry(album).State = EntityState.Modified;
            //db.SaveChanges();
        }

        public static IEnumerable<Album> GetTopSellingAlbums(int count)
        {
            MusicStoreEntities db = new MusicStoreEntities();

            var rowcount = db.Albums
                //.OrderByDescending(a => a.OrderDetails.Count())
                .Take(count)
                .ToList().Count();

            //if (rowcount > 0)
            //{
                //return db.Albums
                //    .OrderByDescending(a => a.OrderDetails.Count())
                //    .Take(count)
                //    .ToList();
            //}
            //else
            {
                return db.Albums
               .Take(count)
               .ToList();
            }
        }

        #endregion

        #region -Artist-
        public static IEnumerable<Artist> GetAllArtistList()
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Database.SqlQuery<Artist>("GetAllArtists").ToList();

                //return db.Artists.ToList<Artist>();
            }
        //    MusicStoreEntities db = new MusicStoreEntities();
        //    return db.Artists.ToList<Artist>();
        }
        #endregion

        #region -Store Manger-
        public static IEnumerable<Album> GetAllStoreMangerList()
        {
            //MusicStoreEntities db = new MusicStoreEntities();
            //return db.Albums.Include(a => a.Genre).Include(a => a.Artist);

            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Albums.Include(a => a.Genre).Include(a => a.Artist);
            } 
        }
        #endregion
    }
}
