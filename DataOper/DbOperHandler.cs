using System;
using System.Data;

namespace XkCms.DataOper.Data
{
    /// <summary>
    /// 表示数据库连接类型。
    /// </summary>
    public enum DatabaseType : byte { SqlServer, OleDb };
    /// <summary>
    /// DbOperHandler 的摘要说明。
    /// </summary>
    public abstract class DbOperHandler
    {
        /// <summary>
        /// 析构函数，释放申请的资源。
        /// </summary>
        ~DbOperHandler()
        {
            conn.Close();
        }
        /// <summary>
        /// 表示数据库连接的类型，目前支持SqlServer和OLEDB
        /// </summary>
        public DatabaseType dbType = DatabaseType.OleDb;
        /// <summary>
        /// 返回当前使用的数据库连接对象。
        /// </summary>
        /// <returns></returns>
        public System.Data.IDbConnection GetConnection()
        {
            return conn;
        }
        /// <summary>
        /// 条件表达式，用于在数据库操作时筛选记录，通常用于仅需指定表名称和某列名称的操作，如GetValue()，Delete()等，支持查询参数，由AddConditionParameters指定。。
        /// </summary>
        public string ConditionExpress = string.Empty;
        /// <summary>
        /// 当前的SQL语句。
        /// </summary>
        public string SqlCmd = string.Empty;
        /// <summary>
        /// 当前操作所涉及的数据表名称。
        /// </summary>
        protected string tableName = string.Empty;
        /// <summary>
        /// 当前操作所设计的字段名称。
        /// </summary>
        protected string fieldName = string.Empty;
        /// <summary>
        /// 当前所使用的数据库连接。
        /// </summary>
        protected System.Data.IDbConnection conn;
        /// <summary>
        /// 当前所使用的命令对象。
        /// </summary>
        protected System.Data.IDbCommand cmd;
        /// <summary>
        /// 当前所使用的数据库适配器。
        /// </summary>
        protected System.Data.IDbDataAdapter da;


        /// <summary>
        /// 用于存储字段/值配对。
        /// </summary>
        protected System.Collections.ArrayList alFieldItems = new System.Collections.ArrayList(10);
        /// <summary>
        /// 用于存储SQL语句中的查询参数。
        /// </summary>
        protected System.Collections.ArrayList alSqlCmdParameters = new System.Collections.ArrayList(5);
        /// <summary>
        /// 用于存储条件表达式中的查询参数。
        /// </summary>
        protected System.Collections.ArrayList alConditionParameters = new System.Collections.ArrayList(5);
        /// <summary>
        /// 重值该对象，使之恢复到构造时的状态。
        /// </summary>
        public void Reset()
        {
            this.alFieldItems.Clear();
            this.alSqlCmdParameters.Clear();
            this.alConditionParameters.Clear();
            this.ConditionExpress = string.Empty;
            this.SqlCmd = string.Empty;
            this.cmd.Parameters.Clear();
            this.cmd.CommandText = string.Empty;
        }
        /// <summary>
        /// 添加一个字段/值对到数组中。
        /// </summary>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_fieldValue">字段值。</param>
        public void AddFieldItem(string _fieldName, object _fieldValue)
        {
            if (dbType == DatabaseType.OleDb)
                _fieldName = "[" + _fieldName + "]";
            for (int i = 0; i < this.alFieldItems.Count; i++)
            {
                if (((DbKeyItem)this.alFieldItems[i]).fieldName == _fieldName)
                {
                    throw new ArgumentException("The field name has existed!");
                }
            }
            this.alFieldItems.Add(new DbKeyItem(_fieldName, _fieldValue));
        }
        /// <summary>
        /// 添加条件表达式中的查询参数到数组中。注意：当数据库连接为SqlServer时，参数名称必须和SQL语句匹配。其它则只需保持添加顺序一致，名称无需匹配。
        /// </summary>
        /// <param name="_conditionName">条件名称。</param>
        /// <param name="_conditionValue">条件值。</param>
        public void AddConditionParameter(string _conditionName, object _conditionValue)
        {
            for (int i = 0; i < this.alConditionParameters.Count; i++)
            {
                if (((DbKeyItem)this.alConditionParameters[i]).fieldName == _conditionName)
                {
                    throw new ArgumentException("The condition name has existed!");
                }
            }
            this.alConditionParameters.Add(new DbKeyItem(_conditionName, _conditionValue));
        }

        /// <summary>
        /// 添加SQL语句中的查询参数到数组中。注意：当数据库连接为SqlServer时，参数名称必须和SQL语句匹配。其它则只需保持添加顺序一致，名称无需匹配。
        /// </summary>
        /// <param name="_paraName">参数名称。</param>
        /// <param name="_paraValue">参数值。</param>
        public void AddSqlCmdParameters(string _paraName, object _paraValue)
        {
            for (int i = 0; i < this.alSqlCmdParameters.Count; i++)
            {
                if (((DbKeyItem)this.alSqlCmdParameters[i]).fieldName == _paraName)
                {
                    throw new ArgumentException("The sqlcmd parameter name has existed!");
                }
            }
            this.alSqlCmdParameters.Add(new DbKeyItem(_paraName, _paraValue));
        }
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="tableName">要查询的数据表名</param>
        /// <returns>是/否</returns>
        public bool Exist(string tableName)
        {
            return this.GetValue(tableName, "count(*)").ToString() != "0";
        }
        /// <summary>
        /// 抽象函数。用于产生Command对象所需的参数。
        /// </summary>
        protected abstract void GenParameters();
        /// <summary>
        /// 根据当前alFieldItem数组中存储的字段/值向指定表中添加一条数据。在该表无触发器的情况下返回添加数据所获得的自动增长id值。
        /// </summary>
        /// <param name="_tableName">要插入数据的表名称。</param>
        /// <returns>返回本数据连接上产生的最后一个自动增长id值。</returns>
        public int Insert(string _tableName)
        {

            this.tableName = _tableName;
            this.fieldName = string.Empty;
            this.SqlCmd = "insert into " + this.tableName + "(";
            string tempValues = " values (";
            for (int i = 0; i < this.alFieldItems.Count - 1; i++)
            {
                this.SqlCmd += ((DbKeyItem)alFieldItems[i]).fieldName;
                this.SqlCmd += ",";

                tempValues += "@para";
                tempValues += i.ToString();

                tempValues += ",";
            }
            this.SqlCmd += ((DbKeyItem)alFieldItems[alFieldItems.Count - 1]).fieldName;
            this.SqlCmd += ") ";

            tempValues += "@para";
            tempValues += (alFieldItems.Count - 1).ToString();

            tempValues += ")";
            this.SqlCmd += tempValues;
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            cmd.ExecuteNonQuery();
            int autoId = 0;
            try
            {
                cmd.CommandText = "select @@identity as id";
                autoId = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch(Exception){}
            return autoId;
        }

        /// <summary>
        /// 根据当前alFieldItem数组中存储的字段/值和条件表达式所指定的条件来更新数据库中的记录，返回所影响的行数。
        /// </summary>
        /// <param name="_tableName">要更新的数据表名称。</param>
        /// <returns>返回此次操作所影响的数据行数。</returns>
        public int Update(string _tableName)
        {
            this.tableName = _tableName;
            this.fieldName = string.Empty;
            this.SqlCmd = "update " + this.tableName + " set ";
            for (int i = 0; i < this.alFieldItems.Count - 1; i++)
            {
                this.SqlCmd += ((DbKeyItem)alFieldItems[i]).fieldName;
                this.SqlCmd += "=";

                this.SqlCmd += "@para";
                this.SqlCmd += i.ToString();

                this.SqlCmd += ",";
            }
            this.SqlCmd += ((DbKeyItem)alFieldItems[alFieldItems.Count - 1]).fieldName;
            this.SqlCmd += "=";

            this.SqlCmd += "@para";
            this.SqlCmd += (alFieldItems.Count - 1).ToString();


            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            int effectedLines = this.cmd.ExecuteNonQuery();
            return effectedLines;
        }

        /// <summary>
        /// 执行SqlCmd中的SQL语句，参数由AddSqlCmdParameters指定，与ConditionExpress无关。
        /// </summary>
        /// <returns>返回此次操作所影响的数据行数。</returns>
        public int ExecuteSqlNonQuery()
        {
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// 获取指定表，指定列，指定条件的第一个符合条件的值。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>获取的值。如果为空则返回string.Empty。</returns>
        public object GetValue(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            this.SqlCmd = "select " + this.fieldName + " from " + this.tableName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            object ret = cmd.ExecuteScalar();
            if (ret == null) ret = (object)string.Empty;
            return ret;
        }

        /// <summary>
        /// 获取指定表，指定列，指定条件的第一行中符合条件的值的集合。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldNames">字段名称,以逗号隔开。</param>
        /// <returns>获取的值。如果为空则返回null。</returns>
        public object[] GetValues(string _tableName, string _fieldNames)
        {
            this.SqlCmd = "select " + _fieldNames + " from " + _tableName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            System.Data.DataSet ds = new System.Data.DataSet();
            this.da.SelectCommand = this.cmd;
            this.da.Fill(ds);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                object[] _obj = new object[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                    _obj[i] = dt.Rows[0][i];
                return _obj;
            }
            return null;
        }

        /// <summary>
        /// 获取指定表，指定列，指定条件的记录数。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>获取的记录数</returns>
        public int GetCount(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            this.SqlCmd = "select count(" + this.fieldName + ") from " + this.tableName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return (int)cmd.ExecuteScalar();
        }

        /// <summary>
        /// 需要分页时使用,根据参数和ConditionExpress获取DataTable,不要ConditionExpress中使用order
        /// </summary>
        /// <param name="_tableName">表名</param>
        /// <param name="_fieldNames">字段名集合,用逗号分开</param>
        /// <param name="_OrderColumn">排序字段</param>
        /// <param name="IsDesc">是否倒序</param>
        /// <param name="_indexColumn">自增字段名</param>
        /// <param name="_currentPage">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="_rowsCount">总记录数</param>
        /// <returns>获取到的DataTable</returns>
        public DataTable GetDataTable(string _tableName, string _fieldNames, string _OrderColumn, bool IsDesc, string _indexColumn, int _currentPage, int pageSize, ref int _rowsCount)
        {
            string whereStr = " where ";
            string sort = IsDesc ? " desc" : " asc";
            string sqlStr = " from " + _tableName;
            string orderStr = " order by " + _OrderColumn + sort;
            if (_OrderColumn != _indexColumn)
                orderStr += "," + _indexColumn + sort;
            if (this.ConditionExpress != string.Empty)
            {
                whereStr += this.ConditionExpress;
            }
            sqlStr += whereStr;
            this.SqlCmd = "select count(" + _OrderColumn + ") " + sqlStr;
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            _rowsCount = (int)cmd.ExecuteScalar();

            if (_currentPage > _rowsCount) _currentPage = _rowsCount;

            if (_currentPage > 1)
            {
                if (IsDesc)
                    sqlStr += " and " + _OrderColumn + " < (select MIN(" + _OrderColumn + ") from ";
                else
                    sqlStr += " and " + _OrderColumn + " > (select MAX(" + _OrderColumn + ") from ";
                sqlStr += "(select top " + (pageSize * (_currentPage - 1)) + " " + _OrderColumn + " from " + _tableName + whereStr + orderStr + ") as t)";
            }
            sqlStr = "select top " + pageSize + " " + _fieldNames + sqlStr + orderStr;
            DataSet ds = new DataSet();
            this.cmd.CommandText = sqlStr;
            this.da.SelectCommand = this.cmd;
            this.da.Fill(ds);
            return ds.Tables[0];
        }

        /// <summary>
        /// 根据当前指定的SqlCmd获取DataTable。如果ConditionExpress不为空则会将其清空，所以条件表达式需要包含在SqlCmd中。
        /// </summary>
        /// <returns>返回查询结果DataTable。</returns>
        public DataTable GetDataTable()
        {
            System.Data.DataSet ds = this.GetDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// 根据当前指定的SqlCmd获取DataSet。如果ConditionExpress不为空则会将其清空，所以条件表达式需要包含在SqlCmd中。
        /// </summary>
        /// <returns>返回查询结果DataSet。</returns>
        public DataSet GetDataSet()
        {
            this.alConditionParameters.Clear();
            this.ConditionExpress = string.Empty;
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            DataSet ds = new DataSet();
            this.da.SelectCommand = this.cmd;
            this.da.Fill(ds);
            return ds;
        }
        /// <summary>
        /// 对指定表，指定字段执行加一计数，返回计数后的值。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回计数后的值。</returns>
        public int Count(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            int count = Convert.ToInt32(this.GetValue(this.tableName, this.fieldName));
            count++;
            this.cmd.Parameters.Clear();
            this.cmd.CommandText = string.Empty;
            this.AddFieldItem(_fieldName, count);
            this.Update(this.tableName);
            return count;
        }

        /// <summary>
        /// 对指定表，指定字段执行减一计数，返回计数后的值。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回计数后的值。</returns>
        public int Substract(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            int count = Convert.ToInt32(this.GetValue(this.tableName, this.fieldName));
            if (count > 0) count--;
            this.cmd.Parameters.Clear();
            this.cmd.CommandText = string.Empty;
            this.AddFieldItem(_fieldName, count);
            this.Update(this.tableName);
            return count;
        }

        /// <summary>
        /// 根据ConditionExpress指定的条件在指定表中删除记录。返回删除的记录数。
        /// </summary>
        /// <param name="_tableName">指定的表名称。</param>
        /// <returns>返回删除的记录数。</returns>
        public int Delete(string _tableName)
        {
            this.tableName = _tableName;
            this.SqlCmd = "delete from " + this.tableName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 审核函数。将指定表，指定字段的值进行翻转，如：1->0或0->1。条件由ConditionExpress指定。
        /// </summary>
        /// <param name="_tableName">表名称。</param>
        /// <param name="_fieldName">字段名称。</param>
        /// <returns>返回影响的行数。</returns>
        public int Audit(string _tableName, string _fieldName)
        {
            this.tableName = _tableName;
            this.fieldName = _fieldName;
            this.SqlCmd = "update " + this.tableName + " set " + this.fieldName + "=1-" + this.fieldName;
            if (this.ConditionExpress != string.Empty)
            {
                this.SqlCmd = this.SqlCmd + " where " + this.ConditionExpress;
            }
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            conn.Close();
        }

    }

    /// <summary>
    /// 数据表中的字段属性，包括字段名，字段值。
    /// 常用于保存要提交的数据。
    /// </summary>
    public class DbKeyItem
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="_fieldName">字段名称。</param>
        /// <param name="_fieldValue">字段值。</param>
        public DbKeyItem(string _fieldName, object _fieldValue)
        {
            this.fieldName = _fieldName;
            this.fieldValue = _fieldValue.ToString();
        }
        /// <summary>
        /// 字段名称。
        /// </summary>
        public string fieldName;
        /// <summary>
        /// 字段值。
        /// </summary>
        public string fieldValue;
    }
}
