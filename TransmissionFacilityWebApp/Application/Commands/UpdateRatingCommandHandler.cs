

using CimXml2Json;
using MediatR;
using TransmissionFacilityWebApp.Application.Commands;
using TransmissionFacilityWebApp.Core.Interfaces;

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, RatingsData>
{
    private readonly IRatingProposalRepository _ratingProposalRepository;

    public UpdateRatingCommandHandler(IRatingProposalRepository ratingProposalRepository)
    {
        _ratingProposalRepository = ratingProposalRepository;
    }

    public async Task<RatingsData> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        var updatedRating = await _ratingProposalRepository.UpdateRatingAsync(request.ratingsData, cancellationToken);
        return updatedRating;
    }
}