using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly StockContext stockContext;

        public StocksController(StockContext _stockContext)
        {
            stockContext = _stockContext;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(stockContext.stocks.ToList());
        }

        [HttpPost]
        public ActionResult Post([FromBody] Stocks stock)
       
            {
             stockContext.stocks.Add(new Stocks() { Amount = stock.Amount });
            stockContext.SaveChanges();
            return Ok(stock.Amount);
            }

        }
    }
