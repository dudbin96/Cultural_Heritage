using Cultural_Heritage.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cultural_Heritage.Controllers
{
    public class LoginController : Controller
    {
        List<Login> list;
        assets_services stud;


        public LoginController()
        {
            String ConnectionString = "Server=" + "127.0.0.1" + ";Database=" +
                                      "cultureassets" + ";port=" + "3306" + ";User=" + "root" + ";password=" + "암호";
            stud = new assets_services(ConnectionString);
        }
        public IActionResult Index()
        {
            list = stud.GetLogin();
            return View(list);
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Loginpost(Login model)
        {
            Login admin = stud.SelectLogin(model.id);
            if (admin != null && admin.password == model.password)
            {

                return RedirectToAction("Index", "Login"); //일치하면 데이터 관리 출력
            }
            else
            {
                TempData["ErrorMessage"] = "잘못된 관리자 계정입니다.";
                return RedirectToAction("Login"); //오류 메세지 띄운 후 다시 로그인 창
            }
        }

        //회원가입
        public IActionResult Signup()
        {
            return View();
        }

        public ActionResult Signuppost(IFormCollection form)
        {
            var num = Convert.ToInt32(form["num"]);
            var id = form["id"].ToString();
            var password = form["password"].ToString();

            int result = stud.sign_up(num, id, password);
            TempData["result"] = result;
            return View();
        }
    }
}
