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
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> GetAll()
        {

            var stocks = await _context.Stocks.ToListAsync();  //deferred execution (.ToList())

            var stockDto = stocks.Select(s => s.ToStockDto());   // .Select() is actually a MAP it's mapping data from model to stockDto

            return Ok(stocks);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id); // converting Find() to FindAsync() bcz we used AWAIT
            if (stock == null)
                return NotFound();

            //Mapping to stockDto before returning
            return Ok(stock.ToStockDto());

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {

            //Mapping this Dto to Model
            var stockModel = stockDto.ToStockFromCreateDto();

            //These two lines are must to add and save data in database
            await _context.Stocks.AddAsync(stockModel); //Adding await and Async  
            await _context.SaveChangesAsync();

            //to send the object created as response we're calling stock from id using the GetById functino described above in this controller and providing id, while getting the stock.toStockDto.....
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)  //since we'r gettin id from route and dto from body or request
        {

            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
                return NotFound();

            stockModel.Symbol = updateDto.Symbol;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Industry = updateDto.Industry;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();  //saving changes in database

            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

            if (stockModel == null)
                return NotFound();

            _context.Stocks.Remove(stockModel);  //remove can't be async
            await _context.SaveChangesAsync();

            return NoContent();  //used for successful deletion
        }
    }
}