using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoftJail.DataProcessor.ExportDto
{
    [XmlType("Prisoner")]
    public class ExportPrisonerDto
    {

        public int Id { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "IncarcerationDate")]
        public string IncarcerationDate { get; set; }

        [XmlArray("EncryptedMessages")]
        public Message[] EncryptedMessages { get; set; }
        [XmlType("Message")]
        public class Message
        {

            [XmlElement(ElementName = "Description")]
            public string Description { get; set; }
        }

    }
}