using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper;

namespace tps_result_to_stul_tulos.STULTulos
{
    public class STULTulosWriter
    {
        private const string DefaultCsvDelimiter = ",";
        private string TulosFileName { get; set; }
        public IList<STULTulosLine> TulosLines { get; set; }

        public STULTulosWriter(string tulosFileName)
        {
            TulosFileName = tulosFileName;
            TulosLines = new List<STULTulosLine>();
        }
        public void WriteAll()
        {
            var outFileStream = System.IO.File.Create(TulosFileName);
            using (StreamWriter stream = new StreamWriter(outFileStream, Encoding.UTF8))
            {
                var writer = new CsvWriter(stream);
                writer.Configuration.Delimiter = DefaultCsvDelimiter;
                writer.Configuration.QuoteAllFields = true;
                writer.Configuration.RegisterClassMap<STULTulosLineClassMap>();

                foreach (STULTulosLine tulos in TulosLines)
                {
                    writer.WriteRecord<STULTulosLine>(tulos);
                    writer.NextRecord();
                }
            }
        }
    }
}
