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
        // GET /api/ratingproposal/co/{co}
        [HttpGet("co/{co}")]
        public async Task<IActionResult> GetRatingsByCOQuery(string co)
        {
            var query = new Application.Queries.GetRatingsByCOQuery(co);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        // GET /api/ratingproposal/bydate?co=NEEPOOL&fromDate=yyyy-MM-dd&toDate=yyyy-MM-dd
        [HttpGet("bydate")]
        public async Task<IActionResult> GetRatingsByDateQuery([FromQuery] string co, [FromQuery] DateTime fromDate, [FromQuery] DateTime toDate)
        {
            // Validate


            if (toDate < fromDate)
                return BadRequest("toDate must be greater than fromDate");

            var query = new Application.Queries.GetRatingsByDateQuery(co, fromDate, toDate);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        /*[HttpPost("newrealtime")]
        public async Task<ActionResult> NewRealTime([FromBody] Application.Commands.RealTimeRatingCommand command)
        {
            try
            {
                var newRating = await _mediator.Send(command);
                return CreatedAtAction("NewRealTime", new { id = newRating.id }, newRating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }*/
        [HttpPost("newrealtime")]
        public async Task<ActionResult> NewRealTime2([FromBody] Application.Commands.RealTimeRatingCommand2 command)
        {
            try
            {
                var newRating = await _mediator.Send(command);
                return CreatedAtAction("NewRealTime2", new { id = newRating.id }, newRating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("updaterating")]
        public async Task<IActionResult> UpdateRating([FromBody] Application.Commands.UpdateRatingCommand command)
        {

            try
            {
                var updatedRating = await _mediator.Send(command);
                return Ok(updatedRating);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}