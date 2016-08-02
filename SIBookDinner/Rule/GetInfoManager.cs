using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rule.Entities;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SIStudio.Framework;
using Common;
using System.Data;

namespace Rule
{
    public class GetInfoManager
    {
        #region 获取职员信息记录
        /// <summary>
        /// 获取职员信息记录
        /// </summary>
        /// <returns></returns>
        public static List<staff> GetStaff(string StaffNo, string StaffName, string startBalance, string endBalance)
        {
            List<staff> staffList = new List<staff>();
            StringBuilder sql = new StringBuilder("select ID,StaffNo,StaffName,StaffPower,Balance,Remark from staffs where State = 0");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(StaffNo))
            {
                sql.Append(" and StaffNo = @staffno ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@staffno",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Format("{0}", StaffNo.Trim())
                });
                //countSql.Append("and parameters.visitor_nick='" + nike + "'");
            }
            if (!string.IsNullOrEmpty(StaffName))
            {
                sql.Append(" and StaffName like @staffname ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@staffname",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Format("%{0}%", StaffName.Trim())
                });
                //countSql.Append("and appsession.VisitorID='" + visitorId + "'");
            }
            if (!string.IsNullOrEmpty(startBalance))
            {
                decimal dt = Convert.ToDecimal(startBalance);
                // countSql.Append("and appsession.UpdateDT >= '" + dt + "' ");
                sql.Append(" and Balance >= @startbalance ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startbalance",
                    MySqlDbType = MySqlDbType.Decimal,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(endBalance))
            {
                decimal dt = Convert.ToDecimal(endBalance);
                //countSql.Append("and appsession.UpdateDT <= '" + dt + "' ");
                sql.Append(" and Balance <= @startbalance ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startbalance",
                    MySqlDbType = MySqlDbType.Decimal,
                    Value = dt
                });
            }

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            while (dr.Read())
            {
                staff newstaff = new staff();
                newstaff.ID = Convert.ToInt32(dr["ID"]);
                newstaff.StaffNo = Convert.ToString(dr["StaffNo"]);
                newstaff.StaffName = Convert.ToString(dr["StaffName"]);
                newstaff.StaffPower = Convert.ToInt32(dr["StaffPower"]);
                newstaff.Balance = Convert.ToDecimal(dr["Balance"]);
                newstaff.Remark = Convert.ToString(dr["Remark"]);

                staffList.Add(newstaff);
            }
            dr.Close();
            return staffList;
        }

        /// <summary>
        /// 获取全部不在本组的职员信息记录
        /// </summary>
        /// <returns></returns>
        public static List<staff> GetStaff(int GroupID)
        {
            List<staff> staffList = new List<staff>();
            StringBuilder sql = new StringBuilder("select ID,StaffNo,StaffName,StaffPower,Balance,Remark from staffs where State = 0 and staffno not in (select staffno from groupmemberstable where GroupID = @groupid);");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@groupid",
                MySqlDbType = MySqlDbType.Int32,
                Value = GroupID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());
            while (dr.Read())
            {
                staff newstaff = new staff();
                newstaff.ID = Convert.ToInt32(dr["ID"]);
                newstaff.StaffNo = Convert.ToString(dr["StaffNo"]);
                newstaff.StaffName = Convert.ToString(dr["StaffName"]);
                newstaff.StaffPower = Convert.ToInt32(dr["StaffPower"]);
                newstaff.Balance = Convert.ToDecimal(dr["Balance"]);
                newstaff.Remark = Convert.ToString(dr["Remark"]);

                staffList.Add(newstaff);
            }
            dr.Close();
            return staffList;
        }
        #endregion

        #region 获取充值信息记录
        /// <summary>
        /// 获取充值信息记录
        /// </summary>
        /// <returns></returns>
        public static List<recharge> GetRecharge(string StaffNo, string startRechargeMoney, string endRechargeMoney, string startRechargeDatetime, string endRechargeDatetime)
        {
            List<recharge> rechargeList = new List<recharge>();
            StringBuilder sql = new StringBuilder("select rechargetable.ID,rechargetable.StaffNo,staffs.StaffName,rechargetable.RechargeMoney,rechargetable.RechargeDatetime,rechargetable.Remark,rechargetable.CreateStaffNo ");
            sql.Append("from rechargetable inner join staffs on rechargetable.StaffNo = staffs.StaffNo ");
            sql.Append("where rechargetable.State = 0 and staffs.State=0;");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(StaffNo))
            {
                sql.Append(" and StaffNo = @staffno ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@staffno",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Format("{0}", StaffNo.Trim())
                });
            }
            if (!string.IsNullOrEmpty(startRechargeMoney))
            {
                decimal dt = Convert.ToDecimal(startRechargeMoney);
                sql.Append(" and RechargeMoney >= @startrechargemoney ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startrechargemoney",
                    MySqlDbType = MySqlDbType.Decimal,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(endRechargeMoney))
            {
                decimal dt = Convert.ToDecimal(endRechargeMoney);
                sql.Append(" and RechargeMoney <= @endrechargemoney ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@endrechargemoney",
                    MySqlDbType = MySqlDbType.Decimal,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(startRechargeDatetime))
            {
                DateTime dt = Convert.ToDateTime(startRechargeDatetime);
                sql.Append(" and RechargeDatetime >= @startrechargedatetime ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startrechargedatetime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(endRechargeDatetime))
            {
                DateTime dt = Convert.ToDateTime(endRechargeDatetime);
                sql.Append(" and RechargeDatetime <= @endrechargedatetime ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@endrechargedatetime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = dt
                });
            }
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());


            while (dr.Read())
            {
                recharge newrecharge = new recharge();
                newrecharge.ID = Convert.ToInt32(dr["ID"]);
                newrecharge.StaffNo = Convert.ToString(dr["StaffNo"]);
                newrecharge.StaffName = Convert.ToString(dr["StaffName"]);
                newrecharge.RechargeMoney = Convert.ToDecimal(dr["RechargeMoney"]);
                newrecharge.RechargeDatetime = Convert.ToDateTime(dr["RechargeDatetime"]);
                newrecharge.Remark = Convert.ToString(dr["Remark"]);
                newrecharge.CreateStaffNo = Convert.ToString(dr["CreateStaffNo"]);

                rechargeList.Add(newrecharge);
            }
            dr.Close();
            return rechargeList;
        }
        #endregion

        #region 获取消费信息记录
        /// <summary>
        /// 获取消费信息记录
        /// </summary>
        /// <returns></returns>
        public static List<consume> GetConsume(string StaffNo, string startConsumeMoney, string endConsumeMoney, string startConsumeDatetime, string endConsumeDatetime)
        {
            List<consume> consumeList = new List<consume>();
            StringBuilder sql = new StringBuilder("select consumetable.ID,consumetable.BookID,consumetable.StaffNo,staffs.StaffName,consumetable.ConsumeMoney,consumetable.ConsumeDatetime,consumetable.Remark,consumetable.CreateStaffNo ");
            sql.Append("from consumetable inner join staffs on consumetable.StaffNo = staffs.StaffNo ");
            sql.Append("where consumetable.State = 0 and staffs.State=0 ");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(StaffNo))
            {
                sql.Append(" and StaffNo = @staffno ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@staffno",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Format("{0}", StaffNo.Trim())
                });
            }
            if (!string.IsNullOrEmpty(startConsumeMoney))
            {
                decimal dt = Convert.ToDecimal(startConsumeMoney);
                sql.Append(" and ConsumeMoney >= @startconsumemoney ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startconsumemoney",
                    MySqlDbType = MySqlDbType.Decimal,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(endConsumeMoney))
            {
                decimal dt = Convert.ToDecimal(endConsumeMoney);
                sql.Append(" and ConsumeMoney <= @endconsumemoney ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@endconsume",
                    MySqlDbType = MySqlDbType.Decimal,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(startConsumeDatetime))
            {
                DateTime dt = Convert.ToDateTime(startConsumeDatetime);
                sql.Append(" and ConsumeDatetime >= @startconsumedatetime ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startconsumedatetime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(endConsumeDatetime))
            {
                DateTime dt = Convert.ToDateTime(endConsumeDatetime);
                sql.Append(" and ConsumeDatetime <= @endconsumedatetime ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@endrechargedatetime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = dt
                });
            }
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());


            while (dr.Read())
            {
                consume newconsume = new consume();

                newconsume.ID = Convert.ToInt32(dr["ID"]);
                newconsume.BookID = Convert.ToInt32(dr["BookID"]);
                newconsume.StaffNo = Convert.ToString(dr["StaffNo"]);
                newconsume.StaffName = Convert.ToString(dr["StaffName"]);
                newconsume.ConsumeMoney = Convert.ToDecimal(dr["ConsumeMoney"]);
                newconsume.ConsumeDatetime = Convert.ToDateTime(dr["ConsumeDatetime"]);
                newconsume.Remark = Convert.ToString(dr["Remark"]);
                newconsume.CreateStaffNo = Convert.ToString(dr["CreateStaffNo"]);

                consumeList.Add(newconsume);
            }
            dr.Close();
            return consumeList;
        }
        #endregion

        #region 获取订餐信息记录
        /// <summary>
        /// 获取订餐信息记录
        /// </summary>
        /// <returns></returns>
        public static List<dinnerbook> GetDinnerbook(string ID, string BookStaffNo, string startBookDatetime, string endBookDatetime)
        {
            List<dinnerbook> dinnerbookList = new List<dinnerbook>();
            StringBuilder sql = new StringBuilder("select ID,TotalPrices,BookStaffNo,BookDatetime,Remark from dinnerbooktable where State = 0");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            if (!string.IsNullOrEmpty(ID))
            {
                sql.Append(" and ID = @id ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    MySqlDbType = MySqlDbType.Int32,
                    Value = string.Format("{0}", ID.Trim())
                });
            }
            if (!string.IsNullOrEmpty(BookStaffNo))
            {
                sql.Append(" and BookStaffNo = @bookstaffno ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@bookstaffno",
                    MySqlDbType = MySqlDbType.VarChar,
                    Value = string.Format("{0}", BookStaffNo.Trim())
                });
            }
            if (!string.IsNullOrEmpty(startBookDatetime))
            {
                DateTime dt = Convert.ToDateTime(startBookDatetime);
                sql.Append(" and ConsumeDatetime >= @startconsumedatetime ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@startconsumedatetime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = dt
                });
            }
            if (!string.IsNullOrEmpty(endBookDatetime))
            {
                DateTime dt = Convert.ToDateTime(endBookDatetime);
                sql.Append(" and ConsumeDatetime <= @endconsumedatetime ");
                parameter.Add(new MySqlParameter
                {
                    ParameterName = "@endrechargedatetime",
                    MySqlDbType = MySqlDbType.DateTime,
                    Value = dt
                });
            }
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());


            while (dr.Read())
            {
                dinnerbook newdinnerbook = new dinnerbook();
                newdinnerbook.ID = Convert.ToInt32(dr["ID"]);
                newdinnerbook.TotalPrices = Convert.ToDecimal(dr["TotalPrices"]);
                newdinnerbook.BookStaffNo = Convert.ToString(dr["BookStaffNo"]);
                newdinnerbook.BookDatetime = Convert.ToDateTime(dr["BookDatetime"]);
                newdinnerbook.Remark = Convert.ToString(dr["Remark"]);

                dinnerbookList.Add(newdinnerbook);
            }
            dr.Close();
            return dinnerbookList;
        }
        #endregion

        #region 获取分组记录
        /// <summary>
        /// 获取分组记录
        /// </summary>
        /// <returns></returns>
        public static List<group> GetGroup()
        {
            List<group> groupList = new List<group>();
            StringBuilder sql = new StringBuilder("select ID,GroupName from grouptable where State = 0");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                group newdinnerbook = new group();
                newdinnerbook.ID = Convert.ToInt32(dr["ID"]);
                newdinnerbook.GroupName = Convert.ToString(dr["GroupName"]);
                groupList.Add(newdinnerbook);
            }
            dr.Close();
            return groupList;
        }
        #endregion

        #region 获取分组成员情况记录
        /// <summary>
        /// 获取职员分组情况记录
        /// </summary>
        /// <returns></returns>
        public static List<staffGroup> GetStaffGroup(int ID)
        {
            List<staffGroup> staffgroupList = new List<staffGroup>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select groupmemberstable.ID, groupmemberstable.StaffNo, staffs.StaffName, groupmemberstable.Remark ");
            sql.Append(" from groupmemberstable inner join staffs on groupmemberstable.StaffNo = staffs.StaffNo ");
            sql.Append(" where groupmemberstable.State=0 and staffs.State=0 and groupmemberstable.GroupID=@id");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.VarChar,
                Value = ID
            });
            
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());


            while (dr.Read())
            {
                staffGroup newstaffgroup = new staffGroup();
                newstaffgroup.ID = Convert.ToInt32(dr["ID"]);
                newstaffgroup.StaffNo = Convert.ToString(dr["StaffNo"]);
                newstaffgroup.StaffName = Convert.ToString(dr["StaffName"]);
                newstaffgroup.Remark = Convert.ToString(dr["Remark"]);

                staffgroupList.Add(newstaffgroup);
            }
            dr.Close();
            return staffgroupList;
        }
        #endregion

        #region 获取分组中指定职员详细记录
        /// <summary>
        /// 获取职员分组情况记录
        /// </summary>
        /// <returns></returns>
        public static List<staffGroup> GetGroupStaffInfo(int ID)
        {
            List<staffGroup> staffgroupList = new List<staffGroup>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select grouptable.ID, grouptable.StaffNo, staffs.StaffName, grouptable.StaffGroup, grouptable.Remark, grouptable.CreateStaffNo, grouptable.CreateDatetime ");
                sql.Append(" from grouptable inner join staffs on grouptable.StaffNo = staffs.StaffNo ");
                sql.Append(" where grouptable.State=0 and staffs.State=0 and grouptable.ID=@id");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                staffGroup newstaffgroup = new staffGroup();
                newstaffgroup.ID = Convert.ToInt32(dr["ID"]);
                newstaffgroup.StaffNo = Convert.ToString(dr["StaffNo"]);
                newstaffgroup.StaffName = Convert.ToString(dr["StaffName"]);
                newstaffgroup.StaffGroup = Convert.ToString(dr["StaffGroup"]);
                newstaffgroup.Remark = Convert.ToString(dr["Remark"]);
                newstaffgroup.CreateStaffNo = Convert.ToString(dr["CreateStaffNo"]);
                newstaffgroup.CreateDatetime = Convert.ToDateTime(dr["CreateDatetime"]);

                staffgroupList.Add(newstaffgroup);
            }
            dr.Close();
            return staffgroupList;
        }
        #endregion

        #region 获取指定职员详细记录
        /// <summary>
        /// 获取指定职员详细记录
        /// </summary>
        /// <returns></returns>
        public static List<staff> GetStaffInfo(int ID)
        {
            List<staff> staffList = new List<staff>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select * from staffs where State = 0 and ID = @id ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                staff newstaff = new staff();
                newstaff.ID = Convert.ToInt32(dr["ID"]);
                newstaff.StaffNo = Convert.ToString(dr["StaffNo"]);
                newstaff.StaffName = Convert.ToString(dr["StaffName"]);
                newstaff.StaffPwd = Convert.ToString(dr["StaffPwd"]);
                newstaff.StaffPower = Convert.ToInt32(dr["StaffPower"]);
                newstaff.Balance = Convert.ToDecimal(dr["Balance"]);
                newstaff.Remark = Convert.ToString(dr["Remark"]);
                newstaff.CreateStaffNo = Convert.ToString(dr["CreateStaffNo"]);
                newstaff.CreateDatetime = Convert.ToDateTime(dr["CreateDatetime"]);
                newstaff.UpdateDatetime = Convert.ToDateTime(dr["UpdateDatetime"]);
                newstaff.LoginTime = Convert.ToDateTime(dr["LoginTime"]);
                newstaff.LoginIP = Convert.ToString(dr["LoginIP"]);

                staffList.Add(newstaff);
            }
            dr.Close();
            return staffList;
        }

        /// <summary>
        /// 获取指定职员权限情况
        /// </summary>
        /// <returns></returns>
        public static int GetStaffPower(int ID)
        {
            int StaffPower = -1;
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select StaffPower from staffs where State = 0 and ID = @id ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {                
                StaffPower = Convert.ToInt32(dr["StaffPower"]);
            }
            dr.Close();
            return StaffPower;

        }
        #endregion

        #region 获取订单号
        /// <summary>
        /// 获取订单号
        /// </summary>
        /// <returns></returns>
        public static int GetBookID()
        {
            int bookid = -1;
            StringBuilder sql = new StringBuilder("select max(ID) ID from dinnerbooktable where State = 0");
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                bookid = Convert.ToInt32(dr["ID"]);
            }
            dr.Close();
            return bookid;
        }
        #endregion

        #region 获取密码
        /// <summary>
        /// 获取密码
        /// </summary>
        /// <returns></returns>
        public static string GetPwd(int ID)
        {
            string Password = null;
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select StaffPwd from staffs where State = 0 and ID = @id ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                Password = Convert.ToString(dr["StaffPwd"]);
            }
            dr.Close();
            return Password;

        }
        #endregion

        #region 获取待充值人员信息
        /// <summary>
        /// 获取待充值人员工号，姓名
        /// </summary>
        /// <returns></returns>
        public static List<recharge> GetStaffNo(int ID)
        {
            List<recharge> rechargeList = new List<recharge>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select StaffNo,StaffName from staffs where State = 0 and ID = @id ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                recharge newrecharge = new recharge();
                newrecharge.StaffNo = Convert.ToString(dr["StaffNo"]);
                newrecharge.StaffName = Convert.ToString(dr["StaffName"]);

                rechargeList.Add(newrecharge);
            }
            dr.Close();
            return rechargeList;
        }
        #endregion

        #region 获取订餐记录明细
        /// <summary>
        /// 获取订餐记录明细
        /// </summary>
        /// <returns></returns>
        public static List<consume> GetDinnerbookDetails(string ID)
        {
            List<consume> consumeList = new List<consume>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder(" select consumetable.ID,consumetable.StaffNo,staffs.StaffName,consumetable.ConsumeMoney,consumetable.ConsumeDatetime,consumetable.Remark ");
            sql.Append(" from consumetable inner join staffs on consumetable.StaffNo = staffs.StaffNo ");
            sql.Append(" where consumetable.State = 0 and staffs.State=0 and consumetable.BookID = @bookid ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@bookid",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });
            
            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                consume newconsume = new consume();

                newconsume.ID = Convert.ToInt32(dr["ID"]);
                newconsume.StaffNo = Convert.ToString(dr["StaffNo"]);
                newconsume.StaffName = Convert.ToString(dr["StaffName"]);
                newconsume.ConsumeMoney = Convert.ToDecimal(dr["ConsumeMoney"]);
                newconsume.ConsumeDatetime = Convert.ToDateTime(dr["ConsumeDatetime"]);
                newconsume.Remark = Convert.ToString(dr["Remark"]);

                consumeList.Add(newconsume);
            }
            dr.Close();
            return consumeList;
        }
        #endregion

        #region 获取指定分组信息
        /// <summary>
        /// 获取指定分组的GroupID和GroupName
        /// </summary>
        /// <returns></returns>
        public static List<groupMembers> GetGroupInfo(int ID)
        {
            List<groupMembers> groupMembersList = new List<groupMembers>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select ID,GroupName from grouptable where State=0 and ID=@id; ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                groupMembers newgroupMembers = new groupMembers();
                newgroupMembers.GroupID = Convert.ToInt32(dr["ID"]);
                newgroupMembers.GroupName = Convert.ToString(dr["GroupName"]);

                groupMembersList.Add(newgroupMembers);
            }
            dr.Close();
            return groupMembersList;
        }

        /// <summary>
        /// 获取指定分组的GroupID和GroupName
        /// 用于Edit
        /// </summary>
        /// <returns></returns>
        public static List<group> GetGroupInfoEdit(int ID)
        {
            List<group> groupList = new List<group>();
            List<MySqlParameter> parameter = new List<MySqlParameter>();
            StringBuilder sql = new StringBuilder("select ID,GroupName from grouptable where State=0 and ID=@id; ");
            parameter.Add(new MySqlParameter
            {
                ParameterName = "@id",
                MySqlDbType = MySqlDbType.Int32,
                Value = ID
            });

            MySqlDataReader dr = MySQLHelper.ExecuteReader(CommandType.Text, sql.ToString(), parameter.ToArray());

            while (dr.Read())
            {
                group newgroup = new group();
                newgroup.ID = Convert.ToInt32(dr["ID"]);
                newgroup.GroupName = Convert.ToString(dr["GroupName"]);

                groupList.Add(newgroup);
            }
            dr.Close();
            return groupList;
        }
        #endregion
    }
}
