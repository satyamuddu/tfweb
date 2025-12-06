using System;
using CimXml2Json;
using Microsoft.EntityFrameworkCore;
using TransmissionFacilityWebApp.Application.Dto;

namespace TransmissionFacilityWebApp.DBContext;

public class TransmissionFacilityDbContext : DbContext
{
        DataDBContext dataDBContext = new DataDBContext();
        public TransmissionFacilityDbContext()
        {
        }
        public async Task<RatingsData> GetTransmissionFacilityByIdAsync(string id)
        {
                var data = dataDBContext.GetByCo(id);
                var facility = data.Result.transmissionFacilities.Where(f => f.id == id);
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
                return await dataDBContext.GetByCo("ECAR");
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
