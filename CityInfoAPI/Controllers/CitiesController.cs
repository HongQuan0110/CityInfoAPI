using CityInfoAPI.Services;
using CityInfoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace CityInfoAPI.Controllers
{
    [Route("/api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetCities()
        {
            // Use data memory
            //return Ok(CitiesDataStore.Current.Cities);

            // =================================================== //

            // Mapper handmade
            //var cities = _cityInfoRepository.GetCities();
            ////return Ok(cities);
            //var results = new List<CityWithoutPointsOfInterestDto>();
            //foreach(var city in cities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = city.Id,
            //        Name = city.Name,
            //        Description = city.Description
            //    });
            //}
            //return Ok(results);

            // =================================================== //

            // Use Mapper
            var cities = _cityInfoRepository.GetCities();
            var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            // Use data memory
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            //if(city == null)
            //{
            //    return NotFound( new { message = "Not found city" });
            //}

            //return Ok(city);

            // =================================================== //

            // Mapper handmade
            //var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            ////return Ok(city);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //if (includePointsOfInterest)
            //{
            //    var cityResult = new CityDto()
            //    {
            //        Id = city.Id,
            //        Name = city.Name,
            //        Description = city.Description,

            //    };

            //    foreach(var point in city.PointsOfInterest)
            //    {
            //        cityResult.PointOfInterest.Add(new PointOfInterestDto()
            //        {
            //            Id = point.Id,
            //            Name = point.Name,
            //            Description = point.Description
            //        });
            //    }

            //    return Ok(cityResult);
            //}

            //var cityWithoutPointOfInterestResult = new CityWithoutPointsOfInterestDto()
            //{
            //    Id = city.Id,
            //    Name = city.Name,
            //    Description = city.Description
            //};

            //return Ok(cityWithoutPointOfInterestResult);

            // =================================================== //

            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);
            if(city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                cityResult.PointOfInterest = Mapper.Map<ICollection<PointOfInterestDto>>(city.PointsOfInterest);
                
                return Ok(cityResult);
            }

            var cityWithoutPointOfInterestResult = Mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointOfInterestResult);
        }
    }
}
