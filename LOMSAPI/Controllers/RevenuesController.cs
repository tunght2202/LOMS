using LOMSAPI.Repositories.Revenues;
using Microsoft.AspNetCore.Mvc;

namespace LOMSAPI.Controllers
{
    public class RevenuesController : ControllerBase
    {
        private readonly IRevenueRepository _revenueRepository;

        public RevenuesController(IRevenueRepository revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }

        public async Task<decimal> GetTotalRevenue() => await _revenueRepository.GetTotalRevenue();

        public async Task<int> GetTotalOrders() => await _revenueRepository.GetTotalOrders();

        public async Task<decimal> GetRevenueByLivestreamId(int livestreamId) => await _revenueRepository.GetRevenueByLivestreamId(livestreamId);
    }
}
