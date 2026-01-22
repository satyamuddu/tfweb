using CimXml2Json;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;
using TransmissionFacilityWebApp.Application.Queries;
using TransmissionFacilityWebApp.Core.Interfaces;

namespace TransmissionFacilityWebApp.Application.Commands;

public class RealTimeRatingCommandHandler2 : IRequestHandler<RealTimeRatingCommand2, RatingsData>
{
    private readonly IRatingProposalRepository _ratingProposalRepository;

    public RealTimeRatingCommandHandler2(IRatingProposalRepository ratingProposalRepository)
    {
        _ratingProposalRepository = ratingProposalRepository;
    }

    public async Task<RatingsData> Handle(RealTimeRatingCommand2 request, CancellationToken cancellationToken)
    {
        var newRatingCreated = await _ratingProposalRepository.AddRealTimeRatingAsync(request.ratingsData, cancellationToken);
        return newRatingCreated;
    }
}