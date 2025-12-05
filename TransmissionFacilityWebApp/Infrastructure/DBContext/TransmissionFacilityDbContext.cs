using System;
using CimXml2Json;
using Microsoft.EntityFrameworkCore;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransmissionFacilityWebApp.Infrastructure.DBContext;

public class TransmissionFacilityDbContext : DbContext
{
        DataDBContext dataDBContext = new DataDBContext();

        public TransmissionFacilityDbContext()
        {
        }



        public async Task<RatingsData> GetTransmissionFacilityByIdAsync(string id)
        {
                if (dataDBContext.RatingsData == null)
                {
                        dataDBContext.ConvertXML2Json();
                        if (dataDBContext.RatingsData == null)
                        {
                                throw new InvalidOperationException("Ratings data could not be loaded.");
                        }
                }
                var facility = dataDBContext.RatingsData.transmissionFacilities.Where(f => f.id == id);
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
                if (dataDBContext.RatingsData == null)
                {
                        if (dataDBContext.RatingsData == null)
                        {
                                throw new InvalidOperationException("Ratings data could not be loaded.");
                        }
                }
                // Simulate async data retrieval with Task.FromResult
                return await Task.FromResult(dataDBContext.RatingsData);
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
