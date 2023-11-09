using Cultural_Heritage.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Web;

namespace Cultural_Heritage.Controllers
{
    
    public class assetsController : Controller
    {
        List<cultureassets> list;
        Login lgn = new Login();
        assets_services stud;


        public assetsController()
        { 
            String ConnectionString = "Server=" + "127.0.0.1" + ";Database=" +
                                      "cultureassets" + ";port=" + "3306" + ";User=" + "root" + ";password=" + "암호";
            stud = new assets_services(ConnectionString);
        }
        
        public IActionResult Index()
        {
            list = stud.Getassets();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        public ActionResult Createpost(IFormCollection form)
        {
            var num = Convert.ToInt32(form["num"]);
            var ca_type = form["ca_type"].ToString();
            var ca_name = form["ca_name"].ToString();
            var ca_addr = form["ca_addr"].ToString();
            var ca_period = form["ca_period"].ToString();
            DateTime ca_date = DateTime.ParseExact(form["ca_date"], "yyyyMMdd", CultureInfo.InvariantCulture);
            var ca_detail = form["ca_detail"].ToString();
            int result = stud.Createassets(num, ca_type, ca_name, ca_addr, ca_period, ca_date, ca_detail);
            TempData["result"] = result;
            return View();
        }

        public ActionResult Delete(int id)
        {
            var std = stud.Selectassets(id);
            return View(std);
        }

        public ActionResult Deletepost(int id)
        {
            int result = stud.Deleteassets(id);
            TempData["result"] = result;
            return View();
        }

        public ActionResult Update(int id)
        {
            var std = stud.Selectassets(id);
            return View(std);
        }
        [HttpPost]
        public ActionResult Updatepost(IFormCollection form)
        {
            var num = Convert.ToInt32(form["num"]);
            var ca_type = form["ca_type"].ToString();
            var ca_name = form["ca_name"].ToString();
            var ca_addr = form["ca_addr"].ToString();
            var ca_period = form["ca_period"].ToString();
            DateTime ca_date = DateTime.ParseExact(form["ca_date"], "yyyyMMdd", CultureInfo.InvariantCulture);
            var ca_detail = form["ca_detail"].ToString();

            int result = stud.Updateassets(num, ca_type, ca_name, ca_addr, ca_period, ca_date, ca_detail);
            TempData["result"] = result;
            return View();
        }

        public ActionResult Find(string ca_name)
        {

            var std = stud.SelectFindassets(ca_name);
            return View(std);
        }

        [HttpPost]
        public ActionResult Findpost(IFormCollection form)
        {
            var ca_name = form["ca_name"].ToString();
            var result = stud.SelectFindassets(ca_name);

            return View(result);

        }

        //로그인
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Loginpost(Login model)
        {
            Login admin = stud.SelectLogin(model.id);
            if (admin != null && admin.password == model.password)
            {
                /*TempData["SuccessMessage"] = "로그인에 성공했습니다.";*/
                return RedirectToAction("Index", "assets"); //일치하면 데이터 관리 출력
            }
            else
            {
                TempData["ErrorMessage"] = "잘못된 관리자 계정입니다.";
                return RedirectToAction("Login"); //오류 메세지 띄운 후 다시 로그인 창
            }
        }
    }
}




























/*DateTime ca_date = DateTime.MinValue;

            var inputDate = form["ca_date"].ToString();
            if (DateTime.TryParseExact(inputDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out ca_date))
            {
                var formattedDate = ca_date.ToString("yyyyMMdd");
                var ca_detail = form["ca_detail"].ToString();
                int result = stud.Updateassets(num, ca_type, ca_name, ca_addr, ca_period, ca_date, ca_detail);
                TempData["result"] = result;
                return View();
            }
            else
            {
                // Handle date parsing error
                ModelState.AddModelError("ca_date", "Invalid date format. Please use 'yyyy-MM-dd' format.");
                return View();
            }*/