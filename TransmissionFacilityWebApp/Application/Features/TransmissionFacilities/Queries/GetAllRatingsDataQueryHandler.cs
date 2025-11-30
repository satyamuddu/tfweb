using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;
using TransmissionFacilityWebApp.Core.Interfaces;
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
       
        var ratingsDataDto = new RatingsDataDto(
                ratingsData.id,
                ratingsData.comment,
                ratingsData.transmissionFacilities.Select(tf => new TransmissionFacilityDto(
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
        return ratingsDataDto;
    }
}

