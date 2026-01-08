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
    public class DoctorsController : ControllerBase
    {
        private readonly HospitalDBContext _context;

        public DoctorsController(HospitalDBContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public async Task<IActionResult> Add(Doctor doctor)
        //{
        //    _context.Doctors.Add(doctor);
        //    await _context.SaveChangesAsync();

        //    return Ok(new ApiResponse { Success = true, Msg = "Doctor added", Data = doctor });
        //}

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return Ok(new ApiResponse { Success = true, Msg = "Success", Data = doctors });
        }

        [HttpPost]
        public async Task<IActionResult> Add(Doctor dto)
        {
            var doctor = new Doctor
            {
                Name = dto.Name,
                Department = dto.Department,
                ConsultationStartTime = dto.ConsultationStartTime,
                ConsultationEndTime = dto.ConsultationEndTime
            };

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse
            {
                Success = true,
                Msg = "Doctor added",
                Data = doctor
            });
        }

    }
}
