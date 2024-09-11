using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionTurnosApis.Models
{
    [Table("User", Schema = "dbo")]
    public class User
    {
        [Key()]
        public int IdUser { get; set; }
        public string DocumentNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
