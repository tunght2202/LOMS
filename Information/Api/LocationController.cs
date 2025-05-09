
using Information.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InformationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LomsdatabaseContext _context;
        public LocationController(LomsdatabaseContext context) => _context = context;

        [HttpGet("/api/districts")]
        public async Task<IActionResult> GetDistricts(string provinceId)
        {
            var districts = await _context.Districts
                .Where(d => d.ProvinceCode == provinceId)
                .ToListAsync();
            return Ok(districts);
        }

        [HttpGet("/api/wards")]
        public async Task<IActionResult> GetWards(string districtId)
        {
            var wards = await _context.Wards
                .Where(w => w.DistrictCode == districtId)
                .ToListAsync();
            return Ok(wards);
        }
    }
}
