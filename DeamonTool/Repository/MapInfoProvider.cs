using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeamonTool.Domain;

namespace DeamonTool.Repository
{
    public class MapInfoProvider
    {
        private string _sqlConnection;
        public MapInfoProvider(string sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
       

        public List<MapInfo> GetMapInfo()
        {
            using (SqlConnection connection = new SqlConnection(_sqlConnection))
            {
                connection.Open();
                var rows = connection.Query("SELECT [InnerNumber],[DoorId] FROM[dbo].[Machines] where InnerNumber is not null and DoorId is not null");
                var result = new List<MapInfo>();
                foreach (var row in rows)
                {
                    var info = new MapInfo();
                    
                    info.MachineId = row.InnerNumber;
                    info.DoorId = row.DoorId.ToString();
                    result.Add(info);
                }
                return result;
            }
        }
    }
}
