using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var citiesEntities = _cityInfoRepository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(citiesEntities);

            //foreach (var cityEntity in citiesEntities)
            //{
            //    results.Add(new CityWithoutPointOfInterestDto()
            //    {
            //        Id = cityEntity.Id,
            //        Name = cityEntity.Name,
            //        Description = cityEntity.Description
            //    });
            //}

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointOfInterest = false)
        {

            var city = _cityInfoRepository.GetCity(id, includePointOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointOfInterest)
            {
                var cityResult = Mapper.Map<CityDto>(city);
                //var cityResult = new CityDto
                //{
                //    Id = city.Id,
                //    Name = city.Description,
                //    Description = city.Name
                //};

                //foreach (var pointOfInterest in city.PointOfInterest)
                //{
                //    cityResult.PointsOfInterestDto.Add(
                //        new PointOfInterestDto
                //        {
                //            Id = pointOfInterest.Id,
                //            Name = pointOfInterest.Name,
                //            Description = pointOfInterest.Description
                //        });
                //}

                return Ok(cityResult);
            }

            var cityWithoutPointsOfInterest = Mapper.Map<CityWithoutPointOfInterestDto>(city);

            //var cityWithoutPointsOfInterest = new CityWithoutPointOfInterestDto
            //{
            //    Id = city.Id,
            //    Name = city.Description,
            //    Description = city.Name
            //};

            return Ok(cityWithoutPointsOfInterest);
        }
    }
}
