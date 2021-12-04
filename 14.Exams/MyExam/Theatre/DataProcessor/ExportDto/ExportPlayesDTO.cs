using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Theatre.DataProcessor.ExportDto
{
    [XmlType("Play")]
    public class ExportPlayesDTO
    {
        [XmlAttribute(AttributeName = "Title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "Duration")]
        public string Duration { get; set; }

        [XmlAttribute(AttributeName = "Rating")]
        public string Rating { get; set; }

        [XmlAttribute(AttributeName = "Genre")]
        public string Genre { get; set; }

        [XmlArray("Actors")]
        public ExportActorsDto[] Actors { get; set; }

    }
}
