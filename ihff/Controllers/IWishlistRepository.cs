using System.Collections.Generic;
using ihff.Models;

namespace ihff.Controllers
{
    public interface IWishlistRepository
    {
        void Add(ihff.Models.Item item);
    }
}