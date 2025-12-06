using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TransmissionFacilityWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingProposalController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RatingProposalController(IMediator mediator) => _mediator = mediator;
        // GET /api/ratingproposal/{co}
        [HttpGet("{co}")]
        public async Task<IActionResult> GetRatingsByCOQuery(string co)
        {
            var query = new Application.Features.TransmissionFacilities.Queries.GetRatingsByCOQuery(co);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("{co}/{bydate}")]
        public async Task<IActionResult> GetRatingsByDateQuery([FromQuery] string co, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            var query = new Application.Features.TransmissionFacilities.Queries.GetRatingsByDateQuery(fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

    }
}