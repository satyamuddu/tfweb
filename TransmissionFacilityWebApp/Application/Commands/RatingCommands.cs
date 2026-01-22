using CimXml2Json;
using MediatR;
using TransmissionFacilityWebApp.Application.Dto;

namespace TransmissionFacilityWebApp.Application.Commands;

public record UpdateRatingCommand(RatingsData ratingsData) : IRequest<RatingsData>;