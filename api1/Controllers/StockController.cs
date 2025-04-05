using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api1.Data;
using api1.Dtos.Stock;
using api1.Helpers;
using api1.Mappers;
using api1.Models;
using api1.Repository;
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
        private readonly StockRepository _stockRepo;
        public StockController(ApplicationDbContext context, StockRepository stockRepo)
        {
            _context = context;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) // url/api/stock?query
        {

            var stocks = await _stockRepo.GetAllStock(query);

            var stockDto = stocks.Select(s => s.ToStockDto());   // .Select() is actually a MAP it's mapping data from model to stockDto

            return Ok(stockDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetById(id);
            if (stock == null)
                return NotFound();

            //Mapping to stockDto before returning
            return Ok(stock.ToStockDto());

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {


            var stockModel = await _stockRepo.Create(stockDto);

            //to send the object created as response we're calling stock from id using the GetById functino described above in this controller and providing id, while getting the stock.toStockDto.....
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());

        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)  //since we'r gettin id from route and dto from body or request
        {

            var stockModel = await _stockRepo.Update(id, updateDto);
            if (stockModel == null)
                return NotFound();


            return Ok(stockModel.ToStockDto());

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var stockModel = await _stockRepo.Delete(id);

            if (stockModel == null)
                return NotFound();


            return NoContent();  //used for successful deletion
        }
    }
}