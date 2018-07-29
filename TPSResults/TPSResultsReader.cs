using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.TPSResults
{
    public class TPSResultsReader
    {
        public TPSEventElement Event { get; private set; }

        public TPSResultsReader(string resultFileName)
        {
            XDocument xdoc = XDocument.Parse(File.ReadAllText(resultFileName, Encoding.UTF8));
            Event = new TPSEventElement(xdoc.Element("Event"));
        }
    }
}
