using System;
using CimXml2Json;
using Microsoft.EntityFrameworkCore;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                DuplicateData(60);
        }

        private void DuplicateData(int ntimes)
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
                var facility = ratingsData.transmissionFacilities.Where(f => f.id == id);
                if (facility == null)
                {
                        throw new KeyNotFoundException($"Transmission facility with ID {id} not found.");
                }
                return await Task.FromResult(new RatingsData
                {
                        id = "APP-RTR20251130-001",
                        comment = "Forecasted Ambient Adjusted Ratings for transmission line 12345 (segment A-B)",
                        transmissionFacilities = new List<TransmissionFacilities>(facility)
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
