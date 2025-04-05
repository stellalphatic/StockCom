using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api1.Dtos.Stock
{
    //THis is request DTO meaning client will send these data bcz we don't want user to give ID
    public class CreateStockRequestDto
    {
        // we don't want ID from user in POST request
        public string Symbol { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public decimal Purchase { get; set; }
        public decimal LastDiv { get; set; }
        public string Industry { get; set; } = string.Empty;
        public long MarketCap { get; set; }

    }
}