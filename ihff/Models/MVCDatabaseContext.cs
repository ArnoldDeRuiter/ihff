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

        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Location> Location { get; set; }
        public List<Item> Item { get; set; }

        // Alles in EventItem - Db is geweizigd
        //
        //public DbSet<Special> Special { get; set; }
        //public DbSet<Restaurant> Restaurant { get; set; }
        //public DbSet<Movie> Movie { get; set; }

        public IHFFdatabasecontext() 
            : base("MVCConnection")
        {
        }

        public System.Data.Entity.DbSet<ihff.Models.Item> Items { get; set; }
    }
}