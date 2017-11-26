using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using DeamonTool.Domain;
namespace DeamonTool.Cache
{
    public class LogCache
    {
        private static ConcurrentQueue<MachineLog> _queue = new ConcurrentQueue<MachineLog>();

        private LogCache()
        {

        }

        public static readonly LogCache Instance = new LogCache();
        public void Push(MachineLog log)
        {
            _queue.Enqueue(log);
        }

        public List<MachineLog> GetItems(int count = 20)
        {
            var result = new List<MachineLog>();
            while (!_queue.IsEmpty && result.Count() < count)
            {
                MachineLog log;
                if (_queue.TryDequeue(out log))
                {
                    result.Add(log);
                }
            }
            return result;
        }

        public bool IsEmpty
        {
            get
            {
                return _queue.IsEmpty;
            }
        }
    }
}
