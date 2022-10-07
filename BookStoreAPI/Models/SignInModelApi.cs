using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public class SignInModelApi
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
