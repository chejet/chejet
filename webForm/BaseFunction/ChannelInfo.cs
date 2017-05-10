using System;
using System.Data;
using System.Web;
using XkCms.Common.Utils;
using XkCms.DataOper.Data;

namespace XkCms.WebForm.BaseFunction
{
    public class ChannelInfo
    {
        private int id = 0;
        private string title = string.Empty;
        private string info;
        private string type = "System";
        private string itemname;
        private string itemunit;
        private int templateId;
        private int templateDiss;
        private int dissPageSize;
        private string dir = string.Empty;
        private bool ispost = false;
        private bool isreview = false;
        private bool isout;
        private string outurl;
        private bool target;
        private bool enabeld;
        private int topicNum;
        private int reviewNum;

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
        public string Info
        {
            set { info = value; }
            get { return info; }
        }
        public string Type
        {
            set { type = value; }
            get { return type; }
        }
        public string ItemName
        {
            set { itemname = value; }
            get { return itemname; }
        }
        public string ItemUnit
        {
            set { itemunit = value; }
            get { return itemunit; }
        }
        public int TemplateId
        {
            get { return templateId; }
            set { templateId = value; }
        }
        public int TemplateDiss
        {
            set { templateDiss = value; }
            get { return templateDiss; }
        }
        public int DissPageSize
        {
            set { dissPageSize = value; }
            get { return dissPageSize; }
        }
        public string Dir
        {
            set { dir = value; }
            get { return dir; }
        }
        public bool IsPost
        {
            set { ispost = value; }
            get { return ispost; }
        }
        public bool IsReview
        {
            set { isreview = value; }
            get { return isreview; }
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
        public bool Target
        {
            set { target = value; }
            get { return target; }
        }
        public bool Enabled
        {
            set { enabeld = value; }
            get { return enabeld; }
        }
        public int TopicNum
        {
            set { topicNum = value; }
            get { return topicNum; }
        }
        public int ReviewNum
        {
            set { reviewNum = value; }
            get { return reviewNum; }
        }

        public ChannelInfo()
        {
        }
        public ChannelInfo(string _id, DbOperHandler doh)
        { 



            doh.Reset();
            doh.SqlCmd = "select * from xk_channel where id=" + _id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                Id = Validator.StrToInt(dr["id"].ToString(), 0);
                Title = dr["title"].ToString();
                Info = dr["info"].ToString();
                Type = dr["type"].ToString();
                ItemName = dr["ItemName"].ToString();
                ItemUnit = dr["ItemUnit"].ToString();
                TemplateId = Validator.StrToInt(dr["TemplateId"].ToString(), 0);
                TemplateDiss = Validator.StrToInt(dr["TemplateDiss"].ToString(), 0);
                DissPageSize = Validator.StrToInt(dr["dissPageSize"].ToString(), 20);
                Dir = dr["dir"].ToString();
                IsPost = dr["ispost"].ToString() == "1";
                IsReview = dr["isreview"].ToString() == "1";
                IsOut = dr["isout"].ToString() == "1";
                OutUrl = dr["outurl"].ToString();
                Target = dr["target"].ToString() == "1";
                Enabled = dr["enabled"].ToString() == "1";
                TopicNum = Validator.StrToInt(dr["topicnum"].ToString(), 0);
                ReviewNum = Validator.StrToInt(dr["reviewnum"].ToString(), 0);
            }
            else
            {
                id = 0;
                type = "System";
                dir = "";
            }
        }
    }
}
