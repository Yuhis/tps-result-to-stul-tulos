using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper;

namespace tps_result_to_stul_tulos.STULParitReader
{
    public class ParitReader
    {
        private const string DefaultCsvDelimiter = ",";
        private string ParitFileName { get; set; }
        public IList<ParitLine> ParitLines { get; set; }

        public ParitReader(string paritFileName)
        {
            ParitFileName = paritFileName;
            ParitLines = new List<ParitLine>();
            ReadAll();
        }

        private void ReadAll()
        {
            var inFileStream = System.IO.File.Open(ParitFileName,FileMode.Open,FileAccess.Read);
            using (StreamReader stream = new StreamReader(inFileStream, Encoding.UTF8))
            {
                var reader = new CsvReader(stream);
                reader.Configuration.HasHeaderRecord = false;

                var records = reader.GetRecords<ParitLine>();
                foreach(ParitLine pari in records)
                {
                    ParitLines.Add(pari);
                }
            }
        }
    }
}
