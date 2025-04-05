using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api1.Data;
using api1.Dtos.Stock;
using api1.Mappers;
using api1.Models;
using Microsoft.EntityFrameworkCore;

namespace api1.Repository
{
    public class StockRepository
    {
        public readonly ApplicationDbContext _context;
        public StockRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStock()
        {

            var stocks = await _context.Stocks.ToListAsync();   //deferred execution (.ToList())

            return stocks;
        }
        public async Task<Stock?> GetById(int id)
        {

            var stock = await _context.Stocks.FindAsync(id);
            return stock;
        }

        public async Task<Stock> Create(CreateStockRequestDto stockDto)
        {

            //Mapping this Dto to Model
            var stockModel = stockDto.ToStockFromCreateDto();

            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();

            return stockModel;

        }

        public async Task<Stock?> Update(int id, UpdateStockRequestDto updateDto)
        {

            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stockModel == null)
                return null;

            stockModel.Symbol = updateDto.Symbol;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Industry = updateDto.Industry;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();  //saving changes in database

            return stockModel;
        }

        public async Task<Stock?> Delete(int id)
        {

            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if (stock == null)
                return null;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return stock;
        }
    }
}