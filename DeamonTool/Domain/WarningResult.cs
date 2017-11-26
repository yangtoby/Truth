using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DeamonTool.Domain
{
    public class WarningResult
    {
        [JsonProperty("errcode")]
        public int ErrCode;
        [JsonProperty("errmsg")]
        public string ErrMsg;
        [JsonProperty("msgid")]
        public long MsgId;
    }
}
