using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.ProgramTulos
{
    public class CoupleResult
    {
        public string CompetitionCode { get; private set; }
        public int CoupleCode { get; private set; }
        public int CoupleNumber { get; private set; }
        public string CoupleNames { get; set; } = "";
        public string Result1 { get; set; } = "";
        public string Result2 { get; set; } = "";
        public string Missing { get; set; } = "";

        public CoupleResult(string competitionCode, int coupleCode, int coupleNumber)
        {
            CompetitionCode = competitionCode;
            CoupleCode = coupleCode;
            CoupleNumber = coupleNumber;
        }
    }
}
