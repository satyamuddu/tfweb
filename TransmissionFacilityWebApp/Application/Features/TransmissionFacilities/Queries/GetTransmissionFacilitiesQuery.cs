using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;

public record GetTransmissionFacilitiesQuery() : IRequest<IEnumerable<TransmissionFacilityDto>>;

public record GetAllRatingsDataQuery() : IRequest<RatingsDataDto>;

public record GetTransmissionFacilityByIdQuery  (string id) : IRequest<RatingsDataDto>;
