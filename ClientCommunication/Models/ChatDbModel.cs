using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientCommunication.Models
{
    internal class ChatDbModel
    {
        public string sender { get; set; }
        public string message { get; set; }
        public int chatid { get; set; }
    }
}
