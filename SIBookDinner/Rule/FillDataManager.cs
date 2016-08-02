using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Rule.Entities;
using System.Web;
using Common;
using System.Data;

namespace Rule
{
    public class FillDataManager
    {
        #region 填充职员信息记录
        /// <summary>
        /// 填充职员信息记录
        /// </summary>
        /// <returns></returns>
        public static int FillStaff(string StaffNo, string StaffName, string StaffPwd, string StaffPower, string Remark)
        {
            //StringBuilder sql = new StringBuilder("call AddOnStaff (@staffname,md5(@staffpwd),@staffpower,'20.00',@remark,@createstaffno,now(),now(),0);");
            StringBuilder sql = new StringBuilder("insert into Staffs (StaffNo,StaffName,StaffPwd,StaffPower,Balance,Remark,CreateStaffNo,CreateDatetime,UpdateDatetime,State,LoginTime,LoginIP) ");
            sql.Append(" values (@staffno,@staffname,md5(@staffpwd),@staffpower,0,@remark,@createstaffno,now(),now(),0,'1900-01-01 00:00:00','未登录');");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@staffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@staffname",MySqlDbType.VarChar,50),
                new MySqlParameter("@staffpwd",MySqlDbType.VarChar,50),
                new MySqlParameter("@staffpower",MySqlDbType.Int32),
                new MySqlParameter("@remark",MySqlDbType.VarChar,200),
                new MySqlParameter("@createstaffno",MySqlDbType.VarChar,50)
            };
            parameter[0].Value = StaffNo;
            parameter[1].Value = StaffName;
            parameter[2].Value = StaffPwd;
            parameter[3].Value = StaffPower;
            parameter[4].Value = Remark;
            parameter[5].Value = HttpContext.Current.Session["StaffNo"].ToString();

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 填充充值记录
        /// <summary>
        /// 填充充值记录
        /// </summary>
        /// <returns></returns>
        public static int FillRecharge(string StaffNo, string RechargeMoney, string Remark)
        {
            StringBuilder sql = new StringBuilder("insert into rechargetable (StaffNo, RechargeMoney, RechargeDatetime, Remark, CreateStaffNo, CreateDatetime, UpdateDatetime, State)");
            sql.Append("values (@staffno, @rechargemoney, @rechargedatetime, @remark, @createstaffno,now(),now(),0)");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@staffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@rechargemoney",MySqlDbType.Decimal,6),
                new MySqlParameter("@rechargedatetime",MySqlDbType.DateTime),
                new MySqlParameter("@remark",MySqlDbType.VarChar,200),
                new MySqlParameter("@createstaffno",MySqlDbType.VarChar,50)
            };
            parameter[0].Value = StaffNo;
            parameter[1].Value = RechargeMoney;
            parameter[2].Value = System.DateTime.Now;
            parameter[3].Value = Remark;
            parameter[4].Value = HttpContext.Current.Session["StaffNo"].ToString();

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 填充订餐记录
        /// <summary>
        /// 填充订餐记录
        /// </summary>
        /// <returns></returns>
        public static int FillDinnerbook(decimal TotalPrices, DateTime BookDatetime, string Remark)
        {
            //StringBuilder sql = new StringBuilder("call AddOnDinnerBookTable (@totalprices,@bookstaffno,now(),@remark,@createstaffno);");
            StringBuilder sql = new StringBuilder("insert into DinnerBookTable (TotalPrices,BookStaffNo,BookDatetime,Remark,CreateStaffNo,CreateDatetime,UpdateDatetime,State) ");
            sql.Append(" values (@totalprices,@bookstaffno,@bookdatetime,@remark,@createstaffno,now(),now(),0);");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@totalprices",MySqlDbType.Decimal,6),
                new MySqlParameter("@bookstaffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@bookdatetime",MySqlDbType.DateTime),
                new MySqlParameter("@remark",MySqlDbType.VarChar,200),
                new MySqlParameter("@createstaffno",MySqlDbType.VarChar,50),
            };
            parameter[0].Value = TotalPrices;
            parameter[1].Value = HttpContext.Current.Session["StaffNo"].ToString();
            parameter[2].Value = BookDatetime;
            parameter[3].Value = Remark;
            parameter[4].Value = HttpContext.Current.Session["StaffNo"].ToString();

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 填充消费记录
        /// <summary>
        /// 填充消费记录
        /// </summary>
        /// <returns></returns>
        public static int FillConsume(int BookID, string StaffNo, decimal ConsumeMoney, DateTime ConsumeDatetime, string Remark)
        {
            //StringBuilder sql = new StringBuilder("call AddOnConsumeTable (@bookid,@staffno,@consumemoney,now(),@remark,@createstaffno);");
            StringBuilder sql = new StringBuilder("insert into ConsumeTable (BookID,StaffNo,ConsumeMoney,ConsumeDatetime,Remark,CreateStaffNo,CreateDatetime,UpdateDatetime,State) ");
            sql.Append(" values (@bookid,@staffno,@consumemoney,@consumedatetime,@remark,@createstaffno,now(),now(),0); ");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@bookid",MySqlDbType.VarChar,50),
                new MySqlParameter("@staffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@consumemoney",MySqlDbType.Decimal,6),
                new MySqlParameter("@consumedatetime",MySqlDbType.DateTime),
                new MySqlParameter("@remark",MySqlDbType.VarChar,200),
                new MySqlParameter("@createstaffno",MySqlDbType.VarChar,50)
            };
            parameter[0].Value = BookID;
            parameter[1].Value = StaffNo;
            parameter[2].Value = ConsumeMoney;
            parameter[3].Value = ConsumeDatetime;
            parameter[4].Value = Remark;
            parameter[5].Value = HttpContext.Current.Session["StaffNo"].ToString();

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 填充分组记录
        /// <summary>
        /// 填充分组信息记录
        /// </summary>
        /// <returns></returns>
        public static int FillGroup(string GroupName, string Remark)
        {
            StringBuilder sql = new StringBuilder("insert into grouptable(GroupName,Remark,CreateStaffNo,CreateDatetime,UpdateDatetime,State) values (@groupname,@remark,@createstaffno,@createdatetime,@updatedatetime,0);");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@groupname",MySqlDbType.VarChar,50),
                new MySqlParameter("@remark",MySqlDbType.VarChar,200),
                new MySqlParameter("@createstaffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@createdatetime",MySqlDbType.DateTime),
                new MySqlParameter("@updatedatetime",MySqlDbType.DateTime)
            };

            parameter[0].Value = GroupName;
            parameter[1].Value = Remark;
            parameter[2].Value = HttpContext.Current.Session["StaffNo"].ToString();
            parameter[3].Value = System.DateTime.Now;
            parameter[4].Value = System.DateTime.Now;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 填充分组成员记录
        /// <summary>
        /// 填充分组成员记录
        /// </summary>
        /// <returns></returns>
        public static int FillGroupMembers(int GroupID, string StaffNo, string Remark)
        {
            StringBuilder sql = new StringBuilder("insert into groupmemberstable(GroupID,StaffNo,Remark,CreateStaffNo,CreateDatetime,UpdateDatetime,State) values (@groupid,@staffno,@remark,@createstaffno,@createdatetime,@updatedatetime,0);");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@groupid",MySqlDbType.VarChar,50),
                new MySqlParameter("@staffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@remark",MySqlDbType.VarChar,200),
                new MySqlParameter("@createstaffno",MySqlDbType.VarChar,50),
                new MySqlParameter("@createdatetime",MySqlDbType.DateTime),
                new MySqlParameter("@updatedatetime",MySqlDbType.DateTime)
            };
            parameter[0].Value = GroupID;
            parameter[1].Value = StaffNo;
            parameter[2].Value = Remark;
            parameter[3].Value = HttpContext.Current.Session["StaffNo"].ToString();
            parameter[4].Value = System.DateTime.Now;
            parameter[5].Value = System.DateTime.Now;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion
    }
}
