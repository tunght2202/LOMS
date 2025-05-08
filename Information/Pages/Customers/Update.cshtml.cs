using Information.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Information.Pages.Customers
{
    public class UpdateModel : PageModel
    {
        private readonly LomsdatabaseContext _context;

        public UpdateModel(LomsdatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Customer Customer { get; set; }

        public List<Province> Provinces { get; set; } = new();
        public string? SuccessMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Customer = await _context.Customers.FindAsync(id);
            if (Customer == null)
            {
                return NotFound();
            }

            Provinces = await _context.Provinces.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var customerInDb = await _context.Customers.FindAsync(Customer.CustomerId);
            if (customerInDb == null)
            {
                return NotFound();
            }

            customerInDb.ProvinceId = Customer.ProvinceId;
            customerInDb.DistrictId = Customer.DistrictId;
            customerInDb.WardId = Customer.WardId;
            customerInDb.DetailAddress = Customer.DetailAddress;            
            customerInDb.PhoneNumber = Customer.PhoneNumber;
           
               // Lấy tên địa phương từ DB
                var province = await _context.Provinces
                .Where(p => p.Code == Customer.ProvinceId.ToString())
                .Select(p => p.Name)
                .FirstOrDefaultAsync();

                var district = await _context.Districts
                    .Where(d => d.Code == Customer.DistrictId.ToString())
                    .Select(d => d.Name)
                    .FirstOrDefaultAsync();

                var ward = await _context.Wards
                    .Where(w => w.Code == Customer.WardId.ToString())
                    .Select(w => w.Name)
                    .FirstOrDefaultAsync();

                // Gộp địa chỉ đầy đủ
                customerInDb.Address = $"{Customer.DetailAddress}, {ward}, {district}, {province}";

            await _context.SaveChangesAsync();

            Provinces = await _context.Provinces.ToListAsync(); // Load lại dropdown
            SuccessMessage = "Cập nhật thông tin thành công.";

            return Page();
        }
    }
}
