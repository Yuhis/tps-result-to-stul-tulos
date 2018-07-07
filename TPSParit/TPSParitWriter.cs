using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper;

namespace tps_result_to_stul_tulos.TPSParit
{
    public class TPSParitWriter
    {
        private const string DefaultCsvDelimiter = ",";
        private string ParitFileName { get; set; }
        public IList<TPSParitLine> ParitLines { get; set; }

        public TPSParitWriter(string paritFileName)
        {
            ParitFileName = paritFileName;
            ParitLines = new List<TPSParitLine>();
        }
    }
}
