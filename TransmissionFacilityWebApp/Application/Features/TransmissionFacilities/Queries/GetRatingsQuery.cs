using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;


public record GetRatingsByCOQuery(string co) : IRequest<RatingsDataDto>;

