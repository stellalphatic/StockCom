using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api1.Data;
using api1.Dtos.Stock;
using api1.Mappers;
using api1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api1.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public StockController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            var stocks = _context.Stocks.ToList().Select(s => s.ToStockDto());  //deferred execution (.ToList())
                                                                                // .Select() is actually a MAP it's mapping data from model to stockDto
            return Ok(stocks);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);
            if (stock == null)
                return NotFound();

            //Mapping to stockDto before returning
            return Ok(stock.ToStockDto());

        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {

            //Mapping this Dto to Model
            var stockModel = stockDto.ToStockFromCreateDto();

            //These two lines are must to add and save data in database
            _context.Stocks.Add(stockModel);
            _context.SaveChanges();

            //to send the object created as response we're calling stock from id using the GetById functino described above in this controller and providing id, while getting the stock.toStockDto.....
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)  //since we'r gettin id from route and dto from body or request
        {

            var stockModel = _context.Stocks.FirstOrDefault(x => x.Id == id);
            if (stockModel == null)
                return NotFound();

            stockModel.Symbol = updateDto.Symbol;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Industry = updateDto.Industry;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();  //saving changes in database

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {

            var stockModel = _context.Stocks.FirstOrDefault(s => s.Id == id);

            if (stockModel == null)
                return NotFound();

            _context.Stocks.Remove(stockModel);
            _context.SaveChanges();

           return NoContent();  //used for successful deletion
        }
    }
}