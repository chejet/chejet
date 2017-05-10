using System;
using System.Data;

namespace XkCms.WebForm.BaseFunction
{
    public class SysInfo
    {
        private string name;
        private string url;
        private string dir;
        private bool isOpen;
        private string closeMessage;
        private string keyWords;
        private string description;
        private bool isTiming;
        private string openTime;
        private bool isHtml;
        private string indexName;
        private bool allowReg;
        private bool onlyEmail;
        private bool checkReg;
        private int userPoint;
        private string managerDir;
        private string editorDir;
        private bool addWater;
        private string waterImage;
        private int regUser = 0;
        private int articleNum = 0;
        private int softNum = 0;
        private int photoNum = 0;
        private int reviewNum = 0;
        private int userupload = 2048;
        private int masterupload = 204800;

        public string Name
        {
            set { name = value; }
            get { return name; }
        }
        public string Url
        {
            set { url = value; }
            get { return url; }
        }
        public string Dir
        {
            set { dir = value; }
            get { return dir; }
        }
        public bool IsOpen
        {
            set { isOpen = value; }
            get { return isOpen; }
        }
        public string CloseMessage
        {
            set { closeMessage = value; }
            get { return closeMessage; }
        }
        public string KeyWords
        {
            set { keyWords = value; }
            get { return keyWords; }
        }
        public string Description
        {
            set { description = value; }
            get { return description; }
        }
        public bool IsTiming
        {
            set { isTiming = value; }
            get { return isTiming; }
        }
        public string OpenTime
        {
            set { openTime = value; }
            get { return openTime; }
        }
        public bool IsHtml
        {
            set { isHtml = value; }
            get { return isHtml; }
        }
        public string IndexName
        {
            set { indexName = value; }
            get { return indexName; }
        }
        public bool AllowReg
        {
            set { allowReg = value; }
            get { return allowReg; }
        }
        public bool OnlyEmail
        {
            set { onlyEmail = value; }
            get { return onlyEmail; }
        }
        public bool CheckUser
        {
            set { checkReg = value; }
            get { return checkReg; }
        }
        public int UserPoint
        {
            set { userPoint = value; }
            get { return userPoint; }
        }
        public string ManagerDir
        {
            set { managerDir = value; }
            get { return managerDir; }
        }
        public string EditorDir
        {
            set { editorDir = value; }
            get { return editorDir; }
        }
        public bool AddWater
        {
            set { addWater = value; }
            get { return addWater; }
        }
        public string WaterImage
        {
            set { waterImage = value; }
            get { return waterImage; }
        }
        public int RegUser
        {
            set { regUser = value; }
            get { return regUser; }
        }
        public int ArticleNum
        {
            set { articleNum = value; }
            get { return articleNum; }
        }
        public int SoftNum
        {
            set { softNum = value; }
            get { return softNum; }
        }
        public int PhotoNum
        {
            set { photoNum = value; }
            get { return photoNum; }
        }
        public int ReviewNum
        {
            set { reviewNum = value; }
            get { return reviewNum; }
        }
        public int UserUpload
        {
            set { userupload = value; }
            get { return userupload; }
        }
        public int MasterUpload
        {
            set { masterupload = value; }
            get { return masterupload; }
        }

        public SysInfo()
        { }

        public SysInfo(XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            object[] _obj = doh.GetValues("Xk_System", "Info,RegUser,ArticleNum,SoftNum,PhotoNum,ReviewNum");
            string[] temp = _obj[0].ToString().Split('|');
            Name = temp[0];
            Url = temp[1];
            Dir = temp[2];
            IsOpen = temp[3] == "1";
            CloseMessage = temp[4];
            KeyWords = temp[5];
            Description = temp[6];
            IsTiming = temp[7] == "1";
            OpenTime = temp[8];
            IsHtml = temp[9] == "0";
            IndexName = temp[10];
            AllowReg = temp[11] == "1";
            OnlyEmail = temp[12] == "1";
            CheckUser = temp[13] == "1";
            ManagerDir = temp[2] + temp[14] + "/";
            EditorDir = temp[2] + temp[15] + "/";
            AddWater = temp[16] == "1";
            WaterImage = temp[2] + temp[17];
            UserPoint = XkCms.Common.Utils.Validator.StrToInt(temp[18], 50);
            UserUpload = XkCms.Common.Utils.Validator.StrToInt(temp[19], 2048);
            MasterUpload = XkCms.Common.Utils.Validator.StrToInt(temp[20], 204800);

            RegUser = XkCms.Common.Utils.Validator.StrToInt(_obj[1].ToString(), 0);
            ArticleNum = XkCms.Common.Utils.Validator.StrToInt(_obj[2].ToString(), 0);
            SoftNum = XkCms.Common.Utils.Validator.StrToInt(_obj[3].ToString(), 0);
            PhotoNum = XkCms.Common.Utils.Validator.StrToInt(_obj[4].ToString(), 0);
            ReviewNum = XkCms.Common.Utils.Validator.StrToInt(_obj[5].ToString(), 0);
        }
    }
}
