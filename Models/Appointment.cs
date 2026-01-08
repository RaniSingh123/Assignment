namespace HospitalOPD.Api.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; } = string.Empty;

        public Patient Patient { get; set; }    
        public Doctor Doctor { get; set; }
    }

}
