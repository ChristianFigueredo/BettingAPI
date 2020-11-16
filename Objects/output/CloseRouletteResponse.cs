using Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.output
{
    public class CloseRouletteResponse
    {
        public Result gameResult{ get; set;  }
        public List<BetResult> resultList { get; set;  }
        public Message message { get; set; }
    }
}
