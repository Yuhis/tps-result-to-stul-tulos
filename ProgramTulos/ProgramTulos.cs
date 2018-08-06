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
        private IDictionary<string, TPSEvent> _tpsEvents;
        private string _stulTulosFileName = "";

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

            // tulos.txt tiedoston nimi
            _stulTulosFileName = stulTulosFileName;

            _tpsEvents = new Dictionary<string, TPSEvent>();
        }

        public void WriteTulosTxt()
        {
            SetResults();

            if (0 == _tpsEvents.Count)
            {
                Console.WriteLine("Ei tuloksia");
                return;
            }

            bool multipleEvents = false;
            if (_tpsEvents.Count > 1)
            {
                multipleEvents = true;
            }

            foreach(KeyValuePair<string,TPSEvent> tpsEvent in _tpsEvents)
            {
                string filename = _stulTulosFileName;
                if(multipleEvents)
                {
                    filename = tpsEvent.Key + _stulTulosFileName;
                }
                WriteTulosTxtForEvent(tpsEvent.Value, filename);
            }
        }

        private void SetResults()
        {
            foreach(TPSResultsElement element in _tpsResultsReader.Event.Results)
            {
                if(!_tpsEvents.ContainsKey(element.EventCode))
                {
                    _tpsEvents[element.EventCode] = new TPSEvent(element.EventCode);
                }
                TPSEvent tpsEvent = _tpsEvents[element.EventCode];

                TPSCompetition tpsCompetition = tpsEvent.Competitions.SingleOrDefault(
                    c => c.CompetitionCode == element.CompetitionCode && c.CompetitionType == element.CompetitionType.ToString());
                if(null == tpsCompetition)
                {
                    tpsCompetition = new TPSCompetition(element.CompetitionCode);
                    tpsCompetition.CompetitionLevel = element.CompetitionLevel;
                    tpsCompetition.CompetitionType = element.CompetitionType.ToString(); // TODO: FixThis
                    tpsCompetition.TotalCouples = element.TotalCouples;
                    tpsEvent.Competitions.Add(tpsCompetition);
                }

                TPSCouple tpsCouple = tpsCompetition.Couples.SingleOrDefault(c => c.CoupleCode == element.CoupleCode);
                if(null == tpsCouple)
                {
                    tpsCouple = new TPSCouple(element.CoupleCode);
                    tpsCouple.CoupleNumber = element.CoupleNumber;
                    tpsCouple.CoupleNames = element.CoupleNames;
                    tpsCouple.Position1 = element.Position1;
                    tpsCouple.Position2 = element.Position2;
                    tpsCouple.Missing = (int)element.Missing; // TODO: FixThis
                    tpsCouple.RoundsDanced = element.RoundsDanced;
                    tpsCompetition.Couples.Add(tpsCouple);
                }

                int coupleRoundNbr = tpsCouple.RoundsDanced;
                TPSCompetitionRound round = tpsCompetition.Rounds.SingleOrDefault(r => r.RoundNumber == coupleRoundNbr);
                if(null == round)
                {
                    round = new TPSCompetitionRound(coupleRoundNbr);
                    tpsCompetition.Rounds.Add(round);
                }
                round.Couples.Add(tpsCouple);
            }
        }

        private void WriteTulosTxtForEvent(TPSEvent tpsEvent, string tulosTxtFileName)
        {
            string eventName = "(tyhjä)";
            if(!string.IsNullOrEmpty(tpsEvent.EventCode))
            {
                eventName = tpsEvent.EventCode;
            }
            Console.WriteLine($"Tapahtuma: {eventName}");
            Console.WriteLine($"Tulostiedosto: {tulosTxtFileName}");
            STULTulosWriter stulTulosWriter = new STULTulosWriter(tulosTxtFileName);

            foreach(TPSCompetition tpsCompetition in tpsEvent.Competitions)
            {
                foreach(TPSCompetitionRound round in tpsCompetition.Rounds)
                {
                    foreach(TPSCouple couple in round.Couples)
                    {
                        STULParitLine stulPari = _stulParitReader.ParitLines.SingleOrDefault(p => p.CoupleCode == couple.CoupleCode);
                        if(null == stulPari)
                        {
                            stulPari = new STULParitLine()
                            {
                                CoupleCode = couple.CoupleCode,
                                CoupleNames = couple.CoupleNames
                            };
                            Console.WriteLine($"Virhe: Paria {couple.CoupleNames} ei ole paritiedostossa");
                        }
                        string result = "";
                        if(couple.RoundsDanced == tpsCompetition.Rounds.Count)
                        {
                            // Final round
                            if(couple.RoundsDanced > 1)
                            {
                                result = $"{couple.Position1}/{round.Couples.Count}+{couple.RoundsDanced-1}";
                            }
                            else
                            {
                                result = $"{couple.Position1}/{round.Couples.Count}";
                            }
                        }
                        else
                        {
                            // Qualification round
                            result = $"+{couple.RoundsDanced}";
                        }
                        var tulos = new STULTulosLine()
                        {
                            CoupleCode = couple.CoupleCode,
                            CoupleNumber = couple.CoupleNumber,
                            CoupleNames = stulPari.CoupleNames,
                            ClubName = stulPari.ClubName,
                            ClubTown = stulPari.ClubTown,
                            AgeGroup = stulPari.AgeGroup,
                            Category = "",
                            Result1 = "",
                            Result2 = ""
                        };
                        if(tpsCompetition.CompetitionType == "Latin")
                        {
                            tulos.Result1 = result;
                        }
                        else
                        {
                            tulos.Result1 = result;
                        }
                    stulTulosWriter.TulosLines.Add(tulos);
                    }
                }
            }

            foreach(TPSCompetition comp in tpsEvent.Competitions)
            {
                Console.WriteLine($"Competition: {comp.CompetitionCode}");
                Console.WriteLine($"    level: {comp.CompetitionLevel}");
                Console.WriteLine($"     type: {comp.CompetitionType}");
                Console.WriteLine($"  couples: {comp.TotalCouples}");
                Console.WriteLine($"   rounds: {comp.Rounds.Count}");
                Console.WriteLine($"  couples: {comp.Couples.Count}");
            }

            stulTulosWriter.WriteAll();
        }
    }
}
