using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;

namespace TransmissionFacilityWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransmissionFacilityController : ControllerBase
{
    private readonly  IMediator _mediator;
    public TransmissionFacilityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<IActionResult> GetTransmissionFacilities()
    {
        var query = new GetTransmissionFacilitiesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
