using AutoMapper;
using MassTransit;
using MassTransit.Messages.Models.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderContext orderContext;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper mapper;

        public OrderController(OrderContext _orderContext, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpoint,IMapper mapper)
        {
            orderContext = _orderContext;
            _publishEndpoint = publishEndpoint;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(orderContext.orders.ToList());
        }

        [HttpPost]
        public  ActionResult Post([FromBody] Orders order)

        {
           
           
            var entity= orderContext.orders.Add(new Orders() { Name = order.Name, Amount = order.Amount });
             orderContext.SaveChanges();
            
            _publishEndpoint.Publish<OrderCreatedEvent>(mapper.Map<OrderCreatedEvent>(entity.Entity));
            return Ok(order.Amount);
        }

    }
}

