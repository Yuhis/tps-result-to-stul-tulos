using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper.Configuration;

namespace tps_result_to_stul_tulos.STULParit
{
    public class STULParitLine
    {
        public int CoupleCode { get; set; } = 0;
        public int AreaCode { get; set; } = 0;
        public string CoupleNames { get; set; } = "";
        public string ClubName { get; set; } = "";
        public string ClubTown { get; set; } = "";
        public string AgeGroup { get; set; } = "";
        public string CategoryStandard { get; set; } = "";
        public string CategoryLatin { get; set; } = "";
    }

    public sealed class STULParitLineClassMap : ClassMap<STULParitLine>
    {
        public STULParitLineClassMap()
        {
            Map(m => m.CoupleCode).Index(0);
            Map(m => m.AreaCode).Index(1);
            Map(m => m.CoupleNames).Index(2);
            Map(m => m.ClubName).Index(3);
            Map(m => m.ClubTown).Index(4);
            Map(m => m.AgeGroup).Index(5);
            Map(m => m.CategoryStandard).Index(6);
            Map(m => m.CategoryLatin).Index(7);
        }
    }
}
