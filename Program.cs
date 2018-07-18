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
using tps_result_to_stul_tulos.ProgramParit;
using tps_result_to_stul_tulos.ProgramTulos;
using McMaster.Extensions.CommandLineUtils;

namespace tps_result_to_stul_tulos
{
    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "tps-result-to-stul-tulos",
                Description = "TPS-STUL tiedostokonversiot"
            };
            app.HelpOption(inherited: true);
            
            app.Command("parit", paritCmd =>
            {
                paritCmd.Description = "STUL parit.txt tiedoston muunnos parit.tps.txt muotoon";
                var optionParitTxt = paritCmd.Option("-p|--parit <parit.txt>", "STUL parit.txt tiedoston nimi", CommandOptionType.SingleValue)
                    .Accepts(v => v.ExistingFile());
                var optionParitTpsTxt = paritCmd.Option("-t|--tps <parit.tps.txt>", "parit.tps.txt tiedoston nimi", CommandOptionType.SingleValue);
                
                paritCmd.OnExecute(() =>
                {
                    var paritTiedosto = optionParitTxt.HasValue()
                        ? optionParitTxt.Value()
                        : "parit.txt";
                    var paritTpsTiedosto = optionParitTpsTxt.HasValue()
                        ? optionParitTpsTxt.Value()
                        : "parit.tps.txt";

                    var program = new ProgramParit.ProgramParit(paritTiedosto, paritTpsTiedosto);
                    return 1;
                });
            });

            app.Command("tulos", tulosCmd =>
            {
                tulosCmd.Description = "TPS result.xml tiedoston muunnos STUL tulos.txt muotoon";
                var optionParitTxt = tulosCmd.Option("-p|--parit <parit.txt>", "STUL parit.txt tiedoston nimi", CommandOptionType.SingleValue)
                    .Accepts(v => v.ExistingFile());
                var optionResultsXml = tulosCmd.Option("-r|--results <results.xml>", "TPS results.xml tiedoston nimi", CommandOptionType.SingleValue)
                    .Accepts(v => v.ExistingFile());
                var optionTulosTxt = tulosCmd.Option("-t|--tulos <tulos.txt>", "STUL tulos.txt tiedoston nimi", CommandOptionType.SingleValue);
                
                tulosCmd.OnExecute(() =>
                {
                    var paritTiedosto = optionParitTxt.HasValue()
                        ? optionParitTxt.Value()
                        : "parit.txt";
                    var resultsTiedosto = optionResultsXml.HasValue()
                        ? optionResultsXml.Value()
                        : "results.xml";
                    var tulosTiedosto = optionTulosTxt.HasValue()
                        ? optionTulosTxt.Value()
                        : "tulos.txt";

                    var program = new ProgramTulos.ProgramTulos(paritTiedosto, resultsTiedosto, tulosTiedosto);
                    return 1;
                });
            });

            app.OnExecute(() =>
            {
                Console.WriteLine("Anna komento");
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);
        }

        public void OldMain(string[] args)
        {
            // result.xml tiedosto
            string resultFileName = "results.xml";
            if(!File.Exists(resultFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {resultFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Results tiedosto: {resultFileName}");
            ResultsReader resultsReader = new ResultsReader(resultFileName);

            // parit.txt tiedosto
            string paritFileName = "parit.txt";
            if(!File.Exists(paritFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {paritFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Parit tiedosto: {paritFileName}");
            STULParitReader paritReader = new STULParitReader(paritFileName);
            foreach(string e in paritReader.LineErrors){
                Console.WriteLine($"Virhe: {e}");
            }

            // parit.tps.txt tiedosto
            string tpsParitFileName = "parit.tps.txt";
            Console.WriteLine($"Parit.tps tiedosto: {tpsParitFileName}");
            TPSParitWriter tpsParitWriter = new TPSParitWriter(tpsParitFileName);

            // tulos.txt tiedosto
            string tulosFileName = "tulos.txt";
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
                    FirstNameMan = pari.ManFirstName,
                    LastNameMan = pari.ManLastName,
                    FirstNameWoman = pari.WomanFirstName,
                    LastNameWoman = pari.WomanLastName,
                    Club = $"{pari.ClubName} / {pari.ClubTown}",
                    MinMan = pari.CoupleCode,
                    MinWoman = pari.CoupleCode
                };
                tpsParitWriter.ParitLines.Add(tps);
            }

            tulosWriter.WriteAll();
            tpsParitWriter.WriteAll();
        }
    }
}
