using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ihff.Models;
using System.Web.Mvc;

namespace ihff.Controllers.Reposotories
{
    interface IWishlistRepository
    {
        IEnumerable<Item> GetAllItems();
        Item GetItem(int itemId);

        bool checkTempCode(string code);
        string getTempCode();

    }
}