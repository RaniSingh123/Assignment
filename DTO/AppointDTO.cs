namespace HospitalOPD.Api.DTO
{
    public class AppointDTO
    {
            public int PatientId { get; set; }
            public int DoctorId { get; set; }
            public DateTime AppointmentDate { get; set; }
            public string AppointmentTime { get; set; } = string.Empty;
            public string Status { get; set; } = "Booked";
        
    }
}
