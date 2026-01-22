using System;
using CimXml2Json;

namespace TransmissionFacilityWebApp.Core.Interfaces;

public interface IRatingProposalRepository
{
  Task<RatingsData> GetRatingProposalByCOQueryAsync(string co);
  Task<RatingsData> GetRatingProposalByDateQueryAsync(string co, DateTime fromDate, DateTime toDate);

  Task<RatingsData> AddRealTimeRatingAsync(string co, RatingsData newRating);
  Task<RatingsData> AddRealTimeRatingAsync(RatingsData newRating, CancellationToken cancellationToken);
  Task<RatingsData> UpdateRatingAsync(RatingsData ratingsData, CancellationToken cancellationToken);
}
