using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CimXml2Json;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries
{
        public class RatingsData2Dto
        {
                public RatingsDataDto ConvertToRatingsDataDto(RatingsData ratingsData) => new RatingsDataDto(
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
        };
}
