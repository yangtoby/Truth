﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using MVCBlog.Entities.Enums;
using MVCBlog.Common;
using System.Web;

namespace MVCBlog.Web.CommonHelper.Menu
{
    public class MenuHelper
    {
        public static List<MenuInfo> GetMenuInfo(MenuType type)
        {
            if (type == MenuType.后台)
            {
                string path = HttpContext.Current.Server.MapPath("~//App_Data//AdminMenu.json");
                string json = FileHelper.GetFileContent(path);
                if (!string.IsNullOrEmpty(json))
                {
                    return JsonConvert.DeserializeObject<List<MenuInfo>>(json).OrderBy(x => x.MenuPosition).ToList();
                }
            }
            return new List<MenuInfo>();
        }
    }
}
