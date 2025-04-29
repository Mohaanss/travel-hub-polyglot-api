using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Domain.Interfaces;
using System.Diagnostics;
using TravelPlanner.Domain.Models;

namespace TravelPlanner.Api.Controllers
{
    [ApiController]
    [Route("offers")]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;
        private readonly IOfferCreationService _offerCreationService;
        private readonly ILogger<OffersController> _logger;

        public OffersController(IOfferService offerService, IOfferCreationService offerCreationService, ILogger<OffersController> logger)
        {
            _offerService = offerService;
            _offerCreationService = offerCreationService;
            _logger = logger;
        }

        // GET /offers?from=XXX&to=YYY&limit=10
        [HttpGet]
        public async Task<IActionResult> GetOffers([FromQuery] string from, [FromQuery] string to, [FromQuery] int limit = 10)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var offers = await _offerService.SearchOffersAsync(from, to, limit);
                sw.Stop();
                _logger.LogInformation("GET /offers executed in {ms} ms", sw.ElapsedMilliseconds);
                return Ok(offers);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "Error in GET /offers after {ms} ms", sw.ElapsedMilliseconds);
                return StatusCode(500, new { error = "Internal Server Error", duration_ms = sw.ElapsedMilliseconds });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfferById(string id)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = await _offerService.GetOfferDetailsAsync(id);
                if (result == null) return NotFound();

                sw.Stop();
                _logger.LogInformation("GET /offers/{id} completed in {ms}ms", id, sw.ElapsedMilliseconds);
                return Ok(result);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "GET /offers/{id} failed after {ms}ms", sw.ElapsedMilliseconds);
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateOffer([FromBody] Offer offer)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await _offerCreationService.CreateOfferAsync(offer);
                sw.Stop();
                _logger.LogInformation("POST /offers executed in {Ms} ms", sw.ElapsedMilliseconds);
                return CreatedAtAction(nameof(GetOfferById), new { id = offer.Id }, offer);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "Error in POST /offers after {Ms} ms", sw.ElapsedMilliseconds);
                return StatusCode(500, new { error = "Internal Server Error", duration_ms = sw.ElapsedMilliseconds });
            }
        }

    }
}
