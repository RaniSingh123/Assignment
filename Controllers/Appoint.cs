using HospitalOPD.Api.Data;
using HospitalOPD.Api.Helpers;
using HospitalOPD.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalOPD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class Appoint : ControllerBase
    {
        private readonly HospitalDBContext _context;

        public Appoint(HospitalDBContext context)
        {
            _context = context;
        }

        [HttpPost("book")]
        public async Task<IActionResult> Book(Appointment appt)
        {
            var doctor = await _context.Doctors.FindAsync(appt.DoctorId);
            if (doctor == null)
                return BadRequest(new ApiResponse { Success = false, Msg = "Doctor not found" });

            // Rule 1: within consultation time
            if (appt.AppointmentTime < doctor.ConsultationStartTime ||
                appt.AppointmentTime > doctor.ConsultationEndTime)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Msg = "Appointment outside consultation time"
                });
            }

            // Rule 2: same doctor same time
            if (await _context.Appointments.AnyAsync(a =>
                a.DoctorId == appt.DoctorId &&
                a.AppointmentDate.Date == appt.AppointmentDate.Date &&
                a.AppointmentTime == appt.AppointmentTime))
            {
                return BadRequest(new ApiResponse { Success = false, Msg = "Doctor already booked" });
            }

            // Rule 3: same patient same day
            if (await _context.Appointments.AnyAsync(a =>
                a.PatientId == appt.PatientId &&
                a.AppointmentDate.Date == appt.AppointmentDate.Date))
            {
                return BadRequest(new ApiResponse { Success = false, Msg = "Patient already has appointment today" });
            }

            appt.Status = "Booked";

            _context.Appointments.Add(appt);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse { Success = true, Msg = "Appointment booked", Data = appt });
        }

        [HttpGet("byDate")]
        public async Task<IActionResult> ByDate(DateTime date)
        {
            var list = await _context.Appointments
                .Where(x => x.AppointmentDate.Date == date.Date)
                .ToListAsync();

            return Ok(new ApiResponse { Success = true, Msg = "Success", Data = list });
        }

        [HttpGet("byDoctor/{doctorId}")]
        public async Task<IActionResult> ByDoctor(int doctorId)
        {
            var list = await _context.Appointments
                .Where(x => x.DoctorId == doctorId)
                .ToListAsync();

            return Ok(new ApiResponse { Success = true, Msg = "Success", Data = list });
        }
    }
}
