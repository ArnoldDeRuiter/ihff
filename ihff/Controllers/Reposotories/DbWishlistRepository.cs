using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ihff.Controllers.Reposotories;
using ihff.Models;

namespace ihff.Controllers
{
    public class DbWishlistRepository : IWishlistRepository
    {
        private IHFFdatabasecontext ctx = new IHFFdatabasecontext();
        
        public IEnumerable<Item> GetAllItems()
        {
            List<Item> allItems = new List<Item>();
            foreach (Item item in ctx.Items)
            {
                allItems.Add(item);
            }
            return allItems;
        }

        public Item GetItem(int itemId)
        {
            return ctx.Items.SingleOrDefault(c => c.ItemId == itemId);
        }


        public bool checkTempCode(string code)
        {
            bool unique = true;
            foreach (Wishlist item in ctx.Wishlists)
            {
                if (item.WishlistCode == code)
                {
                    unique = false;
                    break;
                }
            }
            return unique;
        }

        public string getTempCode()
        {
            Random random = new Random();
            var CodeString = "";
            while (!checkTempCode(CodeString))
            {
            string possibleChars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";
            char[] codeStringChars = new char[5];


            for (int i = 0; i < codeStringChars.Length; i++)
            {
                codeStringChars[i] = possibleChars[random.Next(possibleChars.Length)];
            }

            CodeString = new String(codeStringChars);
            }
            return CodeString;
        }
        

    }
}