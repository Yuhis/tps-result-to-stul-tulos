using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper;

namespace tps_result_to_stul_tulos.STULParit
{
    public class STULParitReader
    {
        private const string DefaultCsvDelimiter = ",";
        private string ParitFileName { get; set; }
        public IList<STULParitLine> ParitLines { get; set; }

        public STULParitReader(string paritFileName)
        {
            ParitFileName = paritFileName;
            ParitLines = new List<STULParitLine>();
            ReadAll();
        }

        private void ReadAll()
        {
            var inFileStream = System.IO.File.Open(ParitFileName,FileMode.Open,FileAccess.Read);
            using (StreamReader stream = new StreamReader(inFileStream, Encoding.UTF8))
            {
                var reader = new CsvReader(stream);
                reader.Configuration.HasHeaderRecord = false;
                reader.Configuration.RegisterClassMap<STULParitLineClassMap>();

                var records = reader.GetRecords<STULParitLine>();
                foreach(STULParitLine pari in records)
                {
                    ParitLines.Add(pari);
                }
            }
        }
    }
}
