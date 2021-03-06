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

        [XmlArray("Tasks")]
        public TaskDto[] Task { get; set; }
        

    }
}
