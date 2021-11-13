using RealEstates.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Services
{
    public interface IPropertiesService
    {
       void Add(string district, int floor, int maxFloor, int size,int yardSize,int year, string propertyType,string bulidingType,int price);

        decimal AveragePriceSquareMeter();
        decimal AveragePriceSquareMeter(int districtId);
        public double AverageSize(int districtId);
        IEnumerable<PropertyInfoDto> Search(int minPrice,int maxPrice,int minSize,int maxSize);
    }
}
