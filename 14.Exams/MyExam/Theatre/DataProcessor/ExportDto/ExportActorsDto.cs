using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Actor")]
    public class ExportActorsDto
    {
        [XmlAttribute(AttributeName = "FullName")]
        public string FullName { get; set; }

        [XmlAttribute(AttributeName = "MainCharacter")]
        public string MainCharacter { get; set; }
    }
}
