using HospitalOPD.Api.Data;
using HospitalOPD.Api.DTO;
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
    public class AppointmentController : ControllerBase
    {
        private readonly HospitalDBContext _context;

        public AppointmentController(HospitalDBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("book")]
        public async Task<IActionResult> Book([FromBody] AppointDTO dto)
        {
            if (!TimeSpan.TryParse(dto.AppointmentTime, out TimeSpan time))
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Msg = "Invalid time format. Use HH:mm"
                });
            }

            var doctor = await _context.Doctors.FindAsync(dto.DoctorId);
            if (doctor == null)
                return BadRequest(new ApiResponse { Success = false, Msg = "Doctor not found" });

            if (time < doctor.ConsultationStartTime || time > doctor.ConsultationEndTime)
                return BadRequest(new ApiResponse { Success = false, Msg = "Outside consultation time" });

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                DoctorId = dto.DoctorId,
                AppointmentDate = dto.AppointmentDate,
                AppointmentTime = time,   // ✅ TimeSpan assigned here
                Status = dto.Status
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Msg = "Appointment booked",
                Data = appointment
            });
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
