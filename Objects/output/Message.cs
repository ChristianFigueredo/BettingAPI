using System;
using System.Collections.Generic;
using System.Text;

namespace Objects.output
{
    public class Message
    {
        public string code { get; set; }
        public string detail { get; set; }
        public Exception exception { get; set; }
        public Message(string c, string d, Exception ?e) 
        {
            code = c;
            detail = d;
            exception = e;
        }

        public Message() { }
    }

    

}
