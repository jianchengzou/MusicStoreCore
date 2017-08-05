using MusicStoreCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { set; get; }
        public decimal CartTotal { set; get; }
    }
}
