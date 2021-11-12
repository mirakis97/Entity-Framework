using RealEstates.Data;
using RealEstates.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public class DistrictsService : IDistrictsService
    {
        private readonly ApplicationDbContext dbContext;
        public DistrictsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<DistrictInfoDto> GetMostExpensiveDistricts(int count)
        {
            var district = dbContext.Districts.Select(x => new DistrictInfoDto
            {
                Name = x.Name,
                PropertiesCount = x.Properties.Count(),
                AveragePricePerSquareMeter = x.Properties.Where(x => x.Price.HasValue)
                .Average(x => x.Price / (decimal)x.Size) ?? 0,
            }).OrderByDescending(x => x.AveragePricePerSquareMeter).Take(count).ToList();

            return district;
        }
    }
}
