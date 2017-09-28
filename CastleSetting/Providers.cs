using System;
using System.Collections.Generic;
using System.Text;

namespace CastleSetting
{
   public class Root
    {
        public List<Provider> Providers { get; set; }
    }

    public class Provider
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public List<Template> Templates { get; set; }
    }

    public class Template
    {
        public int TemplateTypeId { get; set; }
        public int ChannelTypeId { get; set; }
        public string TemplateName { get; set; }
    }
}
