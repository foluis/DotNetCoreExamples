using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        bool CityExists(int cityId);        

        IEnumerable<City> GetCities();

        City GetCity(int Id, bool includePointsOfInteres);

        IEnumerable<PointOfInterest> GetPointsOfinterestForCity (int id);

        PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId);

        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);

        bool Save();

    }
}
