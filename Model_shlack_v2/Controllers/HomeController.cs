using System;
using System.Web.Mvc;
using Model_shlack_v2.Models;
using Excel = Microsoft.Office.Interop.Excel;

namespace Model_shlack_v2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            shlak_raschet shlak_model = new shlak_raschet();
            ViewBag.index = shlak_model;
            return View();
        }
        public ActionResult EnterVoid(double CaO, double SiO2, double Al2O3, double MgO)
        {

            #region Расчеты
            shlak_raschet raschet = new shlak_raschet(CaO, SiO2, Al2O3, MgO);
            string xlsname = "shlak2.xlsx";
            string path = Server.MapPath("~/Content/" + xlsname);
            Excel.Application app = new Excel.Application();
            Excel.Workbook wkbook = app.Workbooks.Open(path);
            Excel.Worksheet wksheet = wkbook.Sheets[1];
            Excel.Worksheet wksheet2 = wkbook.Sheets[7];
            Excel.Worksheet wksheet3 = wkbook.Sheets[8];
            wksheet.Range["C3"].Value = raschet.CaO;
            wksheet.Range["C4"].Value = raschet.SiO2;
            wksheet.Range["C5"].Value = raschet.Al2O3;
            wksheet.Range["C6"].Value = raschet.MgO;
            //вязкость шлака при температуре 1400
            double pseudocoef1_1400 = double.Parse(wksheet2.Range["B157"].Value.ToString());
            double pseudocoef2_1400 = double.Parse(wksheet2.Range["C157"].Value.ToString());
            double pseudocoef3_1400 = double.Parse(wksheet2.Range["D157"].Value.ToString());
            //берем коэфициенты из расчета шлака при 1500 с поправкой на глинозем
            double pseudocoef1_1500 = double.Parse(wksheet3.Range["B157"].Value.ToString());
            double pseudocoef2_1500 = double.Parse(wksheet3.Range["C157"].Value.ToString());
            double pseudocoef3_1500 = double.Parse(wksheet3.Range["D157"].Value.ToString());
            //расчитываем полиномы
            double vis1400_ras = pseudocoef3_1400 + pseudocoef2_1400 * raschet.Al2O3 + pseudocoef1_1400 * Math.Pow(raschet.Al2O3, 2);
            double vis1500_ras = pseudocoef3_1500 + pseudocoef2_1500 * raschet.Al2O3 + pseudocoef1_1500 * Math.Pow(raschet.Al2O3, 2);
            double coefB_ras = -0.01 * (Math.Log10(Math.Log10(Math.Abs(vis1400_ras))) - Math.Log10(Math.Log10(Math.Abs(vis1500_ras))));
            double coefA_ras = Math.Log10(Math.Log10(Math.Abs(vis1500_ras))) - 1500 * coefB_ras;
            double Vis1350RasEx = Math.Pow(10, Math.Pow(10, coefA_ras + coefB_ras * 1350));
            double vis1450RasEx = Math.Pow(10, Math.Pow(10, coefA_ras + coefB_ras * 1450));
            double vis1550RasEx = Math.Pow(10, Math.Pow(10, coefA_ras + coefB_ras * 1550));
            double temp7puaz_ras = (coefA_ras - Math.Log10(Math.Log10(7))) / -coefB_ras;
            double tempPlav25puaz_ras = (coefA_ras - Math.Log10(Math.Log10(25))) / -coefB_ras;
            double gradVisc = (25 - 7) / (temp7puaz_ras - tempPlav25puaz_ras);
            double gradViscCels = vis1400_ras - vis1500_ras;
            #endregion Расчеты
            raschet.OtnCaOSiO2 = CaO / SiO2;
            raschet.OtnCaMgOnaSiO = (CaO + MgO) / SiO2;
            raschet.OtnCaOMgOnaSiO2Al2O3 = (CaO + MgO) / (SiO2 + Al2O3);
            raschet.Viscosity1350 = Vis1350RasEx;
            raschet.Viscosity1400 = vis1400_ras;
            raschet.Viscosity1450 = vis1450RasEx;
            raschet.Viscosity1500 = vis1500_ras;
            raschet.Viscosity1550 = vis1550RasEx;
            raschet.TempVisc25puaz = tempPlav25puaz_ras;
            raschet.TempVisc7puaz = temp7puaz_ras;
            raschet.GradVisc7_25puaz = gradVisc;
            raschet.GradVisc1400_1500grad = gradViscCels;
            wkbook.Save();
            wkbook.Close(true, Type.Missing, Type.Missing);
            app.Quit();
            ViewBag.result = raschet;
            return View();
        }

        public ActionResult About()
        {
            ViewData["Univeristy"] = "Уральский Федеральный университет";
            ViewData["Kafedra"] = "Кафедра: Информационные системы и технологии в металлургии";
            ViewData["Message"] = "Дисциплина: Моделирование процессов и объектов АСУТП";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Наши контакты";

            return View();
        }
    }
}