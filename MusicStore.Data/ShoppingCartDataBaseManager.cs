using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicStore.Core;

namespace MusicStore.Data
{
    public class ShoppingCartDataBaseManager
    {
        static string ShoppingCartId { get; set; }

        public static List<Cart> GetCartItems(string ShoppingCartID)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                return db.Carts.Include("Album").Where(cart => cart.CartId == ShoppingCartID).ToList();
            }
        }

        public static void EmptyCart()
        {
            //MusicStoreEntities db = new MusicStoreEntities();
            //var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            //foreach (var cartItem in cartItems)
            //{
            //    db.Carts.Remove(cartItem);
            //}

            //// Save changes
            //db.SaveChanges();


            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

                foreach (var cartItem in cartItems)
                {
                    db.Carts.Remove(cartItem);
                }

                // Save changes
                db.SaveChanges();        
            }
        }

        public static int CreateOrder(Order order, string ShoppingCartID)
        {
            MusicStoreEntities db = new MusicStoreEntities();

            decimal orderTotal = 0;

            var cartItems = GetCartItems(ShoppingCartID);

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    AlbumId = item.AlbumId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Album.Price,
                    Quantity = item.Count
                };

                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Album.Price);

                //db.OrderDetails.Add(orderDetail);

                SqlParameter CartId = new SqlParameter("@CartId", item.CartId);
                SqlParameter AlbumId = new SqlParameter("@AlbumId", item.AlbumId);
                SqlParameter Count = new SqlParameter("@Count", item.Count);
                SqlParameter DateCreated = new SqlParameter("@DateCreated", item.DateCreated);

                db.Database.ExecuteSqlCommand("InsertCarts @CartId,@AlbumId,@Count,@DateCreated", CartId, AlbumId, Count, DateCreated);


            }

            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            //db.SaveChanges();

            // Empty the shopping cart
            EmptyCart();

            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        public static int GetCount()
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                // Get the count of each item in the cart and sum them up
                int? count = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count).Sum();

                // Return 0 if all entries are null
                return count ?? 0;
            } 
        }
        public static decimal GetTotal(string ShoppingCartID)
        {
            MusicStoreEntities db = new MusicStoreEntities();

            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartID
                              select (int?)cartItems.Count * cartItems.Album.Price).Sum();
            return total ?? decimal.Zero;
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public static void MigrateCart(string userName)
        {
            //MusicStoreEntities db = new MusicStoreEntities();
            //var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);
            //foreach (Cart item in shoppingCart)
            //{
            //    item.CartId = userName;
            //}
            //db.SaveChanges();

            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);
                foreach (Cart item in shoppingCart)
                {
                    item.CartId = userName;
                }
                db.SaveChanges(); 
            }
        }

        public static Album GetAlbum(int id)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                // Retrieve the album from the database
                //var addedAlbum = db.Albums
                //    .Single(album => album.AlbumId == id);

                //var addedAlbum = db.Carts.Where(c => c.RecordId == id);

                var addedAlbum = (from cartItems in db.Carts
                                  where cartItems.RecordId == id
                                  select cartItems.Album).Single();


                return addedAlbum;
            }

        }

        public static void AddOrder(Order order)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                db.Orders.Add(order);

                SqlParameter OrderDate = new SqlParameter("@OrderDate", order.OrderDate);
                SqlParameter Username = new SqlParameter("@Username", order.Username);
                SqlParameter FirstName = new SqlParameter("@FirstName", order.FirstName);
                SqlParameter LastName = new SqlParameter("@LastName", order.LastName);
                SqlParameter Address = new SqlParameter("@Address", order.Address);
                SqlParameter City = new SqlParameter("@City", order.City);

                SqlParameter State = new SqlParameter("@State", order.State);
                SqlParameter PostalCode = new SqlParameter("@PostalCode", order.PostalCode);
                SqlParameter Country = new SqlParameter("@Country", order.Country);
                SqlParameter Phone = new SqlParameter("@Phone", order.City);
                SqlParameter Email = new SqlParameter("@Email", order.City);
                SqlParameter Total = new SqlParameter("@Total", order.Total);

                db.Database.ExecuteSqlCommand("InsertOrders @OrderDate,@Username,@FirstName,@LastName,@Address,@City,@State,@PostalCode,@Country,@Phone,@Email,@Total",
                    OrderDate, Username, FirstName, LastName, Address, City, State, PostalCode, Country, Phone, Email, Total);


                //db.SaveChanges();
            }
        }

        public static bool IsValid(int id, string userName)
        {
            using (MusicStoreEntities db = new MusicStoreEntities())
            {
                bool isValid = db.Orders.Any(
               o => o.OrderId == id &&
               o.Username == userName);

                return isValid;
            }
        }
    }
}
