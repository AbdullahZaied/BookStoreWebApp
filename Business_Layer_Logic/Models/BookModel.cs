using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Business.Logic.Layer.Models
{
    public class BookModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string BookName { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Author { get; set; }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Category { get; set; }
    }
}
