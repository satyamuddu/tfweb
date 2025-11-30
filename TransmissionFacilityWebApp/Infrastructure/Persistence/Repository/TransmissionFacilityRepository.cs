using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;
using CimXml2Json;

namespace TransmissionFacilityWebApp.Infrastructure.Persistence.Repository
{
    public class TransmissionFacilityRepository : ITransmissionFacilityRepository
    {
        readonly TransmissionFacilityDbContext _dbContext;
        public TransmissionFacilityRepository(TransmissionFacilityDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<IEnumerable<TransmissionFacilities>> GetTransmissionFacilitiesAsync() => await _dbContext.GetTransmissionFacilitiesAsync();
        
        public async Task<RatingsData> GetAllRatingsDataAsync() => await _dbContext.GetAllRatingsDataAsync();
    }
}