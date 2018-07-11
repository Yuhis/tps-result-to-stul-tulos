using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using tps_result_to_stul_tulos.TPSResults;
using tps_result_to_stul_tulos.STULParit;
using tps_result_to_stul_tulos.TPSParit;
using tps_result_to_stul_tulos.STULTulos;

namespace tps_result_to_stul_tulos
{
    class Program
    {
        static void Main(string[] args)
        {
            // result.xml tiedosto
            string resultFileName = GetArgument(args, "-r", "results.xml");
            if(!File.Exists(resultFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {resultFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Results tiedosto: {resultFileName}");
            ResultsReader resultsReader = new ResultsReader(resultFileName);

            // parit.txt tiedosto
            string paritFileName = GetArgument(args, "-p", "parit.txt");
            if(!File.Exists(paritFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {paritFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Parit tiedosto: {paritFileName}");
            STULParitReader paritReader = new STULParitReader(paritFileName);

            // parit.tps.txt tiedosto
            string tpsParitFileName = GetArgument(args, "-s", "parit.tps.txt");
            Console.WriteLine($"Parit.tps tiedosto: {tpsParitFileName}");
            TPSParitWriter tpsParitWriter = new TPSParitWriter(tpsParitFileName);

            // tulos.txt tiedosto
            string tulosFileName = GetArgument(args, "-t", "tulos.txt");
            Console.WriteLine($"Tulos tiedosto: {tulosFileName}");
            STULTulosWriter tulosWriter = new STULTulosWriter(tulosFileName);

            // -------------
            foreach(STULParitLine pari in paritReader.ParitLines)
            {
                var tulos = new STULTulosLine(){
                    CoupleCode = pari.CoupleCode,
                    CoupleNames = pari.CoupleNames,
                    ClubName = pari.ClubName,
                    ClubTown = pari.ClubTown,
                    AgeGroup = pari.AgeGroup
                };
                tulosWriter.TulosLines.Add(tulos);

                var tps = new TPSParitLine(){
                    FirstNameMan = "",
                    LastNameMan = "",
                    FirstNameWoman = "",
                    LastNameWoman = "",
                    Club = $"{pari.ClubName} / {pari.ClubTown}",
                    MinMan = pari.CoupleCode,
                    MinWoman = pari.CoupleCode
                };
                tpsParitWriter.ParitLines.Add(tps);
            }

            tulosWriter.WriteAll();
            tpsParitWriter.WriteAll();
        }

        static string GetArgument(IEnumerable<string> args, string option, string defaultValue)
            => args.SkipWhile(i => i != option).Skip(1).Take(1).FirstOrDefault() ?? defaultValue;
    }
}
