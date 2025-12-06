using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;


public record GetRatingsByCOQuery(string co) : IRequest<RatingsDataDto>;
public record GetRatingsByDateQuery(DateTime fromDate, DateTime toDate) : IRequest<RatingsDataDto>;

