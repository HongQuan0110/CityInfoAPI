using AutoMapper;
using CityInfoAPI.Entities;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointOfInterestController(ILogger<PointOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            // Use data memory
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if(city == null)
            //{
            //    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
            //    return NotFound(new { message = "Not Found City"});
            //}
            //return Ok(new { number = city.PointOfInterest });

            // =================================================== //

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            // Use map handmade
            //var pointsOfInterest = _cityInfoRepository.GetPointsOfInterests(cityId);
            //var pointsOfInterestResult = new List<PointOfInterestDto>();

            //foreach(var point in pointsOfInterest)
            //{
            //    pointsOfInterestResult.Add(new PointOfInterestDto()
            //    {
            //        Id = point.Id,
            //        Name = point.Name,
            //        Description = point.Description
            //    });
            //}
            //return Ok(pointsOfInterestResult);

            // =================================================== //

            var pointsOfInterest = _cityInfoRepository.GetPointsOfInterests(cityId);
            var pointsOfInterestResult = Mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterest);
            return Ok(pointsOfInterestResult);
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            // Use data memory
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound(new { message = "Not Found City" });
            //}

            //var pointOfInterest = city.PointOfInterest.FirstOrDefault(p => p.Id == id);

            //if(pointOfInterest == null)
            //{
            //    return NotFound(new { message = "Not Found PointOfInterest" });
            //}
            //return Ok(pointOfInterest);

            // =================================================== //

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterest == null)
            {
                return NotFound();
            }

            // Use map handmade
            //var pointOfInterestResult = new PointOfInterestDto()
            //{
            //    Id = pointOfInterest.Id,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};
            //return Ok(pointOfInterestResult);

            // =================================================== //

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);
            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if(pointOfInterest == null)
            {
                return BadRequest();
            }

            if(pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Use map handmade

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if(city == null)
            //{
            //    return NotFound();
            //}

            //// demo purposes - to be improved
            //var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointOfInterest).Max(p => p.Id);

            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            //city.PointOfInterest.Add(finalPointOfInterest);
            //return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);
            
            // =================================================== //

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);
            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happend while handling your request");
            }

            var createPointOfInterestToReturn = Mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new { city = cityId, Id = createPointOfInterestToReturn.Id }, createPointOfInterestToReturn);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            
            if(pointOfInterest == null)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Use map handmade

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return BadRequest();
            //}

            //var pointOfInterestFromStore = city.PointOfInterest.FirstOrDefault(p => p.Id == id);
            //if(pointOfInterestFromStore == null)
            //{
            //    return BadRequest();
            //}

            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;

            //return NoContent();

            // =================================================== //

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterest == null)
            {
                return NotFound();
            }

            Mapper.Map(pointOfInterest, pointOfInterestEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happend while handling your request");
            }

            return NoContent();
        }

        [HttpPatch("{cityid}/pointsofinterest/{id}")]
        public IActionResult partiallyupdatepointofinterest(int cityid, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchdoc)
        {
            if (patchdoc == null)
            {
                return BadRequest();
            }

            // Use map handmade

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityid);
            //if(city == null)
            //{
            //    return BadRequest();
            //}

            //var pointOfInterestFromStore = city.PointOfInterest.FirstOrDefault(p => p.Id == id);
            //if(pointOfInterestFromStore == null)
            //{
            //    return BadRequest();
            //}

            //var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            //{
            //    Name = pointOfInterestFromStore.Name,
            //    Description = pointOfInterestFromStore.Description
            //};

            //patchdoc.ApplyTo(pointOfInterestToPatch, ModelState);

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            //{
            //    ModelState.AddModelError("Description", "The provided description should be different from the name");
            //}

            //TryValidateModel(pointOfInterestToPatch);

            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            //return NoContent();

            // =================================================== //

            if (!_cityInfoRepository.CityExists(cityid))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityid, id);
            if(pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = Mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchdoc.ApplyTo(pointOfInterestToPatch, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Name == pointOfInterestToPatch.Description)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            Mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happend while handling your request");
            }

            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            //if(city == null)
            //{
            //    return BadRequest();
            //}

            //var pointOfInterestFromStore = city.PointOfInterest.FirstOrDefault(p => p.Id == id);
            //if(pointOfInterestFromStore == null)
            //{
            //    return BadRequest();
            //}

            //city.PointOfInterest.Remove(pointOfInterestFromStore);

            // _mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            // =================================================== //

            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntiy = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterestEntiy == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterestForCity(pointOfInterestEntiy);
            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happend while handling your request");
            }

            _mailService.Send("Point of interest deleted", $"Point of interest {pointOfInterestEntiy.Name} with id {pointOfInterestEntiy.Id} was deleted.");

            return NoContent();
        }
    }
}
