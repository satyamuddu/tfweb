using System;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TransmissionFacilityWebApp.Application.Queries;

public record GetRatingsByCOQuery(string co) : IRequest<RatingsDataDto>;
public record GetRatingsByDateQuery(DateTime fromDate, DateTime toDate) : IRequest<RatingsDataDto>;

