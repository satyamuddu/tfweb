using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;

namespace TransmissionFacilityWebApp.Application.Queries;

public record GetTransmissionFacilitiesQuery() : IRequest<IEnumerable<TransmissionFacilityDto>>;

public record GetAllRatingsDataQuery() : IRequest<RatingsDataDto>;

public record GetTransmissionFacilityByIdQuery(string id) : IRequest<RatingsDataDto>;
