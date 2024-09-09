namespace GestionTurnosApis.Models
{
    public class Shift
    {
        public int IdShift { get; set; }
        public int IdUser { get; set; }
        public int IdBranch { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public bool IsActive { get; set; }
    }
}
