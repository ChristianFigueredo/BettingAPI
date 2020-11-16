using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.output
{
    public class BetResult
    {
        public string client { get; set; }
        public string betType { get; set; }
        public double betAmount { get; set; }
        public int betNumber { get; set; }
        public string betcolor { get; set; }
        public double pay { get; set; }
        public int clientId { get; set; }
        public string result { get; set; }
        public BetResult(string c, string type, double amount, int number, string color, double p, int id, string res) 
        {
            client = c;
            betType = type;
            betAmount = amount;
            betNumber = number;
            pay = p;
            clientId = id;
            result = res;
        }
    }
}
