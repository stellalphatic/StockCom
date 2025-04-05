using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using api1.Data;
using api1.Dtos.Stock;
using api1.Helpers;
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

        public async Task<List<Stock>> GetAllStock(QueryObject query)
        {

            // var stocks = await _context.Stocks.ToListAsync();   //deferred execution (.ToList())

            // return await _context.Stocks.Include(c => c.Comments).ToListAsync(); 

            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {

                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {

                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                //for url/api/stock?sortby=symbol&isdescending=true/false
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.isDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }

                if (query.SortBy.Equals("Purchase", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.isDescending ? stocks.OrderByDescending(s => s.Purchase) : stocks.OrderBy(s => s.Purchase);
                }
            }


            return await stocks.ToListAsync(); // when we we write tolist then our queries are acually give us data otherwise we are actually playing with just queries
        }
        public async Task<Stock?> GetById(int id)
        {

            // var stock = await _context.Stocks.FindAsync(id);
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
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

        public Task<bool> isStockExist(int id)
        {
            // var stock =await _context.Stocks.FindAsync(id);
            // if( stock== null)
            //  return false;
            // return true;

            return _context.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}