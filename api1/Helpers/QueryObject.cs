using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api1.Helpers
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;

        //Sorting Functionlities
        public string? SortBy { get; set; } = null;
        public bool isDescending { get; set; } = false;
    }
}