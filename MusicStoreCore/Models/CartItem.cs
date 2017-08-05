using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.Models
{
    public class CartItem
    {
        [Key]
        public int RecordId { get; set; }
        public string ShoppingCartId { get; set; }//ShoppingCartId
        public int AlbumId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Album Album { get; set; }
    }
}
