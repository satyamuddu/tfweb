using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CimXml2Json;
using TransmissionFacilityWebApp.Application.Dto;

namespace TransmissionFacilityWebApp.Application.Queries;

public class RatingsDto2Data
{



public RatingsData ConvertRatingsDto2Data(RatingsDataDto ratingDataDto) => new RatingsData(){
        
        id = ratingDataDto.id,
        comment = ratingDataDto.comment,
        transmissionFacilities = ratingDataDto.transmissionFacilities.Select(tfDto => new TransmissionFacilities(){
                id = tfDto.id,
                segments = tfDto.segments.Select(segDto => new Segment(){
                        id = segDto.id,
                        ratings = segDto.ratings.Select(ratDto => new Rating(){
                                rating_type = ratDto.rating_type,
                                period = new Period(){
                                        start = ratDto.period.start,
                                        duration = ratDto.period.duration
                                },
                                values = ratDto.values.Select(valDto => new Value(){
                                        name = valDto.name,
                                        value = valDto.value,
                                        unit = valDto.unit
                                }).ToList(),
                                metadata = new Metadata(){
                                        sourceSystem = ratDto.metadata.sourceSystem,
                                        calculationMethod = ratDto.metadata.calculationMethod
                                }
                        }).ToList()
                }).ToList()
        }).ToList()             



};



}
