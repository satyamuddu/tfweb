using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CimXml2Json;

namespace TransmissionFacilityWebApp.DBContext;

public class DataDBContext
{
    CimXml2Json.RatingsData? ratingsData;
    public RatingsData? RatingsData => ratingsData;
    public void ConvertXML2Json()
    {
        string file = "esca60_gev_original.xml";

        string filePath = Path.Combine(Environment.CurrentDirectory, "data", file);
        if (File.Exists(filePath))
        {
            CimXml2Json.TransformCim2Json transformCim2Json = new CimXml2Json.TransformCim2Json();
            transformCim2Json.Parse(filePath);

            ratingsData = transformCim2Json.GetRatingsData();
        }
        else
        {
            Console.WriteLine($"File not found: {filePath}");
        }

    }
}
