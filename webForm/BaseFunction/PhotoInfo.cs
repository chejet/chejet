using System;
using System.Data;
using System.Web;
using XkCms.Common.Utils;

namespace XkCms.WebForm.BaseFunction
{
    public class PhotoInfo
    {
        private int _Id = 0;
        private int _ChannelId = 0;
        private int _ColumnId = 0;
        private string _ColumnCode = string.Empty;
        private string _ColumnName = string.Empty;
        private string _Title = string.Empty;
        private DateTime _AddDate = DateTime.Now;
        private string _Summary = string.Empty;
        private string _PhotoUrl = string.Empty;
        private int _ViewNum = 0;
        private int _ReviewNum = 0;
        private int _pageSize = 5;
        private string _Author = string.Empty;
        private int _userid = 0;
        private int _IsPass = 0;
        private string _TColor = string.Empty;
        private int _IsImg = 0;
        private string _Img = string.Empty;
        private int _IsTop = 0;
        private int _DisId = 0;
        private int _SourceId = 0;
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
        public DateTime AddDate
        {
            set { _AddDate = value; }
            get { return _AddDate; }
        }
        public string Summary
        {
            set { _Summary = value; }
            get { return _Summary; }
        }
        public string PhotoUrl
        {
            set { _PhotoUrl = value; }
            get { return _PhotoUrl; }
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
        public int PageSize
        {
            set { _pageSize = value; }
            get { return _pageSize; }
        }
        public string Author
        {
            set { _Author = value; }
            get { return _Author; }
        }
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
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
        public int SourceId
        {
            set { _SourceId = value; }
            get { return _SourceId; }
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

        public PhotoInfo()
        { }

        public PhotoInfo(string id, XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_Photo where id=" + id;
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
                _AddDate = Validator.StrToDate(dr["AddDate"].ToString(), DateTime.Now);
                _Summary = dr["Summary"].ToString();
                _PhotoUrl = dr["PhotoUrl"].ToString();
                _ViewNum = Validator.StrToInt(dr["ViewNum"].ToString(), 0);
                _ReviewNum = Validator.StrToInt(dr["ReviewNum"].ToString(), 0);
                _pageSize = Validator.StrToInt(dr["pageSize"].ToString(), 0);
                _Author = dr["Author"].ToString();
                _IsPass = Validator.StrToInt(dr["IsPass"].ToString(), 0);
                _TColor = dr["TColor"].ToString();
                _IsImg = Validator.StrToInt(dr["IsImg"].ToString(), 0);
                _Img = dr["Img"].ToString();
                _IsTop = Validator.StrToInt(dr["IsTop"].ToString(), 0);
                _DisId = Validator.StrToInt(dr["DisId"].ToString(), 0);
                _SourceId = Validator.StrToInt(dr["SourceId"].ToString(), 0);
                _Correlation = dr["Correlation"].ToString();
                _KeyWord = dr["KeyWord"].ToString();
            }
        }

        public int Add(XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            AddField(ref doh);
            int t = doh.Insert("xk_Photo");
            if (t > 0)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + _ChannelId;
                doh.Count("xk_Channel", "TopicNum");
                doh.Reset();
                doh.Count("xk_System", "PhotoNum");
            }
            return t;
        }

        public int Update(XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            AddField(ref doh);
            doh.ConditionExpress = "id=" + _Id;
            return doh.Update("xk_Photo");
        }

        private void AddField(ref XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.AddFieldItem("ChannelId", _ChannelId);
            doh.AddFieldItem("ColumnId", _ColumnId);
            doh.AddFieldItem("ColumnCode", _ColumnCode);
            doh.AddFieldItem("ColumnName", _ColumnName);
            doh.AddFieldItem("Title", _Title);
            doh.AddFieldItem("AddDate", _AddDate);
            doh.AddFieldItem("Summary", _Summary);
            doh.AddFieldItem("PhotoUrl", _PhotoUrl);
            doh.AddFieldItem("ViewNum", _ViewNum);
            doh.AddFieldItem("ReviewNum", _ReviewNum);
            doh.AddFieldItem("pageSize", _pageSize);
            doh.AddFieldItem("Author", _Author);
            doh.AddFieldItem("IsPass", _IsPass);
            doh.AddFieldItem("TColor", _TColor);
            if (_Img == string.Empty)
            {
                if (PhotoUrl.IndexOf("|||") > -1)
                    _Img = _PhotoUrl.Substring(0, PhotoUrl.IndexOf("|||"));
                else
                    _Img = _PhotoUrl;
            }
            doh.AddFieldItem("IsImg", 1);
            doh.AddFieldItem("Img", _Img);
            doh.AddFieldItem("IsTop", _IsTop);
            doh.AddFieldItem("DisId", _DisId);
            doh.AddFieldItem("SourceId", _SourceId);
            doh.AddFieldItem("Correlation", _Correlation);
            doh.AddFieldItem("KeyWord", _KeyWord);
        }

        public int Del(string id, XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            int t = doh.Delete("xk_Photo");
            if (t > 0)
            {
                doh.Reset();
                doh.ConditionExpress = "id=" + _ChannelId;
                doh.Substract("xk_Channel", "TopicNum");
                doh.Reset();
                doh.Substract("xk_System", "PhotoNum");
            }
            return t;
        }
    }
}
