using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransmissionFacilityWebApp.Application.Dto;

public record ValueDto(string name, string value, string unit);
public record PeriodDto(DateTime start, string duration);
public record MetadataDto(string sourceSystem, string calculationMethod);
public record RatingDto(string rating_type, PeriodDto period, List<ValueDto> values, MetadataDto metadata);
public record SegmentDto(string id, List<RatingDto> ratings);
public record TransmissionFacilityDto(string id, List<SegmentDto> segments);
public record RatingsDataDto(string id, string comment, List<TransmissionFacilityDto> transmissionFacilities);

