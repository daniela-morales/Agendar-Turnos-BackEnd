using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionTurnosApis.Models
{
    [Table("Shift", Schema = "dbo")]
    public class Shift
    {
        [Key()]
        public int IdShift { get; set; }
        public int? IdUser { get; set; }
        public int IdBranch { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAssociation { get; set; }
    }
}
