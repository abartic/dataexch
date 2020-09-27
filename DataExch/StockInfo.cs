using System;

namespace DataExch
{
    public class StockInfo
    {
        public DateTime Date { get; set; }

        public int OpenPrice { get; set; }

        public int ClosePrice { get; set; }

        public string Summary { get; set; }
    }
}
