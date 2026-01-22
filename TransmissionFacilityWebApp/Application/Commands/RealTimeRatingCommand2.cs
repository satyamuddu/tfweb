using CimXml2Json;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;

namespace TransmissionFacilityWebApp.Application.Commands;

public record RealTimeRatingCommand2(RatingsData ratingsData) : IRequest<RatingsData>;