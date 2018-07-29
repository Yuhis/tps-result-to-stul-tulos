using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using tps_result_to_stul_tulos.STULParit;
using tps_result_to_stul_tulos.TPSParit;

namespace tps_result_to_stul_tulos.ProgramParit
{
    public class ProgramParit
    {
        private STULParitReader _stulParitReader;
        private TPSParitWriter _tpsParitWriter;

        public ProgramParit(string stulParitFileName, string tpsParitFileName)
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

            // parit.tps.txt tiedosto
            Console.WriteLine($"Parit.tps tiedosto: {tpsParitFileName}");
            _tpsParitWriter = new TPSParitWriter(tpsParitFileName);
        }

        public void WriteTPSParit()
        {
            foreach(STULParitLine pari in _stulParitReader.ParitLines)
            {
                var tps = new TPSParitLine(){
                    FirstNameMan = pari.ManFirstName,
                    LastNameMan = pari.ManLastName,
                    FirstNameWoman = pari.WomanFirstName,
                    LastNameWoman = pari.WomanLastName,
                    Club = $"{pari.ClubName} / {pari.ClubTown}",
                    MinMan = pari.CoupleCode,
                    MinWoman = pari.CoupleCode
                };
                _tpsParitWriter.ParitLines.Add(tps);
            }

            _tpsParitWriter.WriteAll();
        }
    }
}
