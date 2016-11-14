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

        public PointOfInterestController(ILogger<PointOfInterestController> logger, 
            IMailService localMailService)
        {
            _logger = logger;
            _mailService = localMailService;
        }

        [HttpGet("{CityId}/pointsOfInterest")]
        public IActionResult GetPointOfInterestByCity(int cityId)
        {
            try
            {
                //throw new Exception("Example exeption"); //Ejemplo login

                var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
                if (city == null)
                {
                    _logger.LogInformation($"City with the id {cityId} wasn't found accessing point of interest.");
                    return NotFound();
                }
                return Ok(city.PointsOfInterestDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception while getting points of interest for city with id {cityId}",ex);
                return StatusCode(500, "A problem happend while handling your request");
            }
        }

        [HttpGet("{CityId}/pointOfInterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
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

            return Ok(pointsOfInterestDto);
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

            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var maxPointOfInterest = CitiesDataStore.Current.Cities
                .SelectMany(c => c.PointsOfInterestDto)
                .Max(p => p.Id);

            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterest,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description
            };

            city.PointsOfInterestDto.Add(newPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = city.Id, id = newPointOfInterest.Id }, newPointOfInterest);
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
