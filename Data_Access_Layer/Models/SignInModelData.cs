using System.ComponentModel.DataAnnotations;

namespace Data.Access.Layer.Models
{
    public class SignInModelData
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
