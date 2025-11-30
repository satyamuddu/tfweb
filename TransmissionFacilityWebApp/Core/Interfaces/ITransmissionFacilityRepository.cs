using System;
using CimXml2Json;

namespace TransmissionFacilityWebApp.Core.Interfaces;

public interface ITransmissionFacilityRepository
{
    Task<IEnumerable<TransmissionFacilities>> GetTransmissionFacilitiesAsync();
    Task<RatingsData> GetAllRatingsDataAsync();
    Task<RatingsData> GetTransmissionFacilityByIdQueryAsync(string id);
}
