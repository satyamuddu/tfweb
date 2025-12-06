using System;
using CimXml2Json;

namespace TransmissionFacilityWebApp.Core.Interfaces;

public interface IRatingProposalRepository
{
  Task<RatingsData> GetRatingProposalByCOQueryAsync(string co);
  Task<RatingsData> GetRatingProposalByDateQueryAsync(DateTime fromDate, DateTime toDate);
}
