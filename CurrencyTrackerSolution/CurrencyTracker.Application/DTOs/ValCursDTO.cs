using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CurrencyTracker.Application.DTOs
{
    [XmlRoot("ValCurs")]
    public class ValCursDTO
    {
        [XmlAttribute("Date")]
        public string Date { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;

        [XmlElement("Valute")]
        public List<ValuteDTO> Valutes { get; set; } = new List<ValuteDTO>();
    }
}
