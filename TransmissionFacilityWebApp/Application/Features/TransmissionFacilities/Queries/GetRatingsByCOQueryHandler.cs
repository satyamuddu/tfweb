using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using CimXml2Json;
namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;

public class GetRatingsByCOQueryHandler : IRequestHandler<GetRatingsByCOQuery, RatingsDataDto>
{
        private readonly IRatingProposalRepository _ratingsProposalRepository;
        public GetRatingsByCOQueryHandler(IRatingProposalRepository ratingsProposalRepository)
        {
                _ratingsProposalRepository = ratingsProposalRepository;
        }
        public async Task<RatingsDataDto> Handle(GetRatingsByCOQuery request, CancellationToken cancellationToken)
        {
                var ratingsData = await _ratingsProposalRepository.GetRatingProposalByCOQueryAsync(request.co);

                return new RatingsData2Dto().ConvertToRatingsDataDto(ratingsData);
        }
}
