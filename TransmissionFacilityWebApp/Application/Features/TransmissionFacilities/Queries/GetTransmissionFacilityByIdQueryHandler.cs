using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using TransmissionFacilityWebApp.Core.Interfaces;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries
{
    public class GetTransmissionFacilityByIdQueryHandler : IRequestHandler<GetTransmissionFacilityByIdQuery, RatingsDataDto>
    {
        private readonly ITransmissionFacilityRepository _transmissionFacilityRepository;
        public GetTransmissionFacilityByIdQueryHandler(ITransmissionFacilityRepository transmissionFacilityRepository)
        {
            _transmissionFacilityRepository = transmissionFacilityRepository;
        }

        public async Task<RatingsDataDto> Handle(GetTransmissionFacilityByIdQuery request, CancellationToken cancellationToken)
        {
            // TODO: Implement retrieval logic and map to RatingsDataDto.
            // Return a default value for now to satisfy the compiler.
            var ratingsData = await _transmissionFacilityRepository.GetTransmissionFacilityByIdQueryAsync(request.id);

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
            return (ratingsDataDto);
        }
    }
}