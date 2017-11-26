using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;
using SuperSocket.ClientEngine;
using DeamonTool.Cache;
using DeamonTool.Repository;
using DeamonTool.Helper;
using DeamonTool.Domain;

namespace DeamonTool
{

    class Watcher
    {

        public void Start()
        {
            var mapInfos = LoadAllMapInfo();
            if (Parameters.MaxSocketCount > mapInfos.Count)
                Parameters.MaxSocketCount = mapInfos.Count();
            
            int connectedCount = 0;

            for (int i = 1; i <= Parameters.MaxSocketCount; i++)
            {
                var mapInfo = mapInfos[i - 1];
                Task.Factory.StartNew((obj) =>
                {
                    var info = obj as MapInfo;
                    var registerMsg = RegisterMsg(info.MachineId);
                    var socket = new SocketClient();
                    socket.Connect(registerMsg);
                    if (Parameters.IsDebug == 0)
                    {
                        Console.WriteLine(string.Format("{0} Socket Connected:{1}", DateTime.Now, socket.IdentityId));
                    }
                    Interlocked.Increment(ref connectedCount);
                    Console.WriteLine("Connected Count:" + connectedCount);
                  //  OpenDoor(info.DoorId);
                    var item = new SocketCacheItem
                    {
                        SocketClient = socket,
                        MachineId = info.MachineId,
                        DoorId = info.DoorId,
                        Indentity = socket.IdentityId,
                    };
                    SocketCache.Instance.Add(socket.IdentityId, item);
                }, mapInfo);
                if (i % Parameters.BatchSocketCount == 0)
                    Thread.Sleep(TimeSpan.FromSeconds(Parameters.ConcurrentInterval));
            }

            if (Parameters.IsLogMongo == 0)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {

                        if (LogCache.Instance.IsEmpty)
                            Thread.Sleep(TimeSpan.FromSeconds(30));
                        var logs = LogCache.Instance.GetItems();
                        if (logs.Count > 0)
                            new DeviceLogProvider().Add(logs);
                    }
                });
            }

        }

        private List<MapInfo> LoadAllMapInfo()
        {
            var conStr = Parameters.SqlServerUrl;
            var listInfos = new MapInfoProvider(conStr).GetMapInfo();
            ConcurrentBag<MapInfo> bag = new ConcurrentBag<MapInfo>();
            foreach (var item in listInfos)
            {
                bag.Add(item);
            }
            return listInfos;

        }
        private void OpenDoor(string doorId)
        {
            var doorUrl = string.Format(Parameters.OpenDoorUrl, doorId);
            var opendoor = HttpRequestHelper.GetHttp(doorUrl);
            if (string.IsNullOrEmpty(opendoor))
            {
                var warningMsg = string.Format(Parameters.WarningUrl, "Opendoor异常", doorUrl);
                HttpRequestHelper.PostHttp(warningMsg, "");
            }
        }

        private string RegisterMsg(string machineId)
        {
            var content = Convert.ToString(machineId.Length, 16).PadLeft(2, '0').ToUpper() + machineId.ToUpper();
            var cmd = new YjCommand(machineId, "ID", content, CommandQorA.Request);
            return cmd.CommandStr;
        }


    }
}
