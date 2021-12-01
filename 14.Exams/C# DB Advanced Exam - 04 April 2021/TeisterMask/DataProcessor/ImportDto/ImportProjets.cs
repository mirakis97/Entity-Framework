using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;
using TeisterMask.Data.Models.Enums;

namespace TeisterMask.DataProcessor.ImportDto
{
    [XmlType("Project")]
    public class ImportProjets
    {
        [Required]
        [MaxLength(40)]
        [MinLength(3)]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement(ElementName = "DueDate")]
        public string DueDate { get; set; }

        [XmlArray(ElementName = "Tasks")]
        public Tasks[] Task { get; set; }
        [XmlType("Task")]
        public class Tasks
        {
            [Required]
            [MaxLength(40)]
            [MinLength(2)]
            [XmlElement(ElementName = "Name")]
            public string Name { get; set; }

            [Required]
            [XmlElement(ElementName = "OpenDate")]
            public string OpenDate { get; set; }
            [Required]
            [XmlElement(ElementName = "DueDate")]
            public string DueDate { get; set; }
            [Range(0, 3)]
            [XmlElement(ElementName = "ExecutionType")]
            public int ExecutionType { get; set; }
            [Range(0, 4)]
            [XmlElement(ElementName = "LabelType")]
            public int LabelType { get; set; }
        }

    }
}
