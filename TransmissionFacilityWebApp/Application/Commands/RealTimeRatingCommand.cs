using CimXml2Json;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;

namespace TransmissionFacilityWebApp.Application.Commands;

public record RealTimeRatingCommand(string co, RatingsDataDto ratingsDataDto) : IRequest<RatingsData>;