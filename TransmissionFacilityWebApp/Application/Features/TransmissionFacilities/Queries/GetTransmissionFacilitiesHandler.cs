using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;

public class GetTransmissionFacilitiesHandler : IRequestHandler<GetTransmissionFacilitiesQuery, IEnumerable<TransmissionFacilityDto>>
{
    private readonly ITransmissionFacilityRepository _transmissionFacilityRepository;
    public GetTransmissionFacilitiesHandler(ITransmissionFacilityRepository transmissionFacilityRepository)
    {
        _transmissionFacilityRepository = transmissionFacilityRepository;
    }
    public async Task<IEnumerable<TransmissionFacilityDto>> Handle(GetTransmissionFacilitiesQuery request, CancellationToken cancellationToken)
    {
        var transmissionFacilities = await _transmissionFacilityRepository.GetTransmissionFacilitiesAsync();
        var transmissionFacilityDtos = transmissionFacilities.Select(tf => new TransmissionFacilityDto(
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
        ));
        return (transmissionFacilityDtos);
    }
   
}
