using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using tps_result_to_stul_tulos.TPSResultsReader;

namespace tps_result_to_stul_tulos
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFileName = GetArgument(args, "-i", "results.xml");
            if(!File.Exists(inputFileName))
            {
                Console.WriteLine($"Virhe: Tiedostoa {inputFileName} ei löydy.");
                Environment.Exit(-1);
            }
            Console.WriteLine($"Input file: {inputFileName}");
            string outputFileName = GetArgument(args, "-o", "tulos.txt");
            Console.WriteLine($"Output file: {outputFileName}");

            ResultsReader reader = new ResultsReader(inputFileName);

        }

        static string GetArgument(IEnumerable<string> args, string option, string defaultValue)
            => args.SkipWhile(i => i != option).Skip(1).Take(1).FirstOrDefault() ?? defaultValue;
    }
}
