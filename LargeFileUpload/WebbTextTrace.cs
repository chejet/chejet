using System;
using System.IO;
using System.Web;

namespace XkCms.LargeFileUpload
{
    /// <summary>
    /// ����վ���ڵ�����·���д���һ���ı��ļ�����������������ݡ�
    /// �ļ���Ϊ��TraceLog.ashx
    /// </summary>
    public class WebbTextTrace
    {
        //static string m_logFilePath	= typeof(WebbSystem).Assembly.Location;//= @"C:\Inetpub\wwwroot\WebbTest\tracelog.txt";
        static string m_logFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "TraceLog.ashx");
        public WebbTextTrace(string str_traceMsg)
        {
            string m_logFilePathDynamic = Path.Combine(Path.GetDirectoryName(typeof(WebbTextTrace).Assembly.Location), "TraceLog.ashx");
            StreamWriter obj_Writer;
            obj_Writer = File.AppendText(m_logFilePathDynamic);
            obj_Writer.WriteLine(DateTime.Now.ToString() + "\t" + str_traceMsg);
            obj_Writer.Close();
            obj_Writer = null;
        }
        /// <summary>
        /// TraceMsg:Write down some message into a txet file.
        /// Your can use this function for dbug, or log.
        /// </summary>
        /// <param name="str_traceMsg"></param>
        /// <param name="str_filePath"></param>
        static public void TraceMsg(string str_traceMsg, string str_fileName)
        {
            StreamWriter obj_Writer;
            obj_Writer = File.AppendText(str_fileName);
            obj_Writer.WriteLine(DateTime.Now.ToString() + "\t" + str_traceMsg);
            obj_Writer.Close();
            obj_Writer = null;
        }
        static public void TraceMsg(string str_traceMsg)
        {
            TraceMsg(str_traceMsg, m_logFilePath);
        }//End function:TraceMsg(2);
    }
}
