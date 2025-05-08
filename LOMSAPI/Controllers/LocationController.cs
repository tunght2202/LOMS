using LOMSAPI.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LOMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly LOMSDbContext _context;
        public LocationController(LOMSDbContext context) => _context = context;

        [HttpGet("/districts")]
        public async Task<IActionResult> GetDistricts(string provinceId)
        {
            var districts = await _context.Districts
                .Where(d => d.ProvinceCode == provinceId)
                .ToListAsync();
            return Ok(districts);
        }

        [HttpGet("/wards")]
        public async Task<IActionResult> GetWards(string districtId)
        {
            var wards = await _context.Wards
                .Where(w => w.DistrictCode == districtId)
                .ToListAsync();
            return Ok(wards);
        }
    }
}

