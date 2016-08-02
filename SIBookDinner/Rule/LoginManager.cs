using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Common;
using System.Web;
using System.Web.Mvc;
using Rule.Entities;
using System.Web.Security;

namespace Rule
{
    public class LoginManager 
    {
        #region 登陆
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="StaffName"></param>
        /// <param name="StaffPwd"></param>
        /// <returns></returns>
        public static bool Login(string StaffName, string StaffPwd) 
        {
            StaffPwd = SIStudio.Framework.SISecurity.MD5(StaffPwd);

            string sql = "select * from staffs where StaffName=@staffname and StaffPwd=@staffpwd and State=0;";
            object obj = MySQLHelper.ExecuteScalar(CommandType.Text, sql, new MySqlParameter { ParameterName = "@staffname", Value = StaffName }, new MySqlParameter { ParameterName = "@staffpwd", Value = StaffPwd });
            if (obj != null)
            {
                StringBuilder loginsql = new StringBuilder("select ID,StaffNo,StaffPower from staffs where StaffName=@staffname and StaffPwd=@staffpwd;");
                loginsql.Append(" update staffs set UpdateDatetime = @logintime, LoginTime = @logintime, LoginIP = @loginip where StaffName=@staffname and StaffPwd=@staffpwd;");
                List<MySqlParameter> parameter = new List<MySqlParameter>();
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@staffname",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Format("{0}", StaffName.Trim())
                });
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@staffpwd",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = StaffPwd
                });
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@logintime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = System.DateTime.Now
                });
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@loginip",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = HttpContext.Current.Request.UserHostAddress
                });
                MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, loginsql.ToString(), parameter.ToArray());

                if (dr.Read())
                {
                    HttpContext.Current.Session.Add("ID", Convert.ToInt32(dr["ID"]));
                    HttpContext.Current.Session.Add("StaffNo", Convert.ToString(dr["StaffNo"]));
                    HttpContext.Current.Session.Add("StaffName", Convert.ToString(StaffName));
                    HttpContext.Current.Session.Add("StaffPower", Convert.ToInt32(dr["StaffPower"]));
                }
                string userdata;
                if (Convert.ToInt32(dr["StaffPower"]) == 1)
                {
                    userdata = "admins";
                }
                else
                {
                    userdata = "staffs";
                }
                    
                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                        1,
                        StaffName,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(30),
                        false,
                        userdata
                        );
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                System.Web.HttpCookie authCookie = new System.Web.HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);

                dr.Close();
                return true;
            }
                
            return false;
        }
        #endregion
    }
}
