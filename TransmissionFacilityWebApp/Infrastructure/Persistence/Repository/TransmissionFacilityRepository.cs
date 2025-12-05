using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;
using CimXml2Json;
using TransmissionFacilityWebApp.Infrastructure.DBContext;

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
        public async Task<RatingsData> GetTransmissionFacilityByIdQueryAsync(string id) => await _dbContext.GetTransmissionFacilityByIdAsync(id);
    }
}