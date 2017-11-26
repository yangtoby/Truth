using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeamonTool.Domain;
namespace DeamonTool.Repository
{
    class DeviceLogProvider:MongoBaseProvider<MachineLog>
    {
        public DeviceLogProvider():base(Parameters.LogUrl)
        {
            collectionName = "DeviceLog";
           
        }
        public void Add(List<MachineLog> logs)
        {
            var models = new List<WriteModel<MachineLog>>();
            foreach (var m in logs)
                models.Add(new InsertOneModel<MachineLog>(m));
            
            BulkWrite(models);
        }

        


    }
}
