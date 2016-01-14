using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ihff.Models;

namespace ihff.Controllers.Reposotories
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetAllItems();
        Item GetItem(int itemId);
        IEnumerable<Item> GetAllMovies();
        IEnumerable<Item> GetAllSpecials();
        IEnumerable<Item> GetAllDiners();
        Location GetItemLocation(int itemId);
        List<Item> GetItems(int itemId);
    }
}