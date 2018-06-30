using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos
{
    public class TPSResult
    {
        private XDocument xdoc;

        public void Parse(string resultFileName)
        {
            xdoc = XDocument.Parse(File.ReadAllText(resultFileName, Encoding.UTF8));
        }


    }
}
