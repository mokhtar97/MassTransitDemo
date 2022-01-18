using MassTransit;
using MassTransit.Messages.Models.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterBranch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DecisionController : ControllerBase
    {
        private readonly ISendEndpointProvider senQueu;
        private readonly IPublishEndpoint _publishEndpoint;

        public DecisionController(ISendEndpointProvider senQueu, IPublishEndpoint publishEndpoint)
        {
            this.senQueu = senQueu;
            this._publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Decision Decision)
        {
            await _publishEndpoint.Publish<Decision>(Decision);
                
            return Ok();
        }

        [HttpPost("SendMessage")]
        public async Task<ActionResult> SendMessage([FromBody] Decision Decision)
        {
            try
            {
                var endpoint = await senQueu.GetSendEndpoint(new Uri("queue:TestSend"));
                await endpoint.Send<Decision>(Decision);
            }
            catch (Exception e)
            {
               //handel exception 
            }

            return Ok();
        }
    }
}
