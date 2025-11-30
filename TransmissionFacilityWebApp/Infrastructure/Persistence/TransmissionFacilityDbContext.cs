using System;
using CimXml2Json;
using Microsoft.EntityFrameworkCore;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;

namespace TransmissionFacilityWebApp.Infrastructure.Persistence;

public class TransmissionFacilityDbContext : DbContext
{

        public TransmissionFacilityDbContext()
        {
        }
        public async Task<IEnumerable<TransmissionFacilities>> GetAllTransmissionFacilitiesAsync()
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
