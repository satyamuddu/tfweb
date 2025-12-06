using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CimXml2Json;

namespace TransmissionFacilityWebApp.Infrastructure.DBContext;


public class RatingProposalDbContext : DbContext
{
    DataDBContext dataDBContext = new DataDBContext();
    public async Task<RatingsData> GetRatingProposalByCOAsync(string co)
    {
        if (dataDBContext.RatingsData == null)
        {
            dataDBContext.ConvertXML2Json();
            if (dataDBContext.RatingsData == null)
            {
                throw new InvalidOperationException("Ratings data could not be loaded.");
            }
        }

        var facility = dataDBContext.RatingsData.transmissionFacilities;
        if (facility == null)
        {
            throw new KeyNotFoundException($"Transmission facility with ID {co} not found.");
        }
        return await Task.FromResult(new RatingsData
        {
            id = "APP-RTR20251130-001",
            comment = "Forecasted Ambient Adjusted Ratings for transmission line 12345 (segment A-B)",
            transmissionFacilities = new List<TransmissionFacilities>(facility)
        });
    }
    public async Task<RatingsData> GetRatingProposalByDateAsync(DateTime fromDate, DateTime toDate)
    {
        if (dataDBContext.RatingsData == null)
        {
            dataDBContext.ConvertXML2Json();
            if (dataDBContext.RatingsData == null)
            {
                throw new InvalidOperationException("Ratings data could not be loaded.");
            }
        }

        var facility = dataDBContext.RatingsData.transmissionFacilities;
        if (facility == null)
        {
            throw new KeyNotFoundException($"No transmission facilities found for the given date range.");
        }
        return await Task.FromResult(new RatingsData
        {
            id = "APP-RTR20251130-001",
            comment = "Forecasted Ambient Adjusted Ratings for transmission line 12345 (segment A-B)",
            transmissionFacilities = new List<TransmissionFacilities>(facility)
        });
    }
}
