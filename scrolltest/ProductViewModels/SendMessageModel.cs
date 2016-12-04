using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scrolltest.ProductViewModels
{
    public class SendMessageModel
    {
        public string Receiver { get; set; }
        public string SenderName { get; set; }
        public string TimeSent { get; set; }
        public string Content { get; set; }
        public int msgId { get; set; }

    }
}