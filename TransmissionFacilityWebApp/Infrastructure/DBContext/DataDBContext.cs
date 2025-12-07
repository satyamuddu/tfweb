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
}
