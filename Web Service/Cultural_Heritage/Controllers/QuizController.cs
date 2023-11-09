using Microsoft.AspNetCore.Mvc;
using Cultural_Heritage.Models;

namespace Cultural_Heritage.Controllers
{
    public class QuizController : Controller
    {
        List<Quiz> Qlist;
        assets_services stud;

        public QuizController()
        {
            String connString = "Server=" + "127.0.0.1" + ";Database=" + "cultureassets" + ";port=" + "3306" + ";User=" + "root" +
                ";password=" + "암호";
            stud = new assets_services(connString);//접속 문자열을 임마한테 넘겨준다 
        }

        /*private int GetRandomNonDuplicateQuizNumber()
        {
            Random ran = new Random();
            int num;

            do
            {
                num = ran.Next() % 5 + 1; // 랜덤한 문제 번호 생성
            } while (num == Convert.ToInt32(TempData["PreviousQuizNum"])); // 중복 문제인 경우 다시 생성

            return num;
        }*/

        public IActionResult Quiz()// 랜덤으로 숫자 줘서 selectQ로 문제 불러오기
        {
            Random ran = new Random();
            int num = ran.Next() % 10 + 1;
            var Q = stud.SelectQuiz(num);
            return View(Q);
        }
        [HttpPost]
        public ActionResult Answer(IFormCollection form)//사용자가 입력한 퀴즈 답을 데이터 베이스내에 정답과 비교하는 함수
        {
            string Qans = form["ans"].ToString();
            int num = Convert.ToInt32(form["quiznum"]);
            string Q = stud.GetAnswer(num); //폼에서 가저온 번호를 매개변수로 정답을 Q에 넣음

            var Qi = stud.SelectQuiz(num);
            string Qdetail = Qi.detail.ToString();
            int Qimg = Convert.ToInt32(Qi.img);

            int result = 1;
            if (Qans == Q)
            {
                result = 1;
                
            }
            else
            { 
                result = 0;


            }
            TempData["result"] = result;
            TempData["num"] = num;
            TempData["Qimg"] = Qimg;
            TempData["Qdetail"] = Qdetail;
            TempData["Q"] = Q;


            return View();

        }
    }
}
