using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TravelPlanner.Domain.Interfaces
{
    public interface ICityGraphRepository
    {
        Task<List<string>> GetRelatedOffersAsync(string cityCode, DateTime departDate, string excludeOfferId, int k = 3);
    }
}
