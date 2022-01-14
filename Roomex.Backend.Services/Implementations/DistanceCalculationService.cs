using Roomex.Backend.Data;
using Roomex.Backend.Services.Interfaces;
using System;

namespace Roomex.Backend.Services.Implementations
{
    /// <inheritdoc cref="IDistanceCalculationService"/>
    public class DistanceCalculationService : IDistanceCalculationService
    {
        private const double r = 6371;
        public double CalculateDistanceInKM(Point pointA, Point pointB)
        {
            var lonA = ConvertDegreeToRadians(pointA.Longitude);
            var lonB = ConvertDegreeToRadians(pointB.Longitude);
            var latA = ConvertDegreeToRadians(pointA.Latitude);
            var latB = ConvertDegreeToRadians(pointB.Latitude);

            double dlon = lonB - lonA;
            double dlat = latB - latA;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) +
                       Math.Cos(latA) * Math.Cos(latB) *
                       Math.Pow(Math.Sin(dlon / 2), 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            return (c * r);
        }

        /// <summary>
        /// Convert lan and lon degrees to radians
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        private double ConvertDegreeToRadians(double degree)
        {
            return (degree *
                       Math.PI) / 180;
        }
    }
}
