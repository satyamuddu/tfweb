using System;
using CimXml2Json;
using Microsoft.EntityFrameworkCore;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;

namespace TransmissionFacilityWebApp.Infrastructure.Persistence;

public class TransmissionFacilityDbContext : DbContext
{
        CimXml2Json.RatingsData? ratingsData;

        public TransmissionFacilityDbContext()
        {
        }

        private void ConvertXML2Json()
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
        }
        public async Task<RatingsData> GetTransmissionFacilityByIdAsync(string id)
        {
                if (ratingsData == null)
                {
                        ConvertXML2Json();
                        if (ratingsData == null)
                        {
                                throw new InvalidOperationException("Ratings data could not be loaded.");
                        }
                }
                var facility = ratingsData.transmissionFacilities.FirstOrDefault(f => f.id == id);
                if (facility == null)
                {
                        throw new KeyNotFoundException($"Transmission facility with ID {id} not found.");
                }
                return await Task.FromResult(new RatingsData
                {
                        transmissionFacilities = new List<TransmissionFacilities> { facility }
                });
        }
        
        public async Task<RatingsData> GetAllRatingsDataAsync()
        {
                if (ratingsData == null)
                {
                        ConvertXML2Json();
                        if (ratingsData == null)
                        {
                                throw new InvalidOperationException("Ratings data could not be loaded.");
                        }
                }
                // Simulate async data retrieval with Task.FromResult
                return await Task.FromResult(ratingsData);
        }
        public async Task<IEnumerable<TransmissionFacilities>> GetTransmissionFacilitiesAsync()
        {
                var transmissionFacilities = new List<TransmissionFacilities>()
                {
                    new TransmissionFacilities()
                    {
                        id = "1", segments = new List<Segment>()
                        {
                                new Segment()
                                {
                                        id = "S1", ratings = new List<Rating>()
                                        {
                                                new Rating()
                                                {
                                                        rating_type = "Type1",
                                                        period = new Period(){ start = "2023-01-01T00:00:00Z", duration = "PT1H" },
                                                        values = new List<Value>()
                                                        {
                                                                new Value(){ name = "MaxCapacity", value = "100", unit = "MW" },
                                                                new Value(){ name = "MinCapacity", value = "10", unit = "MW" }
                                                        },
                                                        metadata = new Metadata(){ sourceSystem = "SystemA", calculationMethod = "Method1" }
                                                }
                                        }
                                }
                        },
                    }
                };
                // Simulate async data retrieval with Task.FromResult
                return await Task.FromResult(transmissionFacilities);
        }
}
