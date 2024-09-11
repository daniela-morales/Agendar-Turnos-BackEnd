using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionTurnosApis.Models
{
    [Table("Branch", Schema = "dbo")]
    public class Branch
    {
        [Key()]
        public int IdBranch { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
