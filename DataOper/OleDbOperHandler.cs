using System;

namespace XkCms.DataOper.Data
{
    /// <summary>
    /// �����������ṩ��OLEDB���ݿ�ĳ��÷��ʲ�����
    /// </summary>
    public class OleDbOperHandler : DbOperHandler
    {
        /// <summary>
        /// ���캯��������һ��OLEDB���ݿ����Ӷ���OleDbConnection
        /// </summary>
        /// <param name="_conn"></param>
        public OleDbOperHandler(System.Data.OleDb.OleDbConnection _conn)
        {

            conn = _conn;
            dbType = DatabaseType.OleDb;
            conn.Open();
            cmd = conn.CreateCommand();
            da = new System.Data.OleDb.OleDbDataAdapter();
        }

        /// <summary>
        /// ���캯��������һ��mdb�ļ�
        /// </summary>
        /// <param name="path"></param>
        public OleDbOperHandler(string path)
        {
            conn = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path);
            dbType = DatabaseType.OleDb;
            conn.Open();
            cmd = conn.CreateCommand();
            da = new System.Data.OleDb.OleDbDataAdapter();
        }
        /// <summary>
        /// ����OleDbCommand��������Ĳ�ѯ������
        /// </summary>
        protected override void GenParameters()
        {
            System.Data.OleDb.OleDbCommand oleCmd = (System.Data.OleDb.OleDbCommand)cmd;
            if (this.alFieldItems.Count > 0)
            {
                for (int i = 0; i < alFieldItems.Count; i++)
                {
                    oleCmd.Parameters.AddWithValue("@para" + i.ToString(), ((DbKeyItem)alFieldItems[i]).fieldValue.ToString());
                }
            }

            if (this.alSqlCmdParameters.Count > 0)
            {
                for (int i = 0; i < this.alSqlCmdParameters.Count; i++)
                {
                    oleCmd.Parameters.AddWithValue("@spara" + i.ToString(), ((DbKeyItem)alSqlCmdParameters[i]).fieldValue.ToString());
                }
            }
            if (this.alConditionParameters.Count > 0)
            {
                for (int i = 0; i < this.alConditionParameters.Count; i++)
                {
                    oleCmd.Parameters.AddWithValue("@cpara" + i.ToString(), ((DbKeyItem)alConditionParameters[i]).fieldValue.ToString());
                }
            }
        }

    }
}
