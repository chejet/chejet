using System;
using System.Data;

namespace XkCms.DataOper.Data
{
    /// <summary>
    /// ��ʾ���ݿ��������͡�
    /// </summary>
    public enum DatabaseType : byte { SqlServer, OleDb };
    /// <summary>
    /// DbOperHandler ��ժҪ˵����
    /// </summary>
    public abstract class DbOperHandler
    {
        /// <summary>
        /// �����������ͷ��������Դ��
        /// </summary>
        ~DbOperHandler()
        {
            conn.Close();
        }
        /// <summary>
        /// ��ʾ���ݿ����ӵ����ͣ�Ŀǰ֧��SqlServer��OLEDB
        /// </summary>
        public DatabaseType dbType = DatabaseType.OleDb;
        /// <summary>
        /// ���ص�ǰʹ�õ����ݿ����Ӷ���
        /// </summary>
        /// <returns></returns>
        public System.Data.IDbConnection GetConnection()
        {
            return conn;
        }
        /// <summary>
        /// �������ʽ�����������ݿ����ʱɸѡ��¼��ͨ�����ڽ���ָ�������ƺ�ĳ�����ƵĲ�������GetValue()��Delete()�ȣ�֧�ֲ�ѯ��������AddConditionParametersָ������
        /// </summary>
        public string ConditionExpress = string.Empty;
        /// <summary>
        /// ��ǰ��SQL��䡣
        /// </summary>
        public string SqlCmd = string.Empty;
        /// <summary>
        /// ��ǰ�������漰�����ݱ����ơ�
        /// </summary>
        protected string tableName = string.Empty;
        /// <summary>
        /// ��ǰ��������Ƶ��ֶ����ơ�
        /// </summary>
        protected string fieldName = string.Empty;
        /// <summary>
        /// ��ǰ��ʹ�õ����ݿ����ӡ�
        /// </summary>
        protected System.Data.IDbConnection conn;
        /// <summary>
        /// ��ǰ��ʹ�õ��������
        /// </summary>
        protected System.Data.IDbCommand cmd;
        /// <summary>
        /// ��ǰ��ʹ�õ����ݿ���������
        /// </summary>
        protected System.Data.IDbDataAdapter da;


        /// <summary>
        /// ���ڴ洢�ֶ�/ֵ��ԡ�
        /// </summary>
        protected System.Collections.ArrayList alFieldItems = new System.Collections.ArrayList(10);
        /// <summary>
        /// ���ڴ洢SQL����еĲ�ѯ������
        /// </summary>
        protected System.Collections.ArrayList alSqlCmdParameters = new System.Collections.ArrayList(5);
        /// <summary>
        /// ���ڴ洢�������ʽ�еĲ�ѯ������
        /// </summary>
        protected System.Collections.ArrayList alConditionParameters = new System.Collections.ArrayList(5);
        /// <summary>
        /// ��ֵ�ö���ʹ֮�ָ�������ʱ��״̬��
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
        /// ���һ���ֶ�/ֵ�Ե������С�
        /// </summary>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <param name="_fieldValue">�ֶ�ֵ��</param>
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
        /// ����������ʽ�еĲ�ѯ�����������С�ע�⣺�����ݿ�����ΪSqlServerʱ���������Ʊ����SQL���ƥ�䡣������ֻ�豣�����˳��һ�£���������ƥ�䡣
        /// </summary>
        /// <param name="_conditionName">�������ơ�</param>
        /// <param name="_conditionValue">����ֵ��</param>
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
        /// ���SQL����еĲ�ѯ�����������С�ע�⣺�����ݿ�����ΪSqlServerʱ���������Ʊ����SQL���ƥ�䡣������ֻ�豣�����˳��һ�£���������ƥ�䡣
        /// </summary>
        /// <param name="_paraName">�������ơ�</param>
        /// <param name="_paraValue">����ֵ��</param>
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
        /// �жϼ�¼�Ƿ����
        /// </summary>
        /// <param name="tableName">Ҫ��ѯ�����ݱ���</param>
        /// <returns>��/��</returns>
        public bool Exist(string tableName)
        {
            return this.GetValue(tableName, "count(*)").ToString() != "0";
        }
        /// <summary>
        /// �����������ڲ���Command��������Ĳ�����
        /// </summary>
        protected abstract void GenParameters();
        /// <summary>
        /// ���ݵ�ǰalFieldItem�����д洢���ֶ�/ֵ��ָ���������һ�����ݡ��ڸñ��޴�����������·��������������õ��Զ�����idֵ��
        /// </summary>
        /// <param name="_tableName">Ҫ�������ݵı����ơ�</param>
        /// <returns>���ر����������ϲ��������һ���Զ�����idֵ��</returns>
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
        /// ���ݵ�ǰalFieldItem�����д洢���ֶ�/ֵ���������ʽ��ָ�����������������ݿ��еļ�¼��������Ӱ���������
        /// </summary>
        /// <param name="_tableName">Ҫ���µ����ݱ����ơ�</param>
        /// <returns>���ش˴β�����Ӱ�������������</returns>
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
        /// ִ��SqlCmd�е�SQL��䣬������AddSqlCmdParametersָ������ConditionExpress�޹ء�
        /// </summary>
        /// <returns>���ش˴β�����Ӱ�������������</returns>
        public int ExecuteSqlNonQuery()
        {
            this.cmd.CommandText = this.SqlCmd;
            this.GenParameters();
            return cmd.ExecuteNonQuery();
        }
        /// <summary>
        /// ��ȡָ����ָ���У�ָ�������ĵ�һ������������ֵ��
        /// </summary>
        /// <param name="_tableName">�����ơ�</param>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <returns>��ȡ��ֵ�����Ϊ���򷵻�string.Empty��</returns>
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
        /// ��ȡָ����ָ���У�ָ�������ĵ�һ���з���������ֵ�ļ��ϡ�
        /// </summary>
        /// <param name="_tableName">�����ơ�</param>
        /// <param name="_fieldNames">�ֶ�����,�Զ��Ÿ�����</param>
        /// <returns>��ȡ��ֵ�����Ϊ���򷵻�null��</returns>
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
        /// ��ȡָ����ָ���У�ָ�������ļ�¼����
        /// </summary>
        /// <param name="_tableName">�����ơ�</param>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <returns>��ȡ�ļ�¼��</returns>
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
        /// ��Ҫ��ҳʱʹ��,���ݲ�����ConditionExpress��ȡDataTable,��ҪConditionExpress��ʹ��order
        /// </summary>
        /// <param name="_tableName">����</param>
        /// <param name="_fieldNames">�ֶ�������,�ö��ŷֿ�</param>
        /// <param name="_OrderColumn">�����ֶ�</param>
        /// <param name="IsDesc">�Ƿ���</param>
        /// <param name="_indexColumn">�����ֶ���</param>
        /// <param name="_currentPage">��ǰҳ</param>
        /// <param name="pageSize">ҳ��С</param>
        /// <param name="_rowsCount">�ܼ�¼��</param>
        /// <returns>��ȡ����DataTable</returns>
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
        /// ���ݵ�ǰָ����SqlCmd��ȡDataTable�����ConditionExpress��Ϊ����Ὣ����գ������������ʽ��Ҫ������SqlCmd�С�
        /// </summary>
        /// <returns>���ز�ѯ���DataTable��</returns>
        public DataTable GetDataTable()
        {
            System.Data.DataSet ds = this.GetDataSet();
            return ds.Tables[0];
        }
        /// <summary>
        /// ���ݵ�ǰָ����SqlCmd��ȡDataSet�����ConditionExpress��Ϊ����Ὣ����գ������������ʽ��Ҫ������SqlCmd�С�
        /// </summary>
        /// <returns>���ز�ѯ���DataSet��</returns>
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
        /// ��ָ����ָ���ֶ�ִ�м�һ���������ؼ������ֵ��������ConditionExpressָ����
        /// </summary>
        /// <param name="_tableName">�����ơ�</param>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <returns>���ؼ������ֵ��</returns>
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
        /// ��ָ����ָ���ֶ�ִ�м�һ���������ؼ������ֵ��������ConditionExpressָ����
        /// </summary>
        /// <param name="_tableName">�����ơ�</param>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <returns>���ؼ������ֵ��</returns>
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
        /// ����ConditionExpressָ����������ָ������ɾ����¼������ɾ���ļ�¼����
        /// </summary>
        /// <param name="_tableName">ָ���ı����ơ�</param>
        /// <returns>����ɾ���ļ�¼����</returns>
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
        /// ��˺�������ָ����ָ���ֶε�ֵ���з�ת���磺1->0��0->1��������ConditionExpressָ����
        /// </summary>
        /// <param name="_tableName">�����ơ�</param>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <returns>����Ӱ���������</returns>
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
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            conn.Close();
        }

    }

    /// <summary>
    /// ���ݱ��е��ֶ����ԣ������ֶ������ֶ�ֵ��
    /// �����ڱ���Ҫ�ύ�����ݡ�
    /// </summary>
    public class DbKeyItem
    {
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_fieldName">�ֶ����ơ�</param>
        /// <param name="_fieldValue">�ֶ�ֵ��</param>
        public DbKeyItem(string _fieldName, object _fieldValue)
        {
            this.fieldName = _fieldName;
            this.fieldValue = _fieldValue.ToString();
        }
        /// <summary>
        /// �ֶ����ơ�
        /// </summary>
        public string fieldName;
        /// <summary>
        /// �ֶ�ֵ��
        /// </summary>
        public string fieldValue;
    }
}
