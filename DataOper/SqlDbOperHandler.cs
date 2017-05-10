using System;

namespace XkCms.DataOper.Data
{
    /// <summary>
    /// �����������ṩ��SqlServer���ݿ�ĳ��÷��ʲ�����
    /// </summary>
    public class SqlDbOperHandler : DbOperHandler
    {
        /// <summary>
        /// ���캯��������һ��SqlServer���ݿ����Ӷ���SqlConnection
        /// </summary>
        public SqlDbOperHandler(System.Data.SqlClient.SqlConnection _conn)
        {

            conn = _conn;
            dbType = DatabaseType.SqlServer;

            conn.Open();
            cmd = conn.CreateCommand();
            da = new System.Data.SqlClient.SqlDataAdapter();

        }
        /// <summary>
        /// ����SqlCommand��������Ĳ�ѯ������
        /// </summary>
        protected override void GenParameters()
        {
            System.Data.SqlClient.SqlCommand sqlCmd = (System.Data.SqlClient.SqlCommand)cmd;
            if (this.alFieldItems.Count > 0)
            {
                for (int i = 0; i < alFieldItems.Count; i++)
                {
                    sqlCmd.Parameters.AddWithValue("@para" + i.ToString(), ((DbKeyItem)alFieldItems[i]).fieldValue.ToString());
                }
            }

            if (this.alSqlCmdParameters.Count > 0)
            {
                for (int i = 0; i < this.alSqlCmdParameters.Count; i++)
                {
                    sqlCmd.Parameters.AddWithValue(((DbKeyItem)alSqlCmdParameters[i]).fieldName.ToString(), ((DbKeyItem)alSqlCmdParameters[i]).fieldValue.ToString());
                }
            }
            if (this.alConditionParameters.Count > 0)
            {
                for (int i = 0; i < this.alConditionParameters.Count; i++)
                {
                    sqlCmd.Parameters.AddWithValue(((DbKeyItem)alConditionParameters[i]).fieldName.ToString(), ((DbKeyItem)alConditionParameters[i]).fieldValue.ToString());
                }
            }
        }

    }
}