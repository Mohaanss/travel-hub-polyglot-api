using TravelPlanner.Domain.DTOs;
using TravelPlanner.Domain.Models;
using System.Collections.Generic;

namespace TravelPlanner.Domain.DTOs;
public class OfferDetailsResponse
{
    public Offer Offer { get; set; } = default!;
    public List<string> RelatedOffers { get; set; } = new();
}
