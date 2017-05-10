using System;
using System.Data;
using System.Web;
using XkCms.Common.Utils;
using XkCms.DataOper.Data;

namespace XkCms.WebForm.BaseFunction
{
    public class ColumnInfo
    {
        private int id = 0;
        private string title = string.Empty;
        private string code;
        private int parentId;
        private int channelid = 0;
        private string info;
        private bool isout;
        private string outurl;
        private bool isreview;
        private bool ispost;
        private bool istop;
        private int templateId;
        private int contenttempid;
        private int pagesize;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Title
        {
            set { title = value; }
            get { return title; }
        }
        public string Code
        {
            set { code = value; }
            get { return code; }
        }
        public int ParentId
        {
            set { parentId = value; }
            get { return parentId; }
        }
        public int ChannelId
        {
            set { channelid = value; }
            get { return channelid; }
        }
        public string Info
        {
            set { info = value; }
            get { return info; }
        }
        public bool IsOut
        {
            set { isout = value; }
            get { return isout; }
        }
        public string OutUrl
        {
            set { outurl = value; }
            get { return outurl; }
        }
        public bool IsReview
        {
            set { isreview = value; }
            get { return isreview; }
        }
        public bool IsPost
        {
            set { ispost = value; }
            get { return ispost; }
        }
        public bool IsTop
        {
            set { istop = value; }
            get { return istop; }
        }
        public int TemplateId
        {
            get { return templateId; }
            set { templateId = value; }
        }
        public int ContentTempId
        {
            set { contenttempid = value; }
            get { return contenttempid; }
        }
        public int PageSize
        {
            set { pagesize = value; }
            get { return pagesize; }
        }

        public ColumnInfo()
        {
        }

        public ColumnInfo(string _id, DbOperHandler doh)
        {
            doh.Reset();
            doh.SqlCmd = "select * from Xk_Column where id=" + _id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Id = Validator.StrToInt(dr["id"].ToString(), 0);
                Title = dr["title"].ToString();
                Code = dr["code"].ToString();
                ParentId = Validator.StrToInt(dr["parentId"].ToString(), 0);
                ChannelId = Validator.StrToInt(dr["channelid"].ToString(), 0);
                Info = dr["info"].ToString();
                IsOut = dr["isout"].ToString() == "1";
                OutUrl = dr["outurl"].ToString();
                IsReview = dr["isreview"].ToString() == "1";
                IsPost = dr["ispost"].ToString() == "1";
                IsTop = dr["istop"].ToString() == "1";
                TemplateId = Validator.StrToInt(dr["templateid"].ToString(), 0);
                ContentTempId = Validator.StrToInt(dr["contenttemp"].ToString(), 0);
                PageSize = Validator.StrToInt(dr["pagesize"].ToString(), 20);
            }
        }
    }
}
