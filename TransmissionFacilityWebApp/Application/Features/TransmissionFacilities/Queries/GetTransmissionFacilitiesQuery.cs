using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;

public record GetTransmissionFacilitiesQuery() : IRequest<IEnumerable<TransmissionFacilityDto>>;
