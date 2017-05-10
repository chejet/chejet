using System;
using System.Data;
using System.Web;
using XkCms.Common.Utils;

namespace XkCms.WebForm.BaseFunction
{
    public class ArticleInfo
    {
        private int _Id = 0;
        private int _ChannelId = 0;
        private int _ColumnId = 0;
        private string _ColumnCode = string.Empty;
        private string _ColumnName = string.Empty;
        private string _Title = string.Empty;
        private string _SubTitle = string.Empty;
        private DateTime _AddDate = DateTime.Now;
        private string _Content = string.Empty;
        private int _ViewNum = 0;
        private int _ReviewNum = 0;
        private string _Author = string.Empty;
        private int _UserId = 0;
        private int _IsPass = 0;
        private string _TColor = string.Empty;
        private int _IsImg = 0;
        private string _Img = string.Empty;
        private int _IsTop = 0;
        private int _DisId = 0;
        private string _OutUrl = string.Empty;
        private int _IsOut = 0;
        private int _SourceId = 0;
        private string _Summary = string.Empty;
        private string _Correlation = string.Empty;
        private string _KeyWord = string.Empty;


        public int Id
        {
            set { _Id = value; }
            get { return _Id; }
        }
        public int ChannelId
        {
            set { _ChannelId = value; }
            get { return _ChannelId; }
        }
        public int ColumnId
        {
            set { _ColumnId = value; }
            get { return _ColumnId; }
        }
        public string ColumnCode
        {
            set { _ColumnCode = value; }
            get { return _ColumnCode; }
        }
        public string ColumnName
        {
            set { _ColumnName = value; }
            get { return _ColumnName; }
        }
        public string Title
        {
            set { _Title = value; }
            get { return _Title; }
        }
        public string SubTitle
        {
            set { _SubTitle = value; }
            get { return _SubTitle; }
        }
        public DateTime AddDate
        {
            set { _AddDate = value; }
            get { return _AddDate; }
        }
        public string Content
        {
            set { _Content = value; }
            get { return _Content; }
        }
        public int ViewNum
        {
            set { _ViewNum = value; }
            get { return _ViewNum; }
        }
        public int ReviewNum
        {
            set { _ReviewNum = value; }
            get { return _ReviewNum; }
        }
        public string Author
        {
            set { _Author = value; }
            get { return _Author; }
        }
        public int UserId
        {
            get { return _UserId; }
            set { _UserId = value; }
        }
        public int IsPass
        {
            set { _IsPass = value; }
            get { return _IsPass; }
        }
        public string TColor
        {
            set { _TColor = value; }
            get { return _TColor; }
        }
        public int IsImg
        {
            set { _IsImg = value; }
            get { return _IsImg; }
        }
        public string Img
        {
            set { _Img = value; }
            get { return _Img; }
        }
        public int IsTop
        {
            set { _IsTop = value; }
            get { return _IsTop; }
        }
        public int DisId
        {
            set { _DisId = value; }
            get { return _DisId; }
        }
        public string OutUrl
        {
            set { _OutUrl = value; }
            get { return _OutUrl; }
        }
        public int IsOut
        {
            set { _IsOut = value; }
            get { return _IsOut; }
        }
        public int SourceId
        {
            set { _SourceId = value; }
            get { return _SourceId; }
        }
        public string Summary
        {
            set { _Summary = value; }
            get { return _Summary; }
        }
        public string Correlation
        {
            set { _Correlation = value; }
            get { return _Correlation; }
        }
        public string KeyWord
        {
            set { _KeyWord = value; }
            get { return _KeyWord; }
        }

        public ArticleInfo()
        { }

        public ArticleInfo(string id, XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_Article where id=" + id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                _Id = Validator.StrToInt(dr["Id"].ToString(), 0);
                _ChannelId = Validator.StrToInt(dr["ChannelId"].ToString(), 0);
                _ColumnId = Validator.StrToInt(dr["ColumnId"].ToString(), 0);
                _ColumnCode = dr["ColumnCode"].ToString();
                _ColumnName = dr["ColumnName"].ToString();
                _Title = dr["Title"].ToString();
                _SubTitle = dr["SubTitle"].ToString();
                _AddDate = Validator.StrToDate(dr["AddDate"].ToString(), DateTime.Now);
                _Content = dr["Content"].ToString();
                _ViewNum = Validator.StrToInt(dr["ViewNum"].ToString(), 0);
                _ReviewNum = Validator.StrToInt(dr["ReviewNum"].ToString(), 0);
                _Author = dr["Author"].ToString();
                _UserId = Validator.StrToInt(dr["userid"].ToString(), 0);
                _IsPass = Validator.StrToInt(dr["IsPass"].ToString(), 0);
                _TColor = dr["TColor"].ToString();
                _IsImg = Validator.StrToInt(dr["IsImg"].ToString(), 0);
                _Img = dr["Img"].ToString();
                _IsTop = Validator.StrToInt(dr["IsTop"].ToString(), 0);
                _DisId = Validator.StrToInt(dr["DisId"].ToString(), 0);
                _OutUrl = dr["OutUrl"].ToString();
                _IsOut = Validator.StrToInt(dr["IsOut"].ToString(), 0);
                _SourceId = Validator.StrToInt(dr["SourceId"].ToString(), 0);
                _Summary = dr["Summary"].ToString();
                _Correlation = dr["Correlation"].ToString();
                _KeyWord = dr["KeyWord"].ToString();
            }
        }

        public int Add(XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            AddField(ref doh);
            int t = doh.Insert("xk_Article");
            if (t > 0)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + _ChannelId;
                doh.Count("xk_Channel", "TopicNum");
                doh.Reset();
                doh.Count("xk_System", "ArticleNum");
            }
            return t;
        }

        public int Update(XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            AddField(ref doh);
            doh.ConditionExpress = "id=" + _Id;
            return doh.Update("xk_Article");
        }

        private void AddField(ref XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.AddFieldItem("ChannelId", _ChannelId);
            doh.AddFieldItem("ColumnId", _ColumnId);
            doh.AddFieldItem("ColumnCode", _ColumnCode);
            doh.AddFieldItem("ColumnName", _ColumnName);
            doh.AddFieldItem("Title", _Title);
            doh.AddFieldItem("SubTitle", _SubTitle);
            doh.AddFieldItem("AddDate", _AddDate);
            doh.AddFieldItem("Content", _Content);
            doh.AddFieldItem("ViewNum", _ViewNum);
            doh.AddFieldItem("ReviewNum", _ReviewNum);
            doh.AddFieldItem("UserId", _UserId);
            doh.AddFieldItem("Author", _Author);
            doh.AddFieldItem("IsPass", _IsPass);
            doh.AddFieldItem("TColor", _TColor);
            if (_Img == string.Empty)
                _IsImg = 0;
            else
                _IsImg = 1;
            doh.AddFieldItem("IsImg", _IsImg);
            doh.AddFieldItem("Img", _Img);
            doh.AddFieldItem("IsTop", _IsTop);
            doh.AddFieldItem("DisId", _DisId);
            doh.AddFieldItem("OutUrl", _OutUrl);
            if (_OutUrl == string.Empty)
                _IsOut = 0;
            else
                _IsOut = 1;
            doh.AddFieldItem("IsOut", _IsOut);
            doh.AddFieldItem("SourceId", _SourceId);
            doh.AddFieldItem("Summary", _Summary);
            doh.AddFieldItem("Correlation", _Correlation);
            doh.AddFieldItem("KeyWord", _KeyWord);
        }

        public int Del(string id, XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            int t = doh.Delete("xk_Article");
            if (t > 0)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + _ChannelId;
                doh.Substract("xk_Channel", "TopicNum");
                doh.Reset();
                doh.Substract("xk_System", "ArticleNum");
            }
            return t;
        }
    }
}
