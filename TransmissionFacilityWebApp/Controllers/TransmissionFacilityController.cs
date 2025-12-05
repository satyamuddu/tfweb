using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransmissionFacilityWebApp.Application.Features.TransmissionFacilities.Queries;

namespace TransmissionFacilityWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransmissionFacilityController : ControllerBase
{
    private readonly IMediator _mediator;
    public TransmissionFacilityController(IMediator mediator) => _mediator = mediator;
    // GET /api/transmissionfacility
    [HttpGet]
    public async Task<IActionResult> GetTransmissionFacilities()
    {
        var query = new GetTransmissionFacilitiesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    // GET /api/transmissionfacility/all
    [HttpGet("all")]
    public async Task<IActionResult> GetAllRatingsDataQuery()
    {
        var query = new GetAllRatingsDataQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
    // GET /api/transmissionfacility/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransmissionFacilityById(string id)
    {
        var query = new GetTransmissionFacilityByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
