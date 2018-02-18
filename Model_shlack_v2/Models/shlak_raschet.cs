using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model_shlack_v2.Models
{
    public class shlak_raschet
    {
        public double CaO { get; set; } = 40;
        public double SiO2 { get; set; } = 32.8071;
        public double Al2O3 { get; set; } = 11.2756;
        public double MgO { get; set; } = 8.5605;


        public double OtnCaOSiO2 { get; set; }
        public double OtnCaMgOnaSiO { get; set; }
        public double OtnCaOMgOnaSiO2Al2O3 { get; set; }
        public double Viscosity1350 { get; set; }
        public double Viscosity1400 { get; set; }
        public double Viscosity1450 { get; set; }
        public double Viscosity1500 { get; set; }
        public double Viscosity1550 { get; set; }
        public double TempVisc25puaz { get; set; }
        public double TempVisc7puaz { get; set; }
        public double GradVisc7_25puaz { get; set; }
        public double GradVisc1400_1500grad { get; set; }
        public double KoefA { get; set; }
        public double KoefB { get; set; }


        public shlak_raschet()
        {
        }


        public shlak_raschet(double CaO, double SiO2, double Al2O3, double MgO)
        {
            this.CaO = CaO;
            this.SiO2 = SiO2;
            this.Al2O3 = Al2O3;
            this.MgO = MgO;
        }

    }
}