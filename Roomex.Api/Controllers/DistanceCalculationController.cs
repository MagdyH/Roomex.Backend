using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Roomex.Backend.Data.Dtos;
using Roomex.Backend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace Roomex.Api.Controllers
{
    /// <summary>
    /// Distance calculation Api controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class DistanceCalculationController : ControllerBase
    {
        private readonly IDistanceCalculationService _distanceCalculationService;
        private readonly ILogger<DistanceCalculationController> _logger;

        /// <summary>
        /// Initialize new instance of distance calculation Api controller
        /// </summary>
        /// <param name="distanceCalculationService"></param>
        /// <param name="logger"></param>
        public DistanceCalculationController(IDistanceCalculationService distanceCalculationService, ILogger<DistanceCalculationController> logger)
        {
            _distanceCalculationService = distanceCalculationService;
            _logger = logger;
        }

        /// <summary>
        /// Calculate distance between two points (a, b) in Km
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<ResponseDto>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public IActionResult CalculateDistance([FromBody] RequestDto request)
        {
            try
            {
                var result = _distanceCalculationService.CalculateDistanceInKM(request.PointA, request.PointB);
                return Ok(new ResponseDto { Distance = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
