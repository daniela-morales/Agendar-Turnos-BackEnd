namespace GestionTurnosApis.Models.Dtos
{
    public class ShiftDto
    {
        public int IdShift { get; set; }
        public string? UserDocument { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public DateTime? ExpirationTime { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? DateAssociation { get; set; }
    }
}
