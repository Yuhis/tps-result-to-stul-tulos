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

                TPSCompetitionType tpsCompetitionType;
                switch(element.CompetitionType)
                {
                    case(CompetitionType.TenDance):
                        tpsCompetitionType = TPSCompetitionType.TenDance;
                        break;
                    case(CompetitionType.Standard):
                        tpsCompetitionType = TPSCompetitionType.Standard;
                        break;
                    case(CompetitionType.Latin):
                        tpsCompetitionType = TPSCompetitionType.Latin;
                        break;
                    default:
                        Console.WriteLine($"Virhe: Tuntematon kilpailun tyyppi: {CompetitionType.TenDance}");
                        tpsCompetitionType = TPSCompetitionType.TenDance;
                        break;
                }
                TPSCompetition tpsCompetition = tpsEvent.Competitions.SingleOrDefault(
                    c => c.CompetitionCode == element.CompetitionCode && c.CompetitionType == tpsCompetitionType);
                if(null == tpsCompetition)
                {
                    tpsCompetition = new TPSCompetition(element.CompetitionCode, tpsCompetitionType);
                    tpsCompetition.CompetitionLevel = element.CompetitionLevel;
                    tpsCompetition.TotalCouples = element.TotalCouples;
                    tpsEvent.Competitions.Add(tpsCompetition);
                }
                // TODO: Else check for property consistency

                TPSMissing tpsMissing;
                switch(element.Missing)
                {
                    case Missing.NotMissing:
                        tpsMissing = TPSMissing.NotMissing;
                        break;
                    case Missing.Dancing:
                        tpsMissing = TPSMissing.Dancing;
                        break;
                    case Missing.Excused:
                        tpsMissing = TPSMissing.Excused;
                        break;
                    case Missing.Missing:
                        tpsMissing = TPSMissing.Missing;
                        break;
                    default:
                        Console.WriteLine($"Virhe: Tuntematon 'missing'arvo: {element.Missing}");
                        tpsMissing = TPSMissing.NotMissing;
                        break;
                }
                TPSCouple tpsCouple = new TPSCouple(element.CoupleCode);
                tpsCouple.CoupleNumber = element.CoupleNumber;
                tpsCouple.CoupleNames = element.CoupleNames;
                tpsCouple.Position1 = element.Position1;
                tpsCouple.Position2 = element.Position2;
                tpsCouple.Missing = tpsMissing;
                tpsCouple.RoundsDanced = element.RoundsDanced;

                int coupleRoundNbr = tpsCouple.RoundsDanced;
                TPSCompetitionRound round = tpsCompetition.Rounds.SingleOrDefault(r => r.RoundNumber == coupleRoundNbr);
                if(null == round)
                {
                    round = new TPSCompetitionRound(coupleRoundNbr);
                    tpsCompetition.Rounds.Add(round);
                }
                round.Couples.Add(tpsCouple);
            }

            foreach(KeyValuePair<string,TPSEvent> eventItem in _tpsEvents)
            {
                var EventCoupleResults = eventItem.Value.CoupleResults;
                foreach(TPSCompetition tpsCompetition in eventItem.Value.Competitions)
                {
                    foreach(TPSCompetitionRound round in tpsCompetition.Rounds)
                    {
                        foreach(TPSCouple couple in round.Couples)
                        {
                            CoupleResult coupleResult = EventCoupleResults.SingleOrDefault(
                                p => p.CoupleCode == couple.CoupleCode &&
                                     string.Equals(p.CompetitionCode, tpsCompetition.CompetitionCode)  &&
                                     p.CoupleNumber == couple.CoupleNumber);
                            if(null == coupleResult)
                            {
                                coupleResult = new CoupleResult(tpsCompetition.CompetitionCode, couple.CoupleCode, couple.CoupleNumber)
                                {
                                    CoupleNames = couple.CoupleNames
                                };
                                EventCoupleResults.Add(coupleResult);
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
                            if(tpsCompetition.CompetitionType == TPSCompetitionType.Latin)
                            {
                                if(!string.IsNullOrEmpty(coupleResult.Result2))
                                {
                                    Console.WriteLine($"Virhe: Parilla {coupleResult.CoupleCode} on jo tulos 2 {coupleResult.Result2}");
                                }
                                coupleResult.Result2 = result;
                            }
                            else
                            {
                                if(!string.IsNullOrEmpty(coupleResult.Result1))
                                {
                                    Console.WriteLine($"Virhe: Parilla {coupleResult.CoupleCode} on jo tulos 1 {coupleResult.Result1}");
                                }
                                coupleResult.Result1 = result;
                            }
                        }
                    }
                }
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

            foreach(CoupleResult coupleResult in tpsEvent.CoupleResults)
            {
                STULParitLine stulPari = _stulParitReader.ParitLines.SingleOrDefault(p => p.CoupleCode == coupleResult.CoupleCode);
                if(null == stulPari)
                {
                    stulPari = new STULParitLine()
                    {
                        CoupleCode = coupleResult.CoupleCode,
                        CoupleNames = coupleResult.CoupleNames // TODO: Convert format
                    };
                    Console.WriteLine($"Virhe: Paria {coupleResult.CoupleNames} ei ole STUL paritiedostossa");
                }

                var tulos = new STULTulosLine()
                {
                    CoupleCode = coupleResult.CoupleCode,
                    CoupleNumber = coupleResult.CoupleNumber,
                    CoupleNames = stulPari.CoupleNames,
                    ClubName = stulPari.ClubName,
                    ClubTown = stulPari.ClubTown,
                    AgeGroup = "", // TODO: Competition age group here
                    Category = "", // TODO: Competition category here
                    Result1 = coupleResult.Result1,
                    Result2 = coupleResult.Result2
                };
                stulTulosWriter.TulosLines.Add(tulos);
            }

            stulTulosWriter.WriteAll();
        }
    }
}
