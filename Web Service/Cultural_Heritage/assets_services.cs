using MySql.Data.MySqlClient;
using Cultural_Heritage.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Linq;
using Cultural_Heritage.Models;

namespace Cultural_Heritage
{
    public class assets_services
    {
        public string ConnectionString { get; set; }

        public assets_services(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        private MySqlConnection GetSqlConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
        public List<cultureassets> Getassets()
        {
            List<cultureassets> list = new List<cultureassets>();
            /*string SQL = "SELECT *FROM assets ORDER BY num ASC";*/
            //프로시저 이용
            string SQL = "call cultureassets.Getassets_SP();"; 
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new cultureassets
                        {                          
                            num = Convert.ToInt32(reader["num"]),
                            ca_type = reader["ca_type"].ToString(),
                            ca_name = reader["ca_name"].ToString(),
                            ca_addr = reader["ca_addr"].ToString(),
                            ca_period = reader["ca_period"].ToString(),
                            ca_date = Convert.ToDateTime(reader["ca_date"]),
                            ca_detail = reader["ca_detail"].ToString()
                        });
                    }
                }
                connection.Close();
            }
            return list;
        }

        public cultureassets Selectassets(int num) 
        { 
            cultureassets assets = new cultureassets();
            /*string SQL = "SELECT *FROM assets WHERE num = '" + num + "' ";*/
            //프로시저 이용
            string SQL = "call cultureassets.Selectassets_SP('" + num + "');";
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        assets.num = Convert.ToInt32(reader["num"]);
                        assets.ca_type = reader["ca_type"].ToString();
                        assets.ca_name = reader["ca_name"].ToString();
                        assets.ca_addr = reader["ca_addr"].ToString();
                        assets.ca_period = reader["ca_period"].ToString();
                        assets.ca_date = Convert.ToDateTime(reader["ca_date"]);
                        assets.ca_detail = reader["ca_detail"].ToString();
                    }
                }
                connection.Close();
            }
            return assets;
        }

        public int Createassets(int num, string ca_type, string ca_name, string ca_addr, string ca_period, DateTime ca_date, string ca_detail)
        {
            /*string SQL = "INSERT INTO assets (num, ca_type, ca_name, ca_addr, ca_period, ca_date, ca_detail)" +
                         "VALUES(@num, @ca_type, @ca_name, @ca_addr, @ca_period, @ca_date, @ca_detail)";*/
            //프로시저 이용
            string SQL = "call cultureassets.Createassets_SP('" + num + "', '"+ ca_type + "','"+ ca_name + "','"+ ca_addr + "','"+ ca_period + "','"+ ca_date.ToString("yyyyMMdd") + "','"+ ca_detail + "');";
            using (MySqlConnection connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, connection);
                    cmd.Parameters.AddWithValue("p_num", num);
                    cmd.Parameters.AddWithValue("p_ca_type", ca_type);
                    cmd.Parameters.AddWithValue("p_ca_name", ca_name);
                    cmd.Parameters.AddWithValue("p_ca_addr", ca_addr);
                    cmd.Parameters.AddWithValue("p_ca_period", ca_period);
                    cmd.Parameters.AddWithValue("p_ca_date", ca_date);
                    cmd.Parameters.AddWithValue("p_ca_detail", ca_detail);
                    if(cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("입력 성공");
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("입력 실패");
                        return 0;
                    }                  
                }
                catch(Exception ex)
                {
                    Console.WriteLine("데이터 베이스 접속 실패");
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
            return 0;
        }

        public int Deleteassets(int num)
        {
            /*string SQL = "DELETE FROM assets WHERE num = '" + num + "'";*/
            //프로시저 이용
            string SQL = "call cultureassets.Deleteassets_SP('" + num + "');";
            using (MySqlConnection connection = GetSqlConnection())
            {
                try
                {
                    connection.Open();
                    MySqlCommand cmd = new MySqlCommand(SQL, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("삭제 성공");
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("삭제 실패");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("데이터 베이스 접속 실패");
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
            return 0;
        }

        public int Updateassets(int num, string ca_type, string ca_name, string ca_addr, string ca_period, DateTime ca_date, string ca_detail)
        {
            //양식의 값을 SQL 쿼리에 문자열로 직접 삽입하는 방법이기 때문에 DateTime 같은 경우 삽입하기 어려워지는 듯
            /*string SQL = "UPDATE assets SET num = '" + num + "', ca_type = '" + ca_type + "', ca_name = '" + ca_name + "', ca_addr = '" + ca_addr + "'" +
                         ", ca_period = '" + ca_period + "', ca_date = '" + ca_date + "', ca_detail = '" + ca_detail + "' " +
                         "where num = '" + num + "'";*/

            /*string SQL = "UPDATE assets SET num = @num, ca_type = @ca_type, ca_name = @ca_name, ca_addr = @ca_addr, ca_period = @ca_period, " +
                         "ca_date = @ca_date, ca_detail = @ca_detail WHERE num = @num";*/
            //프로시저 이용
            string SQL = "call cultureassets.Updateassets_SP('" + num + "', '"+ ca_type + "','"+ ca_name + "','"+ ca_addr + "','"+ ca_period + "','"+ ca_date.ToString("yyyyMMdd") + "','"+ ca_detail + "');";


            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand(SQL, connection);
                    cmd.Parameters.AddWithValue("num", num);
                    cmd.Parameters.AddWithValue("ca_type", ca_type);
                    cmd.Parameters.AddWithValue("ca_name", ca_name);
                    cmd.Parameters.AddWithValue("ca_addr", ca_addr);
                    cmd.Parameters.AddWithValue("ca_period", ca_period);
                    cmd.Parameters.AddWithValue("ca_date", ca_date);
                    cmd.Parameters.AddWithValue("ca_detail", ca_detail);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("수정 성공");
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("수정 실패");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("데이터 베이스 접속 실패");
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
            return 0;
        }

        public cultureassets Findassets(string ca_name)
        {
            cultureassets assets = new cultureassets();
            /*string SQL = "SELECT * FROM assets where ca_name= '" + ca_name + "'";*/
            //프로시저 이용
            string SQL = "call cultureassets.Findassets_SP('" + ca_name + "');";
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        assets.num = Convert.ToInt32(reader["num"]);
                        assets.ca_type = reader["ca_type"].ToString();
                        assets.ca_name = reader["ca_name"].ToString();
                        assets.ca_addr = reader["ca_addr"].ToString();
                        assets.ca_period = reader["ca_period"].ToString();
                        assets.ca_date = Convert.ToDateTime(reader["ca_date"]);
                        assets.ca_detail = reader["ca_detail"].ToString();

                    }
                }
                connection.Close();
            }
            return assets;
        }
        //단일 출력
        public cultureassets SelectFindassets(string ca_name)//Detail 
        {
            cultureassets assets = new cultureassets();
            /*string SQL = "select * from assets where ca_name= '" + ca_name + "'";*/
            //프로시저 이용
            string SQL = "call cultureassets.SelectFindassets_SP('" + ca_name + "');";
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        assets.num = Convert.ToInt32(reader["num"]);
                        assets.ca_type = reader["ca_type"].ToString();
                        assets.ca_name = reader["ca_name"].ToString();
                        assets.ca_addr = reader["ca_addr"].ToString();
                        assets.ca_period = reader["ca_period"].ToString();
                        assets.ca_date = Convert.ToDateTime(reader["ca_date"]);
                        assets.ca_detail = reader["ca_detail"].ToString();

                    }
                }
                connection.Close();
            }
            return assets;
        }

        //다중 출력 및 부분 출력
        /*public List<cultureassets> MultiSelectassets(string ca_name)
        {
            List<cultureassets> assetsList = new List<cultureassets>();
            *//*string SQL = "select * from dept_emp where emp_no like = '%' @id '%';";*//*
            string SQL = "SELECT * FROM dept_emp WHERE emp_no LIKE CONCAT('%', @ca_name, '%');";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                cmd.Parameters.AddWithValue("@ca_name", ca_name);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        assetsList.Add(new cultureassets
                        {
                            num = Convert.ToInt32(reader["num"]),
                            ca_type = reader["ca_type"].ToString(),
                            ca_name = reader["ca_name"].ToString(),
                            ca_addr = reader["ca_addr"].ToString(),
                            ca_period = reader["ca_period"].ToString(),
                            ca_date = Convert.ToDateTime(reader["ca_date"]),
                            ca_detail = reader["ca_detail"].ToString()
                        });
                    }
                }
                conn.Close();
            }
            return assetsList;
        }*/



        /// <summary>
        /// 여기부터 로그인

        public Login SelectLogin(string id)
        {
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                string SQL = "SELECT * FROM login WHERE id = @id";
                MySqlCommand cmd = new MySqlCommand(SQL, connection);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Login
                        {
                            id = reader["id"].ToString(),
                            password = reader["password"].ToString()
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public List<Login> GetLogin()
        {
            List<Login> list = new List<Login>();
            //string SQL = "SELECT *FROM assets ORDER BY num ASC";
            //프로시저 이용
            string SQL = "call cultureassets.GetLogin_SP();";
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, connection);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Login
                        {
                            /*num = Convert.ToInt32(reader["num"]),*/
                            id = reader["id"].ToString(),
                            password = reader["password"].ToString()
                        });
                    }
                }
                connection.Close();
            }
            return list;
        }

        //회원가입
        public int sign_up(int num, string id, string password)
        {
            Login login = new Login();
            string SQL = "call cultureassets.Login_SP('" + num + "', '" + id + "', '" + password + "' );";
            using (MySqlConnection connection = GetSqlConnection())
            {
                connection.Open();
                try
                {
                    MySqlCommand cmd = new MySqlCommand(SQL, connection);
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        Console.WriteLine("회원가입 성공");
                        return 1;
                    }
                    else
                    {
                        Console.WriteLine("회원가입 실패");
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("데이터 베이스 접속 실패");
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
            return 0;
        }






        // 준형님꺼 추가 부분 //
        //이름 부분 검색//
        public List<cultureassets> Findca_name(string whatever)// 특정 문자 포함 검색 , 아직진행중 !!!!
        {
            List<cultureassets> emp = new List<cultureassets>();
            string SQL = "call cultureassets.Findca_name_SP('"+whatever+"');";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                //cmd.Parameters.AddWithValue("whatever", ca_name);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emp.Add(new cultureassets()
                        {

                            num = Convert.ToInt32(reader["num"]),
                            ca_type = reader["ca_type"].ToString(),
                            ca_name = reader["ca_name"].ToString(),
                            ca_addr = reader["ca_addr"].ToString(),
                            ca_period = reader["ca_period"].ToString(),
                            ca_date = Convert.ToDateTime(reader["ca_date"]),
                            ca_detail = reader["ca_detail"].ToString()
                        });

                    }
                }
                conn.Close();
            }
            return emp;
        }

        //종목 부분 검색
        public List<cultureassets> Findca_type(string whatever)// 특정 문자 포함 검색 , 아직진행중 !!!!
        {
            List<cultureassets> emp = new List<cultureassets>();
            string SQL = "call cultureassets.Findca_type_SP('" + whatever + "');";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                //cmd.Parameters.AddWithValue("whatever", ca_name);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emp.Add(new cultureassets()
                        {

                            num = Convert.ToInt32(reader["num"]),
                            ca_type = reader["ca_type"].ToString(),
                            ca_name = reader["ca_name"].ToString(),
                            ca_addr = reader["ca_addr"].ToString(),
                            ca_period = reader["ca_period"].ToString(),
                            ca_date = Convert.ToDateTime(reader["ca_date"]),
                            ca_detail = reader["ca_detail"].ToString()
                        });

                    }
                }
                conn.Close();
            }
            return emp;
        }

        //소재지 부분 검색
        public List<cultureassets> Findca_addr(string whatever)// 특정 문자 포함 검색 , 아직진행중 !!!!
        {
            List<cultureassets> emp = new List<cultureassets>();
            string SQL = "call cultureassets.Findca_addr_SP('" + whatever + "');";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                //cmd.Parameters.AddWithValue("whatever", ca_name);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emp.Add(new cultureassets()
                        {

                            num = Convert.ToInt32(reader["num"]),
                            ca_type = reader["ca_type"].ToString(),
                            ca_name = reader["ca_name"].ToString(),
                            ca_addr = reader["ca_addr"].ToString(),
                            ca_period = reader["ca_period"].ToString(),
                            ca_date = Convert.ToDateTime(reader["ca_date"]),
                            ca_detail = reader["ca_detail"].ToString()
                        });

                    }
                }
                conn.Close();
            }
            return emp;
        }

        //시대 부분 검색
        public List<cultureassets> Findca_period(string whatever)// 특정 문자 포함 검색 , 아직진행중 !!!!
        {
            List<cultureassets> emp = new List<cultureassets>();
            string SQL = "call cultureassets.Findca_period_SP('" + whatever + "');";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                //cmd.Parameters.AddWithValue("whatever", ca_name);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        emp.Add(new cultureassets()
                        {

                            num = Convert.ToInt32(reader["num"]),
                            ca_type = reader["ca_type"].ToString(),
                            ca_name = reader["ca_name"].ToString(),
                            ca_addr = reader["ca_addr"].ToString(),
                            ca_period = reader["ca_period"].ToString(),
                            ca_date = Convert.ToDateTime(reader["ca_date"]),
                            ca_detail = reader["ca_detail"].ToString()
                        });

                    }
                }
                conn.Close();
            }
            return emp;
        }

        //퀴즈 부분
        public List<Quiz> GetQuiz()
        {
            List<Quiz> list = new List<Quiz>();
            string SQL = "select * from quiz ";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Quiz()
                        { 

                            quiznum = Convert.ToInt16(reader["quiznum"]),
                            quiz = reader["quiz"].ToString(),
                            ans = reader["ans"].ToString(),

                        });

                    }
                }
                conn.Close();
            }
            return list;
        }
        
        public Quiz SelectQuiz(int num) //랜덤 번호 받아서 해당번호의 문제와 번호 리턴
        {
            Quiz emp = new Quiz();
            string SQL = "select * from quiz where quiznum = '" + num + "'";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        emp.quiznum = Convert.ToInt16(reader["quiznum"]);
                        emp.quiz = reader["quiz"].ToString();
                        emp.detail = reader["detail"].ToString();
                        emp.img = Convert.ToInt16(reader["img"]);

                    }
                }
                conn.Close();
            }
            return emp;
        }
        public string GetAnswer(int num) //문제 번호받아서 그문제의 답 리턴.
        {
            Quiz emp = new Quiz();
            string? ans = null;
            string SQL = "select ans from quiz where quiznum = '" + num + "'";
            using (MySqlConnection conn = GetSqlConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand(SQL, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        ans = reader["ans"].ToString();


                    }
                }
                conn.Close();
            }
            return ans;

        }
    }
}
