using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.ProgramTulos
{
    public class ProgramTulos
    {
        public ProgramTulos(string paritFileName, string resultsFileName, string tulosFileName)
        {
            // parit.txt tiedosto
            if(!File.Exists(paritFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {paritFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Parit tiedosto: {paritFileName}");
            // STULParitReader paritReader = new STULParitReader(paritFileName);
            // foreach(string e in paritReader.LineErrors){
            //     Console.WriteLine($"Virhe: {e}");
            // }

            // result.xml tiedosto
            if(!File.Exists(resultsFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {resultsFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Results tiedosto: {resultsFileName}");
            //ResultsReader resultsReader = new ResultsReader(resultsFileName);

            // tulos.txt tiedosto
            Console.WriteLine($"Tulos tiedosto: {tulosFileName}");
            //STULTulosWriter tulosWriter = new STULTulosWriter(tulosFileName);
        }
    }
}
