using Roomex.Backend.Data;

namespace Roomex.Backend.Services.Interfaces
{
    public interface IDistanceCalculationService
    {
        /// <summary>
        /// Calculate coordinates distance between two points
        /// </summary>
        /// <param name="pointA"></param>
        /// <param name="pointB"></param>
        /// <returns></returns>
        double CalculateDistanceInKM(Point pointA, Point pointB);
    }
}
