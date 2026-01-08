namespace HospitalOPD.Api.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public TimeSpan ConsultationStartTime { get; set; }
        public TimeSpan ConsultationEndTime { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }

}
