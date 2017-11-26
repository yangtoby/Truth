using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace DeamonTool.Domain
{
    public class MachineLog
    {
        [BsonElement("deviceId")]
        public string MachineId { get; set; }

        [BsonElement("fromName")]
        public string FromName { get; set; }

        [BsonElement("actionStatus")]
        public ActionStatus ActionStatus { get; set; }

        [BsonElement("functionNum")]
        public string FunctionNum { get; set; }

        [BsonElement("msg")]
        public string Msg { get; set; }

        [BsonElement("createTime")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [BsonElement("updateTime")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UpdateTime { get; set; } = DateTime.Now;
    }




}
