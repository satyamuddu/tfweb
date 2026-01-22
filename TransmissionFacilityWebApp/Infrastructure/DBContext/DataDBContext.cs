using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CimXml2Json;

namespace TransmissionFacilityWebApp.DBContext;

public class DataDBContext
{
    public Task<RatingsData> GetByCo(string coId)
    {
        CimXml2Json.JsonFileReader jsonFileReader = new CimXml2Json.JsonFileReader();
        return Task.FromResult(jsonFileReader.GetByCo(coId));
    }

    public Task<RatingsData> GetByCo(string coId, DateTime fromDate, DateTime toDate)
    {
        CimXml2Json.JsonFileReader jsonFileReader = new CimXml2Json.JsonFileReader();
        return Task.FromResult(jsonFileReader.GetByCo(coId, fromDate, toDate));
    }
    public Task<RatingsData> AddRealTimeRating(string coId, RatingsData newRating)
    {
        CimXml2Json.JsonFileWriter jsonFileWriter = new CimXml2Json.JsonFileWriter();
        return Task.FromResult(jsonFileWriter.AddRealTimeRating(newRating, new CancellationToken()));
    }
    public Task<RatingsData> AddRealTimeRating(RatingsData newRating, CancellationToken cancellationToken)
    {
        CimXml2Json.JsonFileWriter jsonFileWriter = new CimXml2Json.JsonFileWriter();
        return Task.FromResult(jsonFileWriter.AddRealTimeRating(newRating, cancellationToken));
    }

    internal async Task<RatingsData> UpdateRating(RatingsData ratingsData, CancellationToken cancellationToken)
    {
        CimXml2Json.JsonFileUpdater jsonFileUpdater = new CimXml2Json.JsonFileUpdater();
        return jsonFileUpdater.UpdateRating(ratingsData, cancellationToken);
    }
}
