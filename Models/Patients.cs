namespace HospitalOPD.Api.Models
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string UHID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string MobileNo { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; }  
    }

}
