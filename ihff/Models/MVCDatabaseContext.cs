using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ihff.Models
{
    public class IHFFdatabasecontext: DbContext
    {
        // public DbSet<'ClassName'> classname {get; set;}

        public IHFFdatabasecontext()
            : base("MVCConnection")
        {
            Database.SetInitializer<IHFFdatabasecontext>(null);
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orderlines { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Item> Items { get; set; }

        // Alles in EventItem - Db is geweizigd
        //
        //public DbSet<Special> Special { get; set; }
        //public DbSet<Restaurant> Restaurant { get; set; }
        //public DbSet<Movie> Movie { get; set; }


    }
}