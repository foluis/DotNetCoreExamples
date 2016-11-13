using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/Cities")]
    public class PointOfInterestController : Controller
    {
        [HttpGet("{CityId}/pointsOfInterest")]
        public IActionResult GetPointOfInterestByCity(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city.PointsOfInterestDto);
        }

        [HttpGet("{CityId}/pointOfInterest/{id}")]
        public IActionResult GetPointOfInterest(int cityId,int id)
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
    }
}
