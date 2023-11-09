using Cultural_Heritage.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cultural_Heritage.Controllers
{
    public class FindassetsController : Controller
    {
        List<cultureassets> list;
        assets_services stud;


        public FindassetsController()
        {
            String ConnectionString = "Server=" + "127.0.0.1" + ";Database=" +
                                      "cultureassets" + ";port=" + "3306" + ";User=" + "root" + ";password=" + "암호";
            stud = new assets_services(ConnectionString);
        }

        public IActionResult Index()
        {
            return View();
        }

        //주소부분검색
        public ActionResult Address(string ca_name)
        {
            var std = stud.SelectFindassets(ca_name);
            return View(std);
        }

        public ActionResult Period(string ca_name)
        {
            var std = stud.SelectFindassets(ca_name);
            return View(std);
        }

        public ActionResult Type(string ca_name)
        {
            var std = stud.SelectFindassets(ca_name);
            return View(std);
        }

        public ActionResult Name(string ca_name)
        {
            var std = stud.SelectFindassets(ca_name);
            return View(std);
        }

        //출력 부분
        public ActionResult FindAddress(IFormCollection form)
        {
            //var std = cual.SelectCa(no);
            //return View(std);
            var ca_addr = form["ca_addr"].ToString();
            List<cultureassets> list = stud.Findca_addr(ca_addr);
            return View("Findpost", list);
        }

        public ActionResult FindPeriod(IFormCollection form)
        {
            //var std = cual.SelectCa(no);
            //return View(std);
            var ca_period = form["ca_period"].ToString();
            List<cultureassets> list = stud.Findca_period(ca_period);
            return View("Findpost", list);
        }

        public ActionResult FindType(IFormCollection form)
        {
            //var std = cual.SelectCa(no);
            //return View(std);
            var ca_type = form["ca_type"].ToString();
            List<cultureassets> list = stud.Findca_type(ca_type);
            return View("Findpost", list);
        }

        public ActionResult FindName(IFormCollection form)
        {
            //var std = cual.SelectCa(no);
            //return View(std);
            var ca_name = form["ca_name"].ToString();
            List<cultureassets> list = stud.Findca_name(ca_name);
            return View("Findpost", list);
        }
    }
}
