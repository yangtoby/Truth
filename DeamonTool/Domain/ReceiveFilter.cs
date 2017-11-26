using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeamonTool.Domain
{
    public class ReceiveFilter : FixedHeaderReceiveFilter<ReceiveInfo>
    {
        public ReceiveFilter() : base(8)
        {

        }
        public override ReceiveInfo ResolvePackage(IBufferStream bufferStream)
        {
            var msgStr = bufferStream.ReadString(Size, Encoding.ASCII);
            var info = new ReceiveInfo();
            info.OriginStr = msgStr;
            info.Head = msgStr.Substring(0, 4);
            // if (info.Head != "####") return info;
            info.BodyLength = msgStr.Substring(4, 4);
            info.OrderNumber = msgStr.Substring(8, 4);
            info.FunctionNumber = msgStr.Substring(12, 2);
            info.QorA = msgStr.Substring(14, 2);

            if (info.FunctionNumber.ToLower() != "hb")
            {
                info.TimeStr = msgStr.Substring(msgStr.Length - 8, 6);
                info.Content = msgStr.Substring(16, msgStr.Length - 24);
            }
            else
            {
                info.Content = msgStr.Substring(16, msgStr.Length - 18);
             }

            info.CheckStr = msgStr.Substring(msgStr.Length - 2, 2);
            var check = info.GetCheckByte(msgStr.Substring(0, Size - 2));
            //if (check != info.CheckStr)
            //{
            //    return info;
            //}
            info.IsLegal = true;
            return info;
            //var info= new StringPackageInfo(string.Empty, bufferStream.Skip(8).ReadString(Size - 8, Encoding.UTF8), null);
            //return info as RequestInfo;
        }

        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {

            var msgStr = bufferStream.ReadString(Size, Encoding.ASCII);
            var bodyLen = Convert.ToInt32(msgStr.Substring(4, 4), 16);
            return bodyLen;
        }
    }

    public class ReceiveFilter2 : TerminatorReceiveFilter<StringPackageInfo>
    {
        public ReceiveFilter2() : base(Encoding.ASCII.GetBytes("AAA"))
        {

        }

        public override StringPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            var s = bufferStream.ReadString((int)bufferStream.Length, Encoding.ASCII);
            return null;
        }
    }
}
