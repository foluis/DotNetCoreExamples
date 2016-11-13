using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id=1,
                    Name = "Bogotá",
                    Description = "2600 metros...",
                    PointsOfInterestDto = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=1,
                            Name = "Moncerrate",
                            Description = "2600 metros..."
                        },
                        new PointOfInterestDto
                        {
                            Id=2,
                            Name = "Museo del oro",
                            Description = "Capital de la salsa"
                        }
                        ,
                        new PointOfInterestDto()
                        {
                            Id=3,
                            Name = "Jardin Botanico",
                            Description = "Nillas lindas"
                        }
                    }
                },
                new CityDto
                {
                    Id=2,
                    Name = "Cali",
                    Description = "Capital de la salsa",
                    PointsOfInterestDto = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=4,
                            Name = "Plaza de toros",
                            Description = "2600 metros..."
                        },
                        new PointOfInterestDto
                        {
                            Id=5,
                            Name = "Chipinque",
                            Description = "Capital de la salsa"
                        }
                        ,
                        new PointOfInterestDto()
                        {
                            Id=6,
                            Name = "Congreso",
                            Description = "Nillas lindas"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Medellin",
                    Description = "Nillas lindas",
                    PointsOfInterestDto = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id=7,
                            Name = "PLaza",
                            Description = "2600 metros..."
                        },
                        new PointOfInterestDto
                        {
                            Id=8,
                            Name = "Torre",
                            Description = "Capital de la salsa"
                        }
                        ,
                        new PointOfInterestDto()
                        {
                            Id=9,
                            Name = "Centro comercial",
                            Description = "Nillas lindas"
                        }
                    }
                }
            };
        }
    }
}
