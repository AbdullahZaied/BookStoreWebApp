using System.ComponentModel.DataAnnotations;

namespace Business.Logic.Layer.Models
{
    public class SignInModelBusiness
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
