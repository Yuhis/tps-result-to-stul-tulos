using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.ProgramTulos
{
    public class TPSCouple
    {
        public int CoupleCode { get; private set; }
        public int CoupleNumber { get; set; } = 0;
        public string CoupleNames { get; set; } = "";
        public int Position1 { get; set; } = 0;
        public int Position2 { get; set; } = 0;
        public TPSMissing Missing { get; set; } = TPSMissing.NotMissing;
        public int RoundsDanced { get; set; } = 0;

        public TPSCouple(int coupleCode)
        {
            CoupleCode = coupleCode;
        }
    }

    public enum TPSMissing
    {
        NotMissing = 0,
        Dancing = 1,
        Excused = 2,
        Missing = 3
    }
}
