using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketTool
{
    public class YjCommand
    {
        /// <summary>
        ///     流水号记录（上次使用过的，新流水号序号在此基础上+1）
        /// </summary>
        private static int _serialNumber = 0xFFFF;
        private static readonly string _deviceHead = "@@@@";
        private static readonly string _serverHead = "####";

        /// <summary>
        ///     有参构造函数，用于生成发送给机器的指令
        /// </summary>
        /// <param name="machineNumber">机器编号</param>
        /// <param name="commandName">指令名称</param>
        /// <param name="content">指令内容</param>
        /// <param name="qOrA">请求或回答</param>
        public YjCommand(string machineNumber, string commandName, string content, CommandQorA qOrA)
        {
            CreateDate = DateTime.Now;
            OrderNumber = SerialNumber;
            MachineNumber = machineNumber;
            Content = content;
            FunctionNumber = commandName.ToUpper();
            Head = _deviceHead;
            QorA = qOrA;
            CreateCommand();
        }

        public YjCommand(string machineNumber, string commandName, string content, string orderNumner, CommandQorA qOrA)
        {
            CreateDate = DateTime.Now;
            OrderNumber = orderNumner;
            MachineNumber = machineNumber;
            Content = content;
            FunctionNumber = commandName.ToUpper();
            Head = _deviceHead;
            QorA = qOrA;
            CreateCommand();
        }



        /// <summary>
        ///     有参构造函数，用户将系统收到的信息转换为command类型
        /// </summary>
        public YjCommand(byte[] msg)
        {
            IsLegal = false;
            if (msg.Length < 24)
                return;

            var msgStr = Encoding.ASCII.GetString(msg);

            CommandStr = msgStr;

            CreateDate = DateTime.Now;

            //帧头 1-4
            Head = msgStr.Substring(0, 4);
            if (Head != _serverHead)
                return;
            //长度 5-8
            Length = Convert.ToInt32(msgStr.Substring(4, 4), 16);
            if (Length < 1)
                return;

            //流水号
            OrderNumber = msgStr.Substring(8, 4);

            //功能号(命令名称)
            FunctionNumber = msgStr.Substring(12, 2);

            //发起or回应
            QorA = msgStr.Substring(14, 2) == "RQ" ? CommandQorA.Request : CommandQorA.Answer;

            //设备编号
            var numberLength = 0;
            if (FunctionNumber.ToLower() != "hb")
            {
                numberLength = Convert.ToInt32(msgStr.Substring(16, 2), 16);
                if (numberLength < 1)
                    return;
                MachineNumber = msgStr.Substring(18, numberLength);
            }

            Content = msgStr.Substring(18 + numberLength, msgStr.Length - 20 - numberLength);
            //时间
            //string timeStr = msgStr.Substring(msgStr.Length - 8, 6);

            //校验值
            CheckStr = msgStr.Substring(msgStr.Length - 2, 2);

            //指令的校验
            var check = GetCheckByte(Encoding.ASCII.GetString(msg).Substring(0, msg.Length - 2));
            if (check != CheckStr)
                return;

            IsLegal = true;
        }

        /// <summary>
        ///     获取新的流水号
        /// </summary>
        internal static string SerialNumber
        {
            get
            {
                _serialNumber++;
                if (_serialNumber > 0xFFFF)
                    _serialNumber = 0x0000;
                return Convert.ToString(_serialNumber, 16).PadLeft(4, '0').ToUpper();
            }
        }

        /// <summary>
        ///     帧头
        /// </summary>
        public string Head { get; set; }

        /// <summary>
        ///     命令名称
        /// </summary>
        public string FunctionNumber { get; set; }

        public CommandQorA QorA { get; set; }

        public string QorAStr
        {
            get
            {
                switch (QorA)
                {
                    case CommandQorA.Request:
                        return "RQ";
                    case CommandQorA.Answer:
                        return "AC";
                }
                return string.Empty;
            }
        }

        public bool HasChecked { get; set; }

        /// <summary>
        /// 设备编号
        /// </summary>
        public string MachineNumber { get; set; }

        /// <summary>
        ///     命令长度
        /// </summary>
        public int Length { get; set; }

        public CommandSource Source
        {
            get
            {
                switch (Head)
                {
                    case "@@@@":
                        return CommandSource.Machine;
                    case "####":
                        return CommandSource.Service;
                }
                return CommandSource.Unknow;
            }
        }

        /// <summary>
        ///     流水号
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        ///     设备上报的命令时间
        /// </summary>
        public TimeSpan CommandTime { get; set; }

        /// <summary>
        ///     命令创建时间（服务器时间）
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public bool HasRequest { get; set; } = false;


        public string AnswerCommand { get; set; }

        /// <summary>
        ///     命令内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///     命令全文
        /// </summary>
        public string CommandStr { get; set; }

        /// <summary>
        ///     校验
        /// </summary>
        public string CheckStr { get; set; }

        /// <summary>
        ///     是否合法
        /// </summary>
        public bool IsLegal { get; set; } = true;

        /// <summary>
        ///     拼接指令
        /// </summary>
        /// <returns></returns>
        private void CreateCommand()
        {
            var len = OrderNumber.Length + FunctionNumber.Length + QorAStr.Length + Content.Length + 6 + 2;
            CommandStr = Head + Convert.ToString(len, 16).PadLeft(4, '0').ToUpper() + OrderNumber +
                         FunctionNumber.ToUpper() + QorAStr + Content + GetTimeString(CreateDate.TimeOfDay);
            CheckStr = GetCheckByte(CommandStr);
            CommandStr += CheckStr;
            HasChecked = true;
            CommandStr = CommandStr.ToUpper();
        }

        public static string GetTimeString(TimeSpan time)
        {
            var ret = "";
            ret += Convert.ToString(time.Hours, 16).PadLeft(2, '0');
            ret += Convert.ToString(time.Minutes, 16).PadLeft(2, '0');
            ret += Convert.ToString(time.Seconds, 16).PadLeft(2, '0');

            return ret;
        }




        /// <summary>
        ///     获取校验值
        /// </summary>
        /// <param name="command">命令内容</param>
        /// <returns></returns>
        private static string GetCheckByte(string command)
        {
            command = command.ToUpper();

            var count = command.Aggregate<char, uint>(0, (current, item) => current + item);
            count = ~count;
            count++;
            return count.ToString("x8").Substring(6, 2).ToUpper();
        }

        #region Call Back 

        public object CallBackArgs { get; set; }

        public Action<YjCommand, object> CallBack { get; set; } = null;

        public bool IsNeendCallback { get; set; } = false;

        public bool HasCallBack { get; set; } = false;

        #endregion


    }
}
