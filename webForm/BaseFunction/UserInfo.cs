using System;
using System.Data;
using XkCms.Common.Utils;

namespace XkCms.WebForm.BaseFunction
{
    public class UserInfo
    {
        private int _Id = 0;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _nickname = string.Empty;
        private int _UserGrade = 1;
        private string _UserGroup = string.Empty;
        private int _IsPass = 1;
        private string _UserFace = "face/1.gif";
        private int _userpoint = 0;
        private int _experience = 10;
        private int _charm = 10;
        private string _UserSign = string.Empty;
        private string _TrueName = string.Empty;
        private string _UserIDCard = string.Empty;
        private string _UserSex = string.Empty;
        private string _usermail = string.Empty;
        private string _HomePage = string.Empty;
        private string _phone = string.Empty;
        private string _oicq = string.Empty;
        private string _postcode = string.Empty;
        private string _address = string.Empty;
        private string _question = string.Empty;
        private string _answer = string.Empty;
        private DateTime _JoinTime = DateTime.Now;
        private DateTime _LastTime = DateTime.Now;
        private int _usermsg = 0;
        private string _userlastip = string.Empty;
        private int _userlogin = 0;
        private string _UserToday = "0|0|0|0|0|0";
        private string _usersetting = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,";


        public int Id
        {
            set { _Id = value; }
            get { return _Id; }
        }
        public string Username
        {
            set { _username = value; }
            get { return _username; }
        }
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        public string Nickname
        {
            set { _nickname = value; }
            get { return _nickname; }
        }
        public int UserGrade
        {
            set { _UserGrade = value; }
            get { return _UserGrade; }
        }
        public string UserGroup
        {
            set { _UserGroup = value; }
            get { return _UserGroup; }
        }
        public int IsPass
        {
            set { _IsPass = value; }
            get { return _IsPass; }
        }
        public string UserFace
        {
            set { _UserFace = value; }
            get { return _UserFace; }
        }
        public int Userpoint
        {
            set { _userpoint = value; }
            get { return _userpoint; }
        }
        public int Experience
        {
            set { _experience = value; }
            get { return _experience; }
        }
        public int Charm
        {
            set { _charm = value; }
            get { return _charm; }
        }
        public string UserSign
        {
            set { _UserSign = value; }
            get { return _UserSign; }
        }
        public string TrueName
        {
            set { _TrueName = value; }
            get { return _TrueName; }
        }
        public string UserIDCard
        {
            set { _UserIDCard = value; }
            get { return _UserIDCard; }
        }
        public string UserSex
        {
            set { _UserSex = value; }
            get { return _UserSex; }
        }
        public string UserMail
        {
            set { _usermail = value; }
            get { return _usermail; }
        }
        public string HomePage
        {
            set { _HomePage = value; }
            get { return _HomePage; }
        }
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        public string Oicq
        {
            set { _oicq = value; }
            get { return _oicq; }
        }
        public string Postcode
        {
            set { _postcode = value; }
            get { return _postcode; }
        }
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        public string Question
        {
            set { _question = value; }
            get { return _question; }
        }
        public string Answer
        {
            set { _answer = value; }
            get { return _answer; }
        }
        public DateTime JoinTime
        {
            set { _JoinTime = value; }
            get { return _JoinTime; }
        }
        public DateTime LastTime
        {
            set { _LastTime = value; }
            get { return _LastTime; }
        }
        public int Usermsg
        {
            set { _usermsg = value; }
            get { return _usermsg; }
        }
        public string Userlastip
        {
            set { _userlastip = value; }
            get { return _userlastip; }
        }
        public int Userlogin
        {
            set { _userlogin = value; }
            get { return _userlogin; }
        }
        public string UserToday
        {
            set { _UserToday = value; }
            get { return _UserToday; }
        }
        public string Usersetting
        {
            set { _usersetting = value; }
            get { return _usersetting; }
        }

        public UserInfo()
        { }

        public UserInfo(string _id, XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.SqlCmd = "select * from xk_user where id=" + _id;
            DataTable dt = doh.GetDataTable();
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                _Id = Validator.StrToInt(dr["Id"].ToString(), 0);
                _username = dr["username"].ToString();
                _password = dr["password"].ToString();
                _nickname = dr["nickname"].ToString();
                _UserGrade = Validator.StrToInt(dr["UserGrade"].ToString(), 0);
                _UserGroup = dr["UserGroup"].ToString();
                _IsPass = Validator.StrToInt(dr["IsPass"].ToString(), 0);
                _UserFace = dr["UserFace"].ToString();
                _userpoint = Validator.StrToInt(dr["userpoint"].ToString(), 0);
                _experience = Validator.StrToInt(dr["experience"].ToString(), 0);
                _charm = Validator.StrToInt(dr["charm"].ToString(), 0);
                _UserSign = dr["UserSign"].ToString();
                _TrueName = dr["TrueName"].ToString();
                _UserIDCard = dr["UserIDCard"].ToString();
                _UserSex = dr["UserSex"].ToString();
                _usermail = dr["usermail"].ToString();
                _HomePage = dr["HomePage"].ToString();
                _phone = dr["phone"].ToString();
                _oicq = dr["oicq"].ToString();
                _postcode = dr["postcode"].ToString();
                _address = dr["address"].ToString();
                _question = dr["question"].ToString();
                _answer = dr["answer"].ToString();
                _JoinTime = Validator.StrToDate(dr["JoinTime"].ToString(), DateTime.Now);
                _LastTime = Validator.StrToDate(dr["LastTime"].ToString(), DateTime.Now);
                _usermsg = Validator.StrToInt(dr["usermsg"].ToString(), 0);
                _userlastip = dr["userlastip"].ToString();
                _userlogin = Validator.StrToInt(dr["userlogin"].ToString(), 0);
                _UserToday = dr["UserToday"].ToString();
                _usersetting = dr["usersetting"].ToString();
            }
        }

        public int Add(XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.ConditionExpress = "username=@username";
            doh.AddSqlCmdParameters("@username", _username);
            if (doh.GetCount("xk_user", "id") > 0) return 0;
            AddField(ref doh);
            int temp = doh.Insert("xk_user");
            if (temp > 0)
            {
                doh.Reset();
                doh.ConditionExpress = "Grades=" + _UserGrade;
                doh.Count("Xk_UserGroup", "UserTotal");

                doh.Reset();
                doh.Count("Xk_System", "RegUser");
            }
            return temp;
        }

        public int Update(XkCms.DataOper.Data.DbOperHandler doh)
        {
            AddField(ref doh);
            doh.ConditionExpress = "id=" + _Id;
            return doh.Update("xk_user");
        }

        private void AddField(ref XkCms.DataOper.Data.DbOperHandler doh)
        {
            doh.Reset();
            doh.AddFieldItem("username", _username);
            if (_password != null && _password != string.Empty)
                doh.AddFieldItem("password", _password);
            doh.AddFieldItem("nickname", _nickname);
            doh.AddFieldItem("UserGrade", _UserGrade);
            doh.AddFieldItem("UserGroup", _UserGroup);
            doh.AddFieldItem("IsPass", _IsPass);
            doh.AddFieldItem("UserFace", _UserFace);
            doh.AddFieldItem("userpoint", _userpoint);
            doh.AddFieldItem("experience", _experience);
            doh.AddFieldItem("charm", _charm);
            doh.AddFieldItem("UserSign", _UserSign);
            doh.AddFieldItem("TrueName", _TrueName);
            doh.AddFieldItem("UserIDCard", _UserIDCard);
            doh.AddFieldItem("UserSex", _UserSex);
            doh.AddFieldItem("usermail", _usermail);
            doh.AddFieldItem("HomePage", _HomePage);
            doh.AddFieldItem("phone", _phone);
            doh.AddFieldItem("oicq", _oicq);
            doh.AddFieldItem("postcode", _postcode);
            doh.AddFieldItem("address", _address);
            doh.AddFieldItem("question", _question);
            if (_answer != null && _answer != string.Empty)
                doh.AddFieldItem("answer", _answer);
            doh.AddFieldItem("JoinTime", _JoinTime);
            doh.AddFieldItem("LastTime", _LastTime);
            doh.AddFieldItem("usermsg", _usermsg);
            doh.AddFieldItem("userlastip", _userlastip);
            doh.AddFieldItem("userlogin", _userlogin);
            doh.AddFieldItem("UserToday", _UserToday);
            doh.AddFieldItem("usersetting", _usersetting);
        }
    }
}
