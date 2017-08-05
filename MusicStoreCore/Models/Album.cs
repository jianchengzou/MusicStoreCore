using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MusicStoreCore.Models
{
    //[Bind(Exclude="AlbumId")] //Not supported with Exclude
    public class Album
    {
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }
        [Display(Name ="Genre")]
        public int GenreId { get; set; }
        [Display(Name ="Artist")]
        public int ArtistId { get; set; }
        [Required(ErrorMessage = "An album title is required")]
        [StringLength(160)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100.00, ErrorMessage = "Price must be between 0.01 and 100.00")]
        public decimal Price { get; set; }
        [Display(Name ="Album Art Url")]
        [StringLength(1024)]
        public string AlbumArtUrl { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
