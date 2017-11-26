using SuperSocket.ClientEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTool
{
    public class SocketClient
    {
        private string host;
        private int port;
        private DateTime lastRecieveHeartTime;
        private EasyClient socketClient;

        public EasyClient Client
        {
            get { return socketClient; }

        }

        public int IdentityId
        {
            get
            {
                return socketClient.GetHashCode();
            }
        }

        public SocketClient()
        {
            this.host = Parameters.Host;
            this.port = Parameters.Port;
            socketClient = new EasyClient();
            socketClient.Initialize(new ReceiveFilter(), SocketClientReceived);
            socketClient.Connected += SocketClientConnected;
            socketClient.Closed += SocketClientClosed;
            socketClient.Error += SocketClientError;
        }

        private void SocketClientError(object sender, ErrorEventArgs e)
        {
          
            Log.LogError(string.Format("Socket Error:{0}", e.Exception.Message));
            if (Parameters.IsDebug == 0)
            {
                Console.WriteLine(string.Format("{0} Socket Error:{1}", DateTime.Now, IdentityId));
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
            lastRecieveHeartTime = DateTime.Now;
            if (Parameters.IsDebug == 0)
            {
                Console.WriteLine(string.Format("{0} Recevie Msg:{1}", DateTime.Now, info.OriginStr));
            }
            if (info.IsLegal)
            {
                //do something
               
            }
        }

        private void SocketClientConnected(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => SendHeartMsg());

        }

        public void Send(string msg)
        {
            socketClient.Send(Encoding.ASCII.GetBytes(msg));
        }

        public void SendHeartMsg()
        {
            while (true)
            {

                if (lastRecieveHeartTime.AddSeconds(Parameters.HeartInterval) < DateTime.Now)
                {
                    var cmdHeart = new YjCommand(string.Empty, "hb", string.Empty, CommandQorA.Request); //产生心跳包
                    socketClient.Send(Encoding.ASCII.GetBytes(cmdHeart.CommandStr));
                    if (Parameters.IsDebug == 0)
                    {
                        Console.WriteLine(string.Format("{0} Send Heart Msg:{1}", DateTime.Now, cmdHeart.CommandStr));
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(Parameters.HeartInterval));
                if (lastRecieveHeartTime.AddSeconds(360) < DateTime.Now) //超过6min还没有收到任何信息，说明连接出了问题
                {
                    socketClient.Close();
                }
                if (!socketClient.IsConnected)
                {
                    break;
                }
            }
        }
        public bool Connect(string registerMsg)
        {
            var flag = socketClient.ConnectAsync(new DnsEndPoint(host, port));
            
            if (flag.Result)
            {
                socketClient.Send(Encoding.ASCII.GetBytes(registerMsg));
                lastRecieveHeartTime = DateTime.Now;
                if (Parameters.IsDebug == 0)
                {
                    Console.WriteLine(string.Format("{0} Send RegisterMsg:{1}", DateTime.Now, registerMsg));
                }
                SocketCache.Instance.Add(IdentityId.ToString(), socketClient);
                return true;
            }
            SocketCache.Instance.Add(IdentityId.ToString(), socketClient);
            return false;
        }
    }

}
