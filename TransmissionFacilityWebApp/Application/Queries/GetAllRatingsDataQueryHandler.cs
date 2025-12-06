using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;
namespace TransmissionFacilityWebApp.Application.Queries;

public class GetAllRatingsDataQueryHandler : IRequestHandler<GetAllRatingsDataQuery, RatingsDataDto>
{
        private readonly ITransmissionFacilityRepository _transmissionFacilityRepository;
        public GetAllRatingsDataQueryHandler(ITransmissionFacilityRepository transmissionFacilityRepository)
        {
                _transmissionFacilityRepository = transmissionFacilityRepository;
        }
        public async Task<RatingsDataDto> Handle(GetAllRatingsDataQuery request, CancellationToken cancellationToken)
        {
                var ratingsData = await _transmissionFacilityRepository.GetAllRatingsDataAsync();
                return new RatingsData2Dto().ConvertToRatingsDataDto(ratingsData);
        }
}

