using HospitalOPD.Api.Data;
using HospitalOPD.Api.Helpers;
using HospitalOPD.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalOPD.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
            private readonly HospitalDBContext _context;

            public PatientController(HospitalDBContext context)
            {
                _context = context;
            }

            [HttpPost]
            public async Task<IActionResult> Add(Patient patient)
            {
                if (patient.MobileNo.Length != 10)
                    return BadRequest(new ApiResponse { Success = false, Msg = "Mobile must be 10 digits" });

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse { Success = true, Msg = "Patient added", Data = patient });
            }

            [HttpGet]
            public async Task<IActionResult> List()
            {
                var patients = await _context.Patients.ToListAsync();
                return Ok(new ApiResponse { Success = true, Msg = "Success", Data = patients });
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> Get(int id)
            {
                var patient = await _context.Patients.FindAsync(id);
                if (patient == null)
                    return NotFound(new ApiResponse { Success = false, Msg = "Not found" });

                return Ok(new ApiResponse { Success = true, Msg = "Success", Data = patient });
            }
        }
    
}
