using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DeamonTool.Domain;
using DeamonTool.Helper;
using DeamonTool.Cache;

namespace DeamonTool
{
    public class SocketClient
    {
        private string host;
        private int port;
        private DateTime lastRecieveDataTime;
        private DateTime lastRecieveOpenDoorTime = DateTime.MinValue;
        private EasyClient client;

        public EasyClient Client
        {
            get { return client; }

        }
        public DateTime LastReceiveOpenDoorTime
        {
            get { return LastReceiveOpenDoorTime; }
        }
        public string IdentityId
        {
            get
            {
                return client.GetHashCode().ToString();
            }
        }

        public SocketClient()
        {
            this.host = Parameters.Host;
            this.port = Parameters.Port;
            client = new EasyClient();
            client.Initialize(new ReceiveFilter(), SocketClientReceived);
            client.Connected += SocketClientConnected;
            client.Closed += SocketClientClosed;
            client.Error += SocketClientError;
        }

        private void SocketClientError(object sender, ErrorEventArgs e)
        {

            Log.LogError(string.Format("Socket Error:{0}", e.Exception.Message));
            if (Parameters.IsDebug == 0)
            {
                Console.WriteLine(string.Format("{0} Socket Error:{1}, Message:{2}", DateTime.Now, IdentityId, e.Exception.Message));
            }
        }

        private void SocketClientClosed(object sender, EventArgs e)
        {
            Log.LogError("Socket Closed");
            if (Parameters.IsDebug == 0)
            {
                Console.WriteLine(string.Format("{0} Socket Closed:{1}", DateTime.Now, IdentityId));
            }

        }

        private void SocketClientReceived(ReceiveInfo info)
        {
            lastRecieveDataTime = DateTime.Now;
            if (Parameters.IsDebug == 0)
            {
                Console.WriteLine(string.Format("{0} Recevie Msg:{1}", DateTime.Now, info.OriginStr));
            }
            if(Parameters.IsLogMongo ==0)
            {
                var cacheItem = SocketCache.Instance.Get(IdentityId);
                if(cacheItem != null)
                {
                    var deviceLog = new MachineLog
                    {
                        MachineId = cacheItem.MachineId,
                        ActionStatus = ActionStatus.Receive,
                        FunctionNum = info.FunctionNumber,
                        Msg = info.OriginStr,
                    };
                    LogCache.Instance.Push(deviceLog);
                }
            }
            if (info.IsLegal)
            {
                if (info.FunctionNumber.Equals("cd", StringComparison.InvariantCultureIgnoreCase))
                {
                    lastRecieveOpenDoorTime = DateTime.Now;
                }

            }
        }



        private void SocketClientConnected(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => SendHeartMsg());

        }

        public void Send(string msg)
        {
            client.Send(Encoding.ASCII.GetBytes(msg));
        }

        public void SendHeartMsg()
        {
            while (true)
            {
                if (!client.IsConnected)
                {
                    break;
                }
                if (lastRecieveDataTime.AddSeconds(Parameters.HeartInterval) < DateTime.Now)
                {
                    var cmdHeart = new YjCommand(string.Empty, "hb", string.Empty, CommandQorA.Request); //产生心跳包
                    client.Send(Encoding.ASCII.GetBytes(cmdHeart.CommandStr));
                    if (Parameters.IsDebug == 0)
                    {
                        Console.WriteLine(string.Format("{0} Send Heart Msg:{1}", DateTime.Now, cmdHeart.CommandStr));
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(Parameters.HeartInterval));
                if (lastRecieveDataTime.AddSeconds(360) < DateTime.Now) //超过6min还没有收到任何信息，说明连接出了问题
                {
                    client.Close();
                }
               
            }
        }
        public bool Connect(string registerMsg)
        {
            var flag = client.ConnectAsync(new DnsEndPoint(host, port));

            if (flag.Result)
            {
                if (client.IsConnected)
                {
                    if (!string.IsNullOrEmpty(registerMsg))
                    {
                        client.Send(Encoding.ASCII.GetBytes(registerMsg));
                    }
                    lastRecieveDataTime = DateTime.Now;
                    if (Parameters.IsDebug == 0)
                    {
                        Console.WriteLine(string.Format("{0} Send RegisterMsg:{1}", DateTime.Now, registerMsg));
                    }
                    return true;
                }
                

               
            }
           
            return false;
        }
    }


}
