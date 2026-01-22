using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CimXml2Json;

namespace TransmissionFacilityWebApp.DBContext;


public class RatingProposalDbContext : DbContext
{
    DataDBContext dataDBContext = new DataDBContext();
    public async Task<RatingsData> GetRatingProposalByCOAsync(string co)
    {
        return await dataDBContext.GetByCo(co);
    }
    public async Task<RatingsData> GetRatingProposalByDateAsync(string co, DateTime fromDate, DateTime toDate)
    {
        return await dataDBContext.GetByCo(co, fromDate, toDate);
    }
    public async Task<RatingsData> AddRealTimeRatingAsync(string co, RatingsData newRating)
    {
        return await dataDBContext.AddRealTimeRating(co, newRating);
    }
    public async Task<RatingsData> AddRealTimeRatingAsync(RatingsData newRating, CancellationToken cancellationToken)
    {
        return await dataDBContext.AddRealTimeRating(newRating, cancellationToken);
    }

    internal async Task<RatingsData> UpdateRatingAsync(RatingsData ratingsData, CancellationToken cancellationToken)
    => await dataDBContext.UpdateRating(ratingsData, cancellationToken);
}
