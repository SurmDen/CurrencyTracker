using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyTracker.Application.DTOs
{
    public class ValuteDTO
    {
        [XmlAttribute("ID")]
        public string Id { get; set; } = string.Empty;

        [XmlElement("NumCode")]
        public string NumCode { get; set; } = string.Empty;

        [XmlElement("CharCode")]
        public string CharCode { get; set; } = string.Empty;

        [XmlElement("Nominal")]
        public int Nominal { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Value")]
        public decimal Value { get; set; }

        [XmlElement("VunitRate")]
        public string VunitRate { get; set; } = string.Empty;
    }
}
