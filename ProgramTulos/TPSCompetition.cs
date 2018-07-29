using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.ProgramTulos
{
    public class TPSCompetition
    {
        public string CompetitionCode { get; private set; }
        public string CompetitionLevel { get; set; } = "";
        public string CompetitionType { get; set; } = "";
        public int TotalCouples { get; set; } = 0;
        public IList<TPSCompetitionRound> Rounds { get; private set; }
        public IList<TPSCouple> Couples { get; private set; }

        public TPSCompetition(string competitionCode)
        {
            CompetitionCode = competitionCode;
            Rounds = new List<TPSCompetitionRound>();
            Couples = new List<TPSCouple>();
        }
    }

    public class TPSCompetitionRound
    {
        public int RoundNumber { get; private set; }
        public IList<TPSCouple> Couples { get; private set; }

        public TPSCompetitionRound(int roundNumber)
        {
            RoundNumber = roundNumber;
            Couples = new List<TPSCouple>();
        }
    }
}
