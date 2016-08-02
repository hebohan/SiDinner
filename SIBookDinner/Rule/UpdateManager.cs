using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MySql.Data.MySqlClient;

namespace Rule
{
    public class UpdateManager
    {
        #region 删除一条分组成员信息
        /// <summary>
        /// 填充职员信息记录
        /// </summary>
        /// <returns></returns>
        public static int DeleteGroupStaff(int ID)
        {
            StringBuilder sql = new StringBuilder("delete from groupmemberstable where ID = @id;");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@id",MySqlDbType.Int32),
            };
            parameter[0].Value = ID;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 删除一条职员信息
        /// <summary>
        /// 更新state字段，置为-1
        /// </summary>
        /// <returns></returns>
        public static int DeleteStaff(int ID)
        {
            StringBuilder sql = new StringBuilder("update staffs set State = -1 where ID = @id;");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@id",MySqlDbType.Int32)
            };
            parameter[0].Value = ID;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 更新一条职员信息
        /// <summary>
        /// 更新相应职员信息
        /// </summary>
        /// <returns></returns>
        public static int UpdateStaff(int ID, string StaffNo, string StaffName, int StaffPower, string Remark)
        {
            StringBuilder sql = new StringBuilder("update staffs set StaffNo=@staffno,StaffName=@staffname,StaffPower=@staffpower,Remark=@remark,UpdateDatetime=@updatedatetime ");
                sql.Append("where ID = @id;");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@id", MySqlDbType.Int32),
                new MySqlParameter("@staffno",MySqlDbType.VarChar, 50),
                new MySqlParameter("@staffname",MySqlDbType.VarChar, 50),
                new MySqlParameter("@staffpower", MySqlDbType.Int32),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 200),
                new MySqlParameter("@updatedatetime", MySqlDbType.DateTime)
            };
            parameter[0].Value = ID;
            parameter[1].Value = StaffNo;
            parameter[2].Value = StaffName;
            parameter[3].Value = StaffPower;
            parameter[4].Value = Remark;
            parameter[5].Value = System.DateTime.Now;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 修改密码
        /// <summary>
        /// 更新相应职员信息
        /// </summary>
        /// <returns></returns>
        public static int UpdateStaffPwd(int ID, string StaffPwd)
        {
            StringBuilder sql = new StringBuilder("update staffs set StaffPwd=md5(@staffpwd),UpdateDatetime=@updatedatetime ");
            sql.Append("where ID = @id;");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@id", MySqlDbType.Int32),
                new MySqlParameter("@staffpwd", MySqlDbType.VarChar, 50),
                new MySqlParameter("@updatedatetime", MySqlDbType.DateTime)
            };
            parameter[0].Value = ID;
            parameter[1].Value = StaffPwd;
            parameter[2].Value = System.DateTime.Now;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 删除一条分组信息
        /// <summary>
        /// 删除一条分组信息
        /// </summary>
        /// <returns></returns>
        public static int DeleteGroup(int ID)
        {
            StringBuilder sql = new StringBuilder("delete from grouptable where ID = @id;");
            sql.Append("delete from groupmemberstable where GroupID = @id;");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@id",MySqlDbType.Int32),
            };
            parameter[0].Value = ID;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion

        #region 更新一条分组信息
        /// <summary>
        /// 更新一条分组信息
        /// </summary>
        /// <returns></returns>
        public static int UpdateGroup(int ID, string GroupName, string Remark)
        {
            StringBuilder sql = new StringBuilder("update grouptable set GroupName=@groupname,Remark=@remark,UpdateDatetime=@updatedatetime ");
            sql.Append("where ID = @id;");
            MySqlParameter[] parameter =
            {
                new MySqlParameter("@id", MySqlDbType.Int32),
                new MySqlParameter("@groupname",MySqlDbType.VarChar, 50),
                new MySqlParameter("@remark", MySqlDbType.VarChar, 200),
                new MySqlParameter("@updatedatetime", MySqlDbType.DateTime)
            };
            parameter[0].Value = ID;
            parameter[1].Value = GroupName;
            parameter[2].Value = Remark;
            parameter[3].Value = System.DateTime.Now;

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            dr.Close();

            return 0;
        }
        #endregion
    }
}
