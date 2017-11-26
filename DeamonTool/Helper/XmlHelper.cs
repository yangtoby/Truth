using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DeamonTool.Domain;

namespace DeamonTool
{
    class XmlHelper
    {
        public static List<MapInfo> LoadMapInfos()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("MapInfos.xml");

            XmlElement root = doc.DocumentElement;

            XmlNodeList personNodes = root.GetElementsByTagName("mapInfo");

            var result = new List<MapInfo>();
            foreach (XmlNode node in personNodes)
            {
               
                string deviceId = ((XmlElement)node).GetAttribute("deviceId");
                string doorId = ((XmlElement)node).GetAttribute("doorId");
                var info = new MapInfo();
                info.MachineId = deviceId;
                info.DoorId = doorId;
                result.Add(info);
            }
            return result;

           
        }
    }
}
