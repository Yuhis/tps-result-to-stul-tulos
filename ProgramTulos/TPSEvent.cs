using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.ProgramTulos
{
    public class TPSEvent
    {
        public string EventCode { get; private set; }
        public IList<TPSCompetition> Competitions { get; private set; }
        public IList<CoupleResult> CoupleResults { get; private set; }

        public TPSEvent(string eventCode)
        {
            EventCode = eventCode;
            Competitions = new List<TPSCompetition>();
            CoupleResults = new List<CoupleResult>();
        }
    }
}
