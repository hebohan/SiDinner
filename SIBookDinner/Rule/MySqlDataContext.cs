using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SIStudio.Framework;

namespace Rule
{
    /// <summary>
    /// 数据库操作
    /// </summary>
    public class MySqlDataContext
    {
        private object _obj = new object();

        #region 数据库连接
        /// <summary>
        /// 数据库连接
        /// </summary>
        private MySqlConnection _mySqlConnection = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connecting">数据库连接</param>
        public MySqlDataContext(string connecting)
        {
            _mySqlConnection = new MySqlConnection(connecting);
        }
        #endregion

        #region 返回受影响行数
        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteStoreCommand(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                return cmd.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        /// <summary>
        /// 返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteStoreCommand(string sql, out int lastInsertId, params MySqlParameter[] parameters)
        {
            lastInsertId = 0;
            int iRet = 0;
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                iRet = cmd.ExecuteNonQuery();
                cmd = new MySqlCommand();
                string sqlStr = "SELECT LAST_INSERT_ID();";
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sqlStr, parameters);
                lastInsertId = SIConvert.ToInt32(cmd.ExecuteScalar(), 0);
                return iRet;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 返回首行
        /// <summary>
        /// 返回首行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T First<T>(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                //属性
                Type type = typeof(T);
                //查询列
                List<string> field = new List<string>();

                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        //对象
                        T model;

                        //查询列
                        if (field.Count <= 0)
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                field.Add(dr.GetName(i).ToLower());
                            }
                        }

                        if (field.Count == 1)
                        {
                            #region 查询单列 基础类型,时间,字符串,及对应的可空类型

                            if (type == typeof(System.String))
                            {
                                if (dr[0] != Convert.DBNull)
                                {
                                    model = (T)dr[0];
                                }
                                else
                                {
                                    model = JsonConvert.DeserializeObject<T>("");
                                }
                            }
                            else
                            {
                                //初始化对象
                                model = (T)Activator.CreateInstance(type);

                                if (dr[0] != Convert.DBNull)
                                {
                                    if (type == typeof(System.DateTime?) || type == typeof(System.DateTime))
                                    {
                                        model =
                                            JsonConvert.DeserializeObject<T>(
                                                JsonConvert.SerializeObject(Convert.ToDateTime(dr[0].ToString())));
                                    }
                                    else
                                    {
                                        model = JsonConvert.DeserializeObject<T>(dr[0].ToString());
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 查询多列 类

                            //实例化
                            model = Activator.CreateInstance<T>();
                            foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                            {
                                if (field.Contains(property.Name.ToLower()) && dr[property.Name] != Convert.DBNull)
                                {
                                    if (property.PropertyType == typeof(System.Boolean) || property.PropertyType == typeof(System.Boolean?))
                                    {
                                        property.SetValue(model, Convert.ToBoolean(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Int16) || property.PropertyType == typeof(System.Int16?))
                                    {
                                        property.SetValue(model, Convert.ToInt16(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Int32) || property.PropertyType == typeof(System.Int32?))
                                    {
                                        property.SetValue(model, Convert.ToInt32(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Int64) || property.PropertyType == typeof(System.Int64?))
                                    {
                                        property.SetValue(model, Convert.ToInt64(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Double) || property.PropertyType == typeof(System.Double?))
                                    {
                                        property.SetValue(model, Convert.ToDouble(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Decimal) || property.PropertyType == typeof(System.Decimal?))
                                    {
                                        property.SetValue(model, Convert.ToDecimal(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.DateTime) || property.PropertyType == typeof(System.DateTime?))
                                    {
                                        property.SetValue(model, Convert.ToDateTime(dr[property.Name]), null);
                                    }
                                    else
                                    {
                                        property.SetValue(model, dr[property.Name], null);
                                    }
                                }
                            }

                            #endregion
                        }

                        dr.Close();
                        //关闭连接
                        _mySqlConnection.Close();

                        return model;
                    }
                    else
                    {
                        dr.Close();
                        //关闭连接
                        _mySqlConnection.Close();

                        return default(T);
                        //throw new Exception("值为空！");
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 返回第一行
        /// <summary>
        /// 返回第一行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public T FirstOrDefault<T>(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                //属性
                Type type = typeof(T);
                //查询列
                List<string> field = new List<string>();

                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        //对象
                        T model;

                        //查询列
                        if (field.Count <= 0)
                        {
                            for (int i = 0; i < dr.FieldCount; i++)
                            {
                                field.Add(dr.GetName(i).ToLower());
                            }
                        }

                        if (field.Count == 1)
                        {
                            #region 查询单列 基础类型,时间,字符串,及对应的可空类型

                            if (type == typeof(System.String))
                            {
                                if (dr[0] != Convert.DBNull)
                                {
                                    model = (T)dr[0];
                                }
                                else
                                {
                                    model = JsonConvert.DeserializeObject<T>("");
                                }
                            }
                            else
                            {
                                //初始化对象
                                model = (T)Activator.CreateInstance(type);

                                if (dr[0] != Convert.DBNull)
                                {
                                    if (type == typeof(System.DateTime?) || type == typeof(System.DateTime))
                                    {
                                        model = JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(Convert.ToDateTime(dr[0].ToString())));
                                    }
                                    else
                                    {
                                        model = JsonConvert.DeserializeObject<T>(dr[0].ToString());
                                    }
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region 查询多列 类

                            //实例化
                            model = Activator.CreateInstance<T>();
                            foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                            {
                                if (field.Contains(property.Name.ToLower()) && dr[property.Name] != Convert.DBNull)
                                {
                                    if (property.PropertyType == typeof(System.Boolean) || property.PropertyType == typeof(System.Boolean?))
                                    {
                                        property.SetValue(model, Convert.ToBoolean(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Int16) || property.PropertyType == typeof(System.Int16?))
                                    {
                                        property.SetValue(model, Convert.ToInt16(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Int32) || property.PropertyType == typeof(System.Int32?))
                                    {
                                        property.SetValue(model, Convert.ToInt32(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Int64) || property.PropertyType == typeof(System.Int64?))
                                    {
                                        property.SetValue(model, Convert.ToInt64(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Double) || property.PropertyType == typeof(System.Double?))
                                    {
                                        property.SetValue(model, Convert.ToDouble(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.Decimal) || property.PropertyType == typeof(System.Decimal?))
                                    {
                                        property.SetValue(model, Convert.ToDecimal(dr[property.Name]), null);
                                    }
                                    else if (property.PropertyType == typeof(System.DateTime) || property.PropertyType == typeof(System.DateTime?))
                                    {
                                        property.SetValue(model, Convert.ToDateTime(dr[property.Name]), null);
                                    }
                                    else
                                    {
                                        property.SetValue(model, dr[property.Name], null);
                                    }
                                }
                            }

                            #endregion
                        }

                        dr.Close();
                        //关闭连接
                        _mySqlConnection.Close();

                        return model;
                    }
                    else
                    {
                        dr.Close();
                        //关闭连接
                        _mySqlConnection.Close();

                        //返回默认值
                        T model;
                        if (type == typeof(System.String))
                        {
                            model = JsonConvert.DeserializeObject<T>("");
                        }
                        else
                        {
                            //初始化对象
                            model = (T)Activator.CreateInstance(type);
                        }

                        return model;
                    }
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 返回查询列表
        /// <summary>
        /// 返回查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> ExecuteStoreQuery<T>(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                lock (_obj)
                {
                    //属性
                    Type type = typeof(T);
                    //查询列表
                    List<T> results = new List<T>();
                    //查询列
                    List<string> field = new List<string>();

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                //查询列
                                if (field.Count <= 0)
                                {
                                    for (int i = 0; i < dr.FieldCount; i++)
                                    {
                                        field.Add(dr.GetName(i).ToLower());
                                    }
                                }

                                if (field.Count == 1)
                                {
                                    #region 查询单列 基础类型,时间,字符串,及对应的可空类型

                                    //对象
                                    T model;

                                    if (type == typeof(System.String))
                                    {
                                        if (dr[0] != Convert.DBNull)
                                        {
                                            model = (T)dr[0];
                                        }
                                        else
                                        {
                                            model = JsonConvert.DeserializeObject<T>("");
                                        }
                                    }
                                    else
                                    {
                                        //初始化对象
                                        model = (T)Activator.CreateInstance(type);

                                        if (dr[0] != Convert.DBNull)
                                        {
                                            if (type == typeof(System.DateTime?) || type == typeof(System.DateTime))
                                            {
                                                model =
                                                    JsonConvert.DeserializeObject<T>(
                                                        JsonConvert.SerializeObject(Convert.ToDateTime(dr[0].ToString())));
                                            }
                                            else
                                            {
                                                model = JsonConvert.DeserializeObject<T>(dr[0].ToString());
                                            }
                                        }
                                    }

                                    //新增
                                    results.Add(model);

                                    #endregion
                                }
                                else
                                {
                                    #region 查询多列 类

                                    //实例化
                                    T model = Activator.CreateInstance<T>();
                                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                                    {
                                        if (field.Contains(property.Name.ToLower()) && dr[property.Name] != Convert.DBNull)
                                        {
                                            if (property.PropertyType == typeof(System.Boolean) || property.PropertyType == typeof(System.Boolean?))
                                            {
                                                property.SetValue(model, Convert.ToBoolean(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.Int16) || property.PropertyType == typeof(System.Int16?))
                                            {
                                                property.SetValue(model, Convert.ToInt16(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.Int32) || property.PropertyType == typeof(System.Int32?))
                                            {
                                                property.SetValue(model, Convert.ToInt32(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.Int64) || property.PropertyType == typeof(System.Int64?))
                                            {
                                                property.SetValue(model, Convert.ToInt64(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.Double) || property.PropertyType == typeof(System.Double?))
                                            {
                                                property.SetValue(model, Convert.ToDouble(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.Decimal) || property.PropertyType == typeof(System.Decimal?))
                                            {
                                                property.SetValue(model, Convert.ToDecimal(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.DateTime) || property.PropertyType == typeof(System.DateTime?))
                                            {
                                                property.SetValue(model, Convert.ToDateTime(dr[property.Name]), null);
                                            }
                                            else if (property.PropertyType == typeof(System.String))
                                            {
                                                property.SetValue(model, Convert.ToString(dr[property.Name]), null);
                                            }
                                            else
                                            {
                                                property.SetValue(model, dr[property.Name], null);
                                            }
                                        }
                                    }
                                    //新增
                                    results.Add(model);

                                    #endregion
                                }
                            }
                        }
                    }

                    return results;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 返回dataset
        /// <summary>
        /// 返回dataset
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();

                //准备执行
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                DataSet ds = new DataSet();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);

                return ds;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 返回DataTable
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, params MySqlParameter[] parameters)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand();

                //准备执行
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                DataTable ds = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = cmd;
                adapter.Fill(ds);

                return ds;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 获取DataReader
        /// <summary>
        /// 用执行的数据库连接执行一个返回数据集的sql命令
        /// </summary>
        /// <remarks>
        /// 举例:
        ///  MySqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new MySqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="parameters">执行命令所用参数的集合</param>
        /// <returns>包含结果的读取器</returns>
        public MySqlDataReader ExecuteReader(string sql, params MySqlParameter[] parameters)
        {
            //在这里我们用一个try/catch结构执行sql文本命令/存储过程，因为如果这个方法产生一个异常我们要关闭连接，因为没有读取器存在，
            //因此commandBehaviour.CloseConnection 就不会执行
            try
            {
                //创建一个MySqlCommand对象
                MySqlCommand cmd = new MySqlCommand();

                //准备执行
                PrepareCommand(cmd, _mySqlConnection, null, CommandType.Text, sql, parameters);

                //调用 MySqlCommand  的 ExecuteReader 方法
                MySqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                //清除参数
                cmd.Parameters.Clear();

                return reader;
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
        }
        #endregion

        #region 准备执行命令
        /// <summary>
        /// 准备执行命令
        /// </summary>
        /// <param name="cmd">sql命令</param>
        /// <param name="conn">OleDb连接</param>
        /// <param name="trans">OleDb事务</param>
        /// <param name="cmdType">命令类型例如存储过程或者文本</param>
        /// <param name="cmdText">命令文本,例如:Select * from Products</param>
        /// <param name="cmdParms">执行命令的参数</param>
        private void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, CommandType cmdType, string cmdText, MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                //开启连接
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 1800;
            cmd.CommandType = cmdType;

            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            if (cmdParms != null)
            {
                foreach (MySqlParameter parm in cmdParms)
                {
                    if (parm != null)
                    {
                        cmd.Parameters.Add(parm);
                    }
                }
            }
        }
        #endregion

        #region 替换SQL语句参数
        /// <summary>
        /// 替换SQL语句参数
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public string ReplaceSqlPara(object para)
        {
            if (para == null)
            {
                return "null";
            }

            if (para.GetType().IsPrimitive)
            {
                return para.ToString();
            }
            else if (para.GetType() == typeof(DateTime))
            {
                return "'" + SIConvert.ToDateTime(para.ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else
            {
                return "'" + para.ToString().Replace("\\", "\\\\").Replace("'", "\\'") + "'";
            }
        }

        #endregion

        //存储过程



        #region 刷数据
        /// <summary>
        /// 刷数据
        /// </summary>
        /// <returns>执行结果</returns>
        public bool RefreshDataSP()
        {
            bool result = false;

            MySqlCommand cmd = new MySqlCommand();
            PrepareCommand(cmd, _mySqlConnection, null, CommandType.StoredProcedure, "SP_RefreshData", null);
            try
            {
                cmd.ExecuteNonQuery();
                result = true;
            }
            catch
            {

            }
            finally
            {
                if (_mySqlConnection.State == ConnectionState.Open)
                {
                    _mySqlConnection.Close();
                }
            }
            return result;
        }
        #endregion
    }
}
