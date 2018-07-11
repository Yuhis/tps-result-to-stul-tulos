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

        public void WriteAll()
        {
            var outFileStream = System.IO.File.Create(ParitFileName);
            using (StreamWriter stream = new StreamWriter(outFileStream, Encoding.UTF8))
            {
                var writer = new CsvWriter(stream);
                writer.Configuration.Delimiter = DefaultCsvDelimiter;
                writer.Configuration.QuoteAllFields = true;
                writer.Configuration.HasHeaderRecord = true;
                writer.Configuration.RegisterClassMap<TPSParitLineClassMap>();

                writer.WriteHeader<TPSParitLine>();
                foreach (TPSParitLine pari in ParitLines)
                {
                    writer.WriteRecord<TPSParitLine>(pari);
                    writer.NextRecord();
                }
            }
        }
    }
}
