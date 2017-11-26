using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using SuperSocket.ClientEngine;
using DeamonTool.Domain;

namespace DeamonTool
{
    public class SocketCache
    {
        public ConcurrentDictionary<string, SocketCacheItem> _cacheDic = new ConcurrentDictionary<string, SocketCacheItem>();
        private SocketCache()
        {

        }
        public static readonly SocketCache Instance = new SocketCache();

        public bool Add(string key, SocketCacheItem value)
        {
           
           if( _cacheDic.ContainsKey(key))
            {
                SocketCacheItem oldValue;
                _cacheDic.TryRemove(key, out oldValue);
            }
            var result= _cacheDic.TryAdd(key, value);
            //Statistic();
            return result;
        }

        public SocketCacheItem Get(string key)
        {
            SocketCacheItem item = null;
            _cacheDic.TryGetValue(key, out item);
            return item;
        }

        public bool Remove(string key)
        {
            if(_cacheDic.ContainsKey(key))
            {
                SocketCacheItem oldValue ;
               return  _cacheDic.TryRemove(key, out oldValue);
            }
            return true;
        }

        public string Statistic()
        {
            var allCount = _cacheDic.Count();
            var aliveCount = _cacheDic.Where(m=>m.Value.SocketClient.Client.IsConnected).Count();
            var stopCount = _cacheDic.Where(m => !m.Value.SocketClient.Client.IsConnected).Count();
            //foreach(var item in _cacheDic)
            //{
            //    Console.WriteLine(item.Value.SocketClient.Client.IsConnected);
            //}
            var result= String.Format("All Count:{0}, Alive Count:{1}, Stop Count:{2}, DateTime:{3}", allCount, aliveCount, stopCount, DateTime.Now);
            Console.WriteLine(result);
            return result;
        }
       

    }
}
