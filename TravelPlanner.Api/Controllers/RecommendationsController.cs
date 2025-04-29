using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Domain.Interfaces;
using System.Diagnostics;

namespace TravelPlanner.Api.Controllers
{
    [ApiController]
    [Route("reco")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly ILogger<RecommendationsController> _logger;

        public RecommendationsController(IRecommendationService recommendationService, ILogger<RecommendationsController> logger)
        {
            _recommendationService = recommendationService;
            _logger = logger;
        }

        // GET /reco?city=XXX&k=3
        [HttpGet]
        public async Task<IActionResult> GetRecommendations([FromQuery] string city, [FromQuery] int k = 3)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var recommendations = await _recommendationService.GetRecommendationsAsync(city, k);
                sw.Stop();
                _logger.LogInformation("GET /reco executed in {Ms} ms", sw.ElapsedMilliseconds);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "Error in GET /reco after {Ms} ms", sw.ElapsedMilliseconds);
                return StatusCode(500, new { error = "Internal Server Error", duration_ms = sw.ElapsedMilliseconds });
            }
        }
    }
}
