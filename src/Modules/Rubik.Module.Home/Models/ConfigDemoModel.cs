using System.Xml.Serialization;

using Rubik.Service;
using Rubik.Service.Models;

namespace Rubik.Module.Home.Models
{
    public class ConfigDemoModel : IDemoModel
    {
        [XmlIgnore]
        public DemoType Type { get; set; }

        [XmlIgnore]
        public string Name { get; set; }

        [XmlIgnore]
        public object Page { get; set; }

        public string IconData { get; set; }

        public double IconWidth { get; set; }

        public double IconHeight { get; set; }

        [XmlArrayItem("Example")]
        public DemoExample[] Examples { get; set; }
    }

    public class DemoExample
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string SyntaxHighlighting { get; set; } = "XML";

        [XmlAttribute]
        public bool JustShowCode { get; set; }

        public string Code { get; set; }
    }
}
