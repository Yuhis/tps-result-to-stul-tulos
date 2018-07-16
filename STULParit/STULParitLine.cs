using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using CsvHelper.Configuration;

namespace tps_result_to_stul_tulos.STULParit
{
    public class STULParitLine
    {
        private string coupleNamesPattern = @"((?<mf>\S+)\s+){0,1}(?<ml>\S+)\s+-\s+((?<wf>\S+)\s+){0,1}(?<wl>\S+)\s*";
        private string coupleNames;

        public int CoupleCode { get; set; } = 0;
        public int AreaCode { get; set; } = 0;
        public string CoupleNames {
             get { return coupleNames; }
             set { SetNames(value); coupleNames = value; } }
        public string ClubName { get; set; } = "";
        public string ClubTown { get; set; } = "";
        public string AgeGroup { get; set; } = "";
        public string CategoryStandard { get; set; } = "";
        public string CategoryLatin { get; set; } = "";
        public string ManFirstName { get; private set; } = "";
        public string ManLastName { get; private set; } = "";
        public string WomanFirstName { get; private set; } = "";
        public string WomanLastName { get; private set; } = "";
        public IList<string> Errors { get; private set;} = new List<string>();

        private void SetNames(string couple)
        {
            Match m = Regex.Match(couple, coupleNamesPattern);
            if(!m.Success){
                SetError($"Puuttuva tai virheellinen nimi parilla nro {CoupleCode}");
                return;
            }
            ManFirstName = m.Groups["mf"].Value;
            ManLastName = m.Groups["ml"].Value;
            WomanFirstName = m.Groups["wf"].Value;
            WomanLastName = m.Groups["wl"].Value;
            if(string.IsNullOrWhiteSpace(ManFirstName) || string.IsNullOrWhiteSpace(WomanFirstName)){
                SetError($"Puuttuva etunimi parilla nro {CoupleCode}");
            }
        }

        private void SetError( string error)
        {
            Errors.Add(error);
            return;
        }
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
