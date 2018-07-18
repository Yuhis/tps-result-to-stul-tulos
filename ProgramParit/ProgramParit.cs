using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.ProgramParit
{
    public class ProgramParit
    {
        public ProgramParit(string paritFileName, string tpsParitFileName)
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

            // parit.tps.txt tiedosto
            Console.WriteLine($"Parit.tps tiedosto: {tpsParitFileName}");
            //TPSParitWriter tpsParitWriter = new TPSParitWriter(tpsParitFileName);
        }
    }
}
