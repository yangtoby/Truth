using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketTool
{
    class Watcher
    {
        public void Start()
        {
            try
            {
                int connectedCount = 0;
                if (Parameters.MaxSocketCount < Parameters.BatchSocketCount)
                {
                    Parameters.BatchSocketCount = Parameters.MaxSocketCount;
                }
                for (int i = 1; i <= Parameters.MaxSocketCount; i++)
                {
                    Task.Factory.StartNew(() =>
                    {
                        var registerMsg = @"@@@@00280001IDRQ0812345678083C04FF080000092D19FA";
                        var socket = new SocketClient();
                        socket.Connect(registerMsg);
                        if (Parameters.IsDebug == 0)
                        {
                            Console.WriteLine(string.Format("{0} Socket Connected:{1}", DateTime.Now, socket.IdentityId));
                        }
                        Interlocked.Increment(ref connectedCount);
                        Console.WriteLine("Connected Count:" + connectedCount);
                    });

                    if (i % Parameters.BatchSocketCount == 0)
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(Parameters.BatchSocketInterval));
                }
            }
            catch(Exception ex)
            {
                Log.LogFatal("msg:" + ex.Message + "\r\nSource:" + ex.Source + "\r\nStactTrace:" + ex.StackTrace);
                Console.WriteLine("Bug Bug Bug!!!!");
            }
        }
    }
}
