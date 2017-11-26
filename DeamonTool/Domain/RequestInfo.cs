using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeamonTool.Domain
{
    public class ReceiveInfo : IPackageInfo<string>, IPackageInfo
    {
        public string Head { get; set; }
        public string BodyLength { get; set; }
        public string OrderNumber { get; set; }
        public string FunctionNumber { get; set; }
        public string QorA { get; set; }
        public string MachineNumber { get; set; }
        public string Content { get; set; }
        public string TimeStr { get; set; }
        public string CheckStr { get; set; }
        public bool IsLegal { get; set; } = false;

        public string OriginStr { get; set; }

       
        public string Key
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        ///     获取校验值
        /// </summary>
        /// <param name="command">命令内容</param>
        /// <returns></returns>
        public string GetCheckByte(string command)
        {
            command = command.ToUpper();

            var count = command.Aggregate<char, uint>(0, (current, item) => current + item);
            count = ~count;
            count++;
            return count.ToString("x8").Substring(6, 2).ToUpper();
        }
    }
}
