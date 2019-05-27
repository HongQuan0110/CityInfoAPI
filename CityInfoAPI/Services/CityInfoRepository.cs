using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityInfoAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfoAPI.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        public CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context;
        }


        public bool CityExists(int cityId)
        {
            return _context.Cities.Any(c => c.Id == cityId);
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(c => c.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest).Where(c => c.Id == cityId).FirstOrDefault();
            }
            return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.poinstOfInterest.Where(c => c.Id == pointOfInterestId && c.CityId == cityId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterests(int cityId)
        {
            return _context.poinstOfInterest.Where(c => c.CityId == cityId).ToList();
        }
    }
}
