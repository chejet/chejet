using System;
using System.Data;
using System.Web.UI.WebControls;
using XkCms.WebForm.BaseFunction;

namespace XkCms.WebForm.Manager
{
    public partial class UserGroup : Admin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Xkzi_Load("013");
        }

        protected override void editBox()
        {
            if (!IsPostBack)
            {
                string Grade = string.Empty;
                string[] GroupSet;
                if (id == "0")
                {
                    doh.Reset();
                    doh.ConditionExpress = "Grades = 1";
                    Grade = doh.GetValue("Xk_UserGroup", "GroupSet").ToString();
                    doh.Reset();
                    doh.ConditionExpress = "Grades < 999 order by Grades desc";
                    int temp = XkCms.Common.Utils.Validator.StrToInt(doh.GetValue("xk_userGroup", "Grades").ToString(), 4);
                    if (temp < 4) temp = 4;
                    txtGrades.Text = temp.ToString();
                }
                else
                {
                    doh.Reset();
                    doh.ConditionExpress = "id=" + id;
                    object[] _obj = doh.GetValues("xk_userGroup", "GroupName,GroupSet,Grades");
                    if (_obj != null)
                    {
                        txtName.Text = _obj[0].ToString();
                        Grade = _obj[1].ToString();
                        txtGrades.Text = _obj[2].ToString();
                        txtGrades.Enabled = false;
                        RangeValidator1.Enabled = false;
                    }
                }
                Grade += "0|0|0|0|0|0|0|0|0|0|"; 
                GroupSet = Grade.Split('|');
                GroupSet0.Items.FindByValue(GroupSet[0]).Selected = true;
                GroupSet1.Items.FindByValue(GroupSet[1]).Selected = true;
                GroupSet2.Items.FindByValue(GroupSet[2]).Selected = true;
                GroupSet3.Items.FindByValue(GroupSet[3]).Selected = true;
                GroupSet4.Items.FindByValue(GroupSet[4]).Selected = true;
                GroupSet5.Text = GroupSet[5];
                GroupSet6.Text = GroupSet[6];
                GroupSet7.Items.FindByValue(GroupSet[7]).Selected = true;
                GroupSet8.Items.FindByValue(GroupSet[8]).Selected = true;
                GroupSet9.Text = GroupSet[9];
                GroupSet10.Text = GroupSet[10];
                GroupSet11.Items.FindByValue(GroupSet[11]).Selected = true;
                GroupSet12.Items.FindByValue(GroupSet[12]).Selected = true;
                GroupSet13.Text = GroupSet[13];
                GroupSet14.Text = GroupSet[14];
                GroupSet15.Items.FindByValue(GroupSet[15]).Selected = true;
                GroupSet16.Text = GroupSet[16];
                GroupSet17.Text = GroupSet[17];
                GroupSet18.Text = GroupSet[18];
                GroupSet19.Text = GroupSet[19];
                GroupSet20.Items.FindByValue(GroupSet[20]).Selected = true;
                GroupSet21.Text = GroupSet[21];
                GroupSet22.Items.FindByValue(GroupSet[22]).Selected = true;
                GroupSet23.Text = GroupSet[23];
                GroupSet24.Text = GroupSet[24];
                GroupSet25.Text = GroupSet[25];
                GroupSet26.Text = GroupSet[26];
                GroupSet27.Text = GroupSet[27];
                GroupSet28.Text = GroupSet[28];
                GroupSet29.Text = GroupSet[29];
                GroupSet30.Items.FindByValue(GroupSet[30]).Selected = true;
                GroupSet31.Items.FindByValue(GroupSet[31]).Selected = true;
                GroupSet32.Text = GroupSet[32];
                GroupSet33.Text = GroupSet[33];
                GroupSet34.Items.FindByValue(GroupSet[34]).Selected = true;
                GroupSet35.Items.FindByValue(GroupSet[35]).Selected = true;
                GroupSet36.Items.FindByValue(GroupSet[36]).Selected = true;
                GroupSet37.Text = GroupSet[37];
                GroupSet38.Text = GroupSet[38];
                GroupSet39.Text = GroupSet[39];
                GroupSet40.Text = GroupSet[40];
            }
        }

        protected override void getListBox()
        {
            plAdd.Visible = false;
            plList.Visible = true;
            doh.Reset();
            doh.SqlCmd = "select * from Xk_UserGroup order by id";
            gvList.DataSource = doh.GetDataTable();
            gvList.DataKeyNames = new string[] { "id" };
            gvList.DataBind();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GvRowAddFun(ref e, 3, 1);
        }

        protected void gvList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            int uGrades = XkCms.Common.Utils.Validator.StrToInt(doh.GetValue("xk_userGroup", "grades").ToString(), 0);
            if (uGrades == 999 || uGrades < 4)
            {
                Alert("系统默认用户组不能删除！");
                return;
            }
            doh.Reset();
            doh.ConditionExpress = "id=" + id;
            doh.Delete("xk_usergroup");
            doh.Reset();
            doh.ConditionExpress = "UserGrade=" + uGrades;
            doh.AddFieldItem("UserGrade", 1);
            doh.Update("xk_User");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (id == "0")
            {
                doh.Reset();
                doh.ConditionExpress = "Grades=" + txtGrades.Text.Trim();
                bool IsExist = doh.GetCount("xk_UserGroup", "Grades") > 0;
                if (IsExist)
                {
                    Alert("存在等级 " + txtGrades.Text + " ，请重新输入用户等级！");
                    return;
                }
            }
            string GroupSet = string.Empty;
            for (int i = 0; i < 41; i++)
                GroupSet += f("GroupSet" + i) + "|";
            doh.Reset();
            doh.AddFieldItem("Groupname", txtName.Text.Trim());
            doh.AddFieldItem("Grades", txtGrades.Text);
            doh.AddFieldItem("Groupset", GroupSet);
            if (id == "0")
            {
                doh.AddFieldItem("UserTotal", 0);
                doh.Insert("Xk_UserGroup");
            }
            else
            {
                doh.ConditionExpress = "id=" + id;
                doh.Update("Xk_UserGroup");
            }
            GetList();
        }
    }
}
