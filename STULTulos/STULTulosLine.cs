using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper.Configuration;

namespace tps_result_to_stul_tulos.STULTulos
{
    public class STULTulosLine
    {
        public int CoupleCode { get; set; } = 0;
        public int CoupleNumber { get; set; } = 0;
        public string CoupleNames { get; set; } = "";
        public string ClubName { get; set; } = "";
        public string ClubTown { get; set; } = "";
        public string AgeGroup { get; set; } = "";
        public string Category { get; set; } = "";
        public string Result1 { get; set; } = "";
        public string Result2 { get; set; } = "";
    }

    public sealed class STULTulosLineClassMap : ClassMap<STULTulosLine>
    {
        public STULTulosLineClassMap()
        {
            Map(m => m.CoupleCode).Index(0);
            Map(m => m.CoupleNumber).Index(1);
            Map(m => m.CoupleNames).Index(2);
            Map(m => m.ClubName).Index(3);
            Map(m => m.ClubTown).Index(4);
            Map(m => m.AgeGroup).Index(5);
            Map(m => m.Category).Index(6);
            Map(m => m.Result1).Index(7);
            Map(m => m.Result2).Index(8);
        }
    }
}
