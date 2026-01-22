using CimXml2Json;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;
using TransmissionFacilityWebApp.Application.Queries;
using TransmissionFacilityWebApp.Core.Interfaces;

namespace TransmissionFacilityWebApp.Application.Commands;

public class RealTimeRatingCommandHandler : IRequestHandler<RealTimeRatingCommand, RatingsData>
{
    private readonly IRatingProposalRepository _ratingProposalRepository;

    public RealTimeRatingCommandHandler(IRatingProposalRepository ratingProposalRepository)
    {
        _ratingProposalRepository = ratingProposalRepository;
    }

    public async Task<RatingsData> Handle(RealTimeRatingCommand request, CancellationToken cancellationToken)
    {
        var newRating = new RatingsDto2Data().ConvertRatingsDto2Data(request.ratingsDataDto);
        var newRatingCreated = await _ratingProposalRepository.AddRealTimeRatingAsync(request.co, newRating);
        return newRatingCreated;

    }
}