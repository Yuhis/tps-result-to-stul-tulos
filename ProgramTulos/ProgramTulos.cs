using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using tps_result_to_stul_tulos.STULParit;
using tps_result_to_stul_tulos.TPSResults;
using tps_result_to_stul_tulos.STULTulos;

namespace tps_result_to_stul_tulos.ProgramTulos
{
    public class ProgramTulos
    {
        private STULParitReader _stulParitReader;
        private TPSResultsReader _tpsResultsReader;
        private STULTulosWriter _stulTulosWriter;

        public ProgramTulos(string stulParitFileName, string tpsResultsFileName, string stulTulosFileName)
        {
            // parit.txt tiedosto
            if(!File.Exists(stulParitFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {stulParitFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Parit tiedosto: {stulParitFileName}");
            _stulParitReader = new STULParitReader(stulParitFileName);
            foreach(string e in _stulParitReader.LineErrors){
                Console.WriteLine($"Virhe: {e}");
            }

            // result.xml tiedosto
            if(!File.Exists(tpsResultsFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {tpsResultsFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Results tiedosto: {tpsResultsFileName}");
            _tpsResultsReader = new TPSResultsReader(tpsResultsFileName);

            // tulos.txt tiedosto
            Console.WriteLine($"Tulos tiedosto: {stulTulosFileName}");
            _stulTulosWriter = new STULTulosWriter(stulTulosFileName);
        }

        public void WriteTulosTxt()
        {
            foreach(STULParitLine pari in _stulParitReader.ParitLines)
            {
                var tulos = new STULTulosLine(){
                    CoupleCode = pari.CoupleCode,
                    CoupleNames = pari.CoupleNames,
                    ClubName = pari.ClubName,
                    ClubTown = pari.ClubTown,
                    AgeGroup = pari.AgeGroup
                };
                _stulTulosWriter.TulosLines.Add(tulos);
            }

            _stulTulosWriter.WriteAll();
        }
    }
}
