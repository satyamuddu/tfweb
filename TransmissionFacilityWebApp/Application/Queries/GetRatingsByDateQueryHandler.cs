using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;
namespace TransmissionFacilityWebApp.Application.Queries;

public class GetRatingsByDateQueryHandler : IRequestHandler<GetRatingsByDateQuery, RatingsDataDto>
{
    private readonly IRatingProposalRepository _ratingsProposalRepository;
    public GetRatingsByDateQueryHandler(IRatingProposalRepository ratingProposalRepository)
    {
        _ratingsProposalRepository = ratingProposalRepository;
    }
    public async Task<RatingsDataDto> Handle(GetRatingsByDateQuery request, CancellationToken cancellationToken)
    {
        var ratingsData = await _ratingsProposalRepository.GetRatingProposalByDateQueryAsync(request.fromDate, request.toDate);

        return new RatingsData2Dto().ConvertToRatingsDataDto(ratingsData);
    }
}
