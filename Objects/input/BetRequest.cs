using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.input
{
    public class BetRequest
    {
        public string token { get; set; }
        public float amount { get; set; }
        public int number { get; set; }
        public string color { get; set; }
        public int rouletteId { get; set; }
    }
}
