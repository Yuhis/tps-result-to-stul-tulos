using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace tps_result_to_stul_tulos.TPSResults
{
    public class TPSResultsElement
    {
        public string EventCode { get; private set; } = "";
        public string CompetitionCode { get; private set; } = "";
        public string CompetitionLevel { get; private set; } = "";
        public CompetitionType CompetitionType { get; private set; } = CompetitionType.TenDance;
        public int TotalCouples { get; private set; } = 0;
        public int CoupleCode { get; private set; } = 0;
        public int CoupleNumber { get; private set; } = 0;
        public string CoupleNames { get; private set; } = "";
        public int Position1 { get; private set; } = 0;
        public int Position2 { get; private set; } = 0;
        public Missing Missing { get; set; } = Missing.NotMissing;
        public int RoundsDanced { get; set; } = 0;


        public TPSResultsElement(XElement xe)
        {
            EventCode = xe.Element("EventCode").Value;
            CompetitionCode = xe.Element("CompCode").Value;
            CompetitionLevel = xe.Element("CompLevel").Value;
            CompetitionType = MapCompetitionType(xe.Element("CompType").Value);
            TotalCouples = ParseInteger(xe.Element("TotalCouples").Value, "TotalCouples");
            CoupleCode = ParseInteger(xe.Element("CoupleCode").Value, "CoupleCode");
            CoupleNumber = ParseInteger(xe.Element("CoupleNumber").Value, "CoupleNumber");
            CoupleNames = xe.Element("Names").Value;
            Position1 = ParseInteger(xe.Element("Position1").Value, "Position1");
            Position2 = ParseInteger(xe.Element("Position2").Value, "Position2");
            Missing = MapMissing(xe.Element("Missing").Value);
            RoundsDanced = ParseInteger(xe.Element("RoundsDanced").Value, "RoundsDanced");
        }

        private int ParseInteger(string strValue, string elemName)
        {
            if(int.TryParse(strValue, out int v))
            {
                return v;
            }
            throw new ArgumentOutOfRangeException($"Tunnistamaton {elemName} elementin arvo: {strValue}");
        }

        private CompetitionType MapCompetitionType(string compType)
        {
            string s = " ";
            if(!string.IsNullOrEmpty(compType))
            {
                s = compType.ToUpper();
            }

            switch(s[0])
            {
                case 'S':
                    return CompetitionType.Standard;
                case 'L':
                    return CompetitionType.Latin;
                case '1':
                    return CompetitionType.TenDance;
                default:
                    throw new ArgumentOutOfRangeException($"Tunnistamaton CompType elementin arvo: {compType}");
            }
        }

        private Missing MapMissing(string missing)
        {
            switch(missing)
            {
                case "0":
                    return Missing.NotMissing;
                case "1":
                    return Missing.Dancing;
                case "2":
                    return Missing.Excused;
                case "3":
                    return Missing.Missing;
                default:
                    throw new ArgumentOutOfRangeException($"Tunnistamaton Missing elementin arvo: {missing}");
            }
        }
        
    }

    public enum CompetitionType
    {
        TenDance = 0,
        Standard,
        Latin
    }

    public enum Missing
    {
        NotMissing = 0,
        Dancing = 1,
        Excused = 2,
        Missing = 3
    }
}
