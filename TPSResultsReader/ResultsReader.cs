using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.TPSResultsReader
{
    public class ResultsReader
    {
        public EventElement Event { get; private set; }

        public ResultsReader(string resultFileName)
        {
            XDocument xdoc = XDocument.Parse(File.ReadAllText(resultFileName, Encoding.UTF8));
            Event = new EventElement(xdoc.Element("Event"));
        }
    }
}
