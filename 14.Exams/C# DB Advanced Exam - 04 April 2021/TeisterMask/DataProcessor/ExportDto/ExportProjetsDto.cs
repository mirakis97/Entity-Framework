using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
    public class ExportProjetsDto
    {
        [XmlElement(ElementName = "ProjectName")]
        public string ProjectName { get; set; }

        [XmlElement(ElementName = "HasEndDate")]
        public string HasEndDate { get; set; }

        [XmlArray("Tasks")]
        public ExportTasksDto[] Tasks { get; set; }

        [XmlAttribute(AttributeName = "TasksCount")]
        public int TasksCount { get; set; }

    }
}
