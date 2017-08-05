using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MusicStoreCore.Models
{
    public class User:IdentityUser
    {
        public User()
        {
            //...in case if customisation is needed
        }
    }
}
