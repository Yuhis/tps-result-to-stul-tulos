using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.TPSParit
{
    public class TPSParitLine
    {
        public string CoupleCode { get; set; } = "";
        public int AreaCode { get; set; } = 0;
        public string CoupleNames { get; set; } = "";
        public string ClubName { get; set; } = "";
        public string ClubTown { get; set; } = "";
        public string AgeGroup { get; set; } = "";
        public string LevelStandard { get; set; } = "";
        public string LevelLatin { get; set; } = "";
    }
}
