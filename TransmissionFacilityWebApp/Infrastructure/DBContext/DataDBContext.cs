using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CimXml2Json;

namespace TransmissionFacilityWebApp.Infrastructure.DBContext
{
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
                transformCim2Json.ToJson();
                string outputPath = Path.Combine(Environment.CurrentDirectory, "output", "ratings.json");
                transformCim2Json.WriteToFile(outputPath);
                ratingsData = transformCim2Json.GetRatingsData();
            }
            else
            {
                Console.WriteLine($"File not found: {filePath}");
            }
            //DuplicateData(60);
        }

        public void DuplicateData(int ntimes)
        {
            if (ratingsData != null)
            {
                var originalFacilities = new List<TransmissionFacilities>(ratingsData.transmissionFacilities);
                for (int i = 1; i < ntimes; i++)
                {
                    foreach (var facility in originalFacilities)
                    {
                        var newFacility = new TransmissionFacilities
                        {
                            id = facility.id,
                            segments = new List<Segment>()
                        };
                        foreach (var segment in facility.segments)
                        {
                            newFacility.segments.Add(new Segment
                            {
                                id = segment.id,
                                ratings = segment.ratings.Select(rating => new Rating
                                {
                                    rating_type = rating.rating_type,
                                    period = new Period
                                    {
                                        start = DateTime.Parse(rating.period.start).AddHours(i * originalFacilities.Count).ToString("o"),
                                        duration = rating.period.duration
                                    },
                                    values = rating.values.Select(value => new Value
                                    {
                                        name = value.name,
                                        value = value.value,
                                        unit = value.unit
                                    }).ToList(),
                                    metadata = new Metadata
                                    {
                                        sourceSystem = rating.metadata.sourceSystem,
                                        calculationMethod = rating.metadata.calculationMethod
                                    }
                                }).ToList()
                            });
                        }
                        ratingsData.transmissionFacilities.Add(newFacility);
                    }
                }
            }
        }

    }
}