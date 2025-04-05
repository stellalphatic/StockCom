using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api1.Dtos.Stock;
using api1.Models;

namespace api1.Mappers
{
    //SO Mapper will contain functions to convert stockDto to Stock model and viceversa 
    public static class StockMappers
    {
        // Here You can easily trim down and remove data from model, which you don't want to send/receive 
        public static StockDto ToStockDto(this Stock stockModel)
        {
            return new StockDto
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
               Comments = stockModel.Comments.Select(c => c.ToCommentDto()).ToList()
                // Here comments are removed bcz i don't want to send comment to client
            };
        }
        // Now when user gives request we'll store in CreateRequestDto and now Mapping it into MODEL
        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto)
        {
            return new Stock
            {
                Symbol = stockDto.Symbol,
                CompanyName = stockDto.CompanyName,
                Purchase = stockDto.Purchase,
                LastDiv = stockDto.LastDiv,
                Industry = stockDto.Industry,
                MarketCap = stockDto.MarketCap

            };
        }
    }
}