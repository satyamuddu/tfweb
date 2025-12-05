using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;
using static System.Net.Mime.MediaTypeNames;
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

        var ratingsDataDto = new RatingsDataDto(
                            ratingsData.id,
                            ratingsData.comment,
                            ratingsData.transmissionFacilities.Where(tf => tf.id.StartsWith(request.co)).Select(tf => new TransmissionFacilityDto(
                                    tf.id,
                                    tf.segments.Select(seg => new SegmentDto(
                                            seg.id,
                                            seg.ratings.Select(rat => new RatingDto(
                                                    rat.rating_type,
                                                    new PeriodDto(rat.period.start, rat.period.duration),
                                                    rat.values.Select(val => new ValueDto(val.name, val.value, val.unit)).ToList(),
                                                    new MetadataDto(rat.metadata.sourceSystem, rat.metadata.calculationMethod)
                                            )).ToList()
                                    )).ToList()
                            )).ToList()
                    );
        return (ratingsDataDto);
    }
}
