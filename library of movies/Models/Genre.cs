using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace library_of_movies.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Byte GenreId { get; set; }
        [Required  , MaxLength(100)]
        public string Name { get; set; }
    }
}
