using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.TPSResults
{
    public class TPSEventElement
    {
        public IList<TPSResultsElement> Results { get; set; }

        public TPSEventElement(XElement xe)
        {
            Results = new List<TPSResultsElement>();
            var resultElements = from el in xe.Elements("Results") select el;
            foreach(XElement r in resultElements)
            {
                Results.Add(new TPSResultsElement(r));
            }
        }
    }
}
