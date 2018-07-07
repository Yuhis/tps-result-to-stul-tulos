using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.TPSResults
{
    public class EventElement
    {
        public IList<ResultsElement> Results { get; set; }

        public EventElement(XElement xe)
        {
            Results = new List<ResultsElement>();
            var resultElements = from el in xe.Elements("Results") select el;
            foreach(XElement r in resultElements)
            {
                Results.Add(new ResultsElement(r));
            }
        }
    }
}
