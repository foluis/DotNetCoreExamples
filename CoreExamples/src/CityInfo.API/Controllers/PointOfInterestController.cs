using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/Cities")]
    public class PointOfInterestController : Controller
    {
        private ILogger<PointOfInterestController> _logger;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointOfInterestController(ILogger<PointOfInterestController> logger,
            IMailService localMailService, ICityInfoRepository cityInfoRepository)
        {

            _cityInfoRepository = cityInfoRepository;
            _logger = logger;
            _mailService = localMailService;
        }

        [HttpGet("{CityId}/pointsOfInterest")]
        public IActionResult GetPointOfInterestByCity(int cityId)
        {
            try
            {
                //throw new Exception("Example exeption"); //Ejemplo login

                if (!_cityInfoRepository.CityExists(cityId))
                {
                    return NotFound();
                }

                var pointsOfinterestForCity = _cityInfoRepository.GetPointsOfinterestForCity(cityId);

                var pointsOfinterestForCityResult = Mapper.Map<List<PointOfInterestDto>>(pointsOfinterestForCity);

                //foreach (var pointOfInt in pointsOfinterestForCity)
                //{
                //    pointsOfinterestForCityResult.Add(new PointOfInterestDto
                //    {
                //        Id = pointOfInt.Id,
                //        Name = pointOfInt.Name,
                //        Description = pointOfInt.Description
                //    });
                //}

                return Ok(pointsOfinterestForCityResult);

                //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with the id {cityId} wasn't found accessing point of interest.");
                //    return NotFound();
                //}
                //return Ok(city.PointsOfInterestDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception while getting points of interest for city with id {cityId}", ex);
                return StatusCode(500, "A problem happend while handling your request");
            }
        }

        [HttpGet("{CityId}/pointOfInterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterest = _cityInfoRepository.GetPointOfInterest(cityId, id);
            if (pointOfInterest == null)
            {
                return NotFound();
            }

            var pointOfInterestResult = Mapper.Map<PointOfInterestDto>(pointOfInterest);

            //var pointOfInterestResult = new PointOfInterestDto
            //{
            //    Id = pointOfInterest.Id,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            return Ok(pointOfInterestResult);

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointsOfInterestDto = city.PointsOfInterestDto.FirstOrDefault(x => x.Id == id);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointsOfInterestDto);
        }

        [HttpPost("{CityId}/pointOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            //FluentValidation Validation Library  for .net

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description shoud be diferent from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            if (_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            //var maxPointOfInterest = CitiesDataStore.Current.Cities
            //     .SelectMany(c => c.PointsOfInterestDto)
            //     .Max(p => p.Id);

            var newPointOfInterest = Mapper.Map<PointOfInterest>(pointOfInterest);

            //var newPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterest,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            //city.PointsOfInterestDto.Add(newPointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId,newPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500,"A problem happend while handdling your request.");
            }

            var createdPointOfInterest = Mapper.Map<PointOfInterestDto>(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = createdPointOfInterest.Id }, createdPointOfInterest);
        }

        [HttpPut("{CityId}/pointOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Name == pointOfInterest.Description)
            {
                ModelState.AddModelError("Description", "The provided description shoud be diferent from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointsOfInterestDto = city.PointsOfInterestDto.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            pointsOfInterestDto.Name = pointOfInterest.Name;
            pointsOfInterestDto.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{CityId}/pointOfInterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var currentPointsOfInterestDto = city.PointsOfInterestDto.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = currentPointsOfInterestDto.Name,
                Description = currentPointsOfInterestDto.Description
            };

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            currentPointsOfInterestDto.Name = pointOfInterestToPatch.Name;
            currentPointsOfInterestDto.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }


        [HttpDelete("{CityId}/pointOfInterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var currentPointsOfInterestDto = city.PointsOfInterestDto.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            city.PointsOfInterestDto.Remove(currentPointsOfInterestDto);

            _mailService.Send("Point of interest deleted", $"Point of interest {currentPointsOfInterestDto.Name} with id {currentPointsOfInterestDto.Id} deleted");

            return NoContent();
        }
    }
}
