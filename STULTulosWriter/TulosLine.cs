using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.STULTulosWriter
{
    public class TulosLine
    {
        public string CoupleCode { get; set; } = "";
        public int CoupleNumber { get; set; } = 0;
        public string CoupleNames { get; set; } = "";
        public string ClubName { get; set; } = "";
        public string ClubTown { get; set; } = "";
        public string AgeGroup { get; set; } = "";
        public string Level { get; set; } = "";
        public string Result1 { get; set; } = "";
        public string Result2 { get; set; } = "";
    }
}
