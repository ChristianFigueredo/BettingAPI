using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.output
{
    public class Roulette
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string finalColor { get; set; }
        public int finalNumber { get; set; }
        public int statusId { get; set; }
        public string statusDetail { get; set; }
        private string[] statusArray = { "CREATED", "OPENED", "CLOSED" };

        public Roulette(int r, DateTime s, DateTime e, string fC, int fN, int sI) 
        {
            id = r;
            startDate = s;
            endDate = e;
            finalColor = fC;
            finalNumber = fN;
            statusId = sI;
            statusDetail = statusArray[sI];
        }
    }
}
