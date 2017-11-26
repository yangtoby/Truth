using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeamonTool.Domain
{
    public class SocketCacheItem
    {
        
        public string MachineId { get; set; }
        public SocketClient SocketClient { get; set; }
        public string Indentity { get; set; }
       
        public string DoorId { get; set; }
    }
}
