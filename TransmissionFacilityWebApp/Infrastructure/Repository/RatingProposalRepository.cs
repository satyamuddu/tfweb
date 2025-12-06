using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CimXml2Json;
using TransmissionFacilityWebApp.Core.Interfaces;
using TransmissionFacilityWebApp.DBContext;

namespace TransmissionFacilityWebApp.Repository;

public class RatingProposalRepository : IRatingProposalRepository
{
    readonly RatingProposalDbContext _dbContext;
    public RatingProposalRepository(RatingProposalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RatingsData> GetRatingProposalByCOQueryAsync(string co) => await _dbContext.GetRatingProposalByCOAsync(co);

    public async Task<RatingsData> GetRatingProposalByDateQueryAsync(DateTime fromDate, DateTime toDate) => await _dbContext.GetRatingProposalByDateAsync(fromDate, toDate);

}
