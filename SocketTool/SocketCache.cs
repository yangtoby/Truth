using SuperSocket.ClientEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool
{
    public class SocketCache
    {
        public ConcurrentDictionary<string, EasyClient> _cacheDic = new ConcurrentDictionary<string, EasyClient>();
        private SocketCache()
        {

        }
        public static readonly SocketCache Instance = new SocketCache();

        public bool Add(string key, EasyClient value)
        {

            if (_cacheDic.ContainsKey(key))
            {
                EasyClient oldValue;
                _cacheDic.TryRemove(key, out oldValue);
            }
            var result = _cacheDic.TryAdd(key, value);
           // Statistic();
            return result;
        }

        public bool Remove(string key)
        {
            if (_cacheDic.ContainsKey(key))
            {
                EasyClient oldValue;
                return _cacheDic.TryRemove(key, out oldValue);
            }
            return true;
        }

        public string Statistic()
        {
            var allCount = _cacheDic.Count();
            var aliveCount = _cacheDic.Where(m => m.Value.IsConnected).Count();
            var stopCount = _cacheDic.Where(m => !m.Value.IsConnected).Count();

            var result = String.Format("All Count:{0}, Alive Count:{1}, Stop Count:{2}, DateTime:{3}", allCount, aliveCount, stopCount, DateTime.Now);
            Console.WriteLine(result);
            return result;
        }


    }
}
