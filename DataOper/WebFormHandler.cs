using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Web;
using System.Collections;

namespace XkCms.DataOper.Data
{

    /// <summary>
    /// ������е����ݣ��ṩ���ݿ���ӡ��޸ĵ�ͨ�ô�����̡�
    /// ����ύ�����ݱ�ί����֤����Ϊ��Ч�����κζ���������������������������¼���
    /// </summary>
    public class WebFormHandler
    {
        /// <summary>
        /// ����Ҫ�󶨵Ŀؼ���object����ʽ�洢�ڴ������С�
        /// </summary>
        protected ArrayList alBinderItems = new ArrayList(8);
        /// <summary>
        /// ���ݿ����ӣ��ṩ���ݷ��ʲ�Ĳ�����
        /// </summary>
        protected XkCms.DataOper.Data.DbOperHandler doh;

        /// <summary>
        /// ���ڴ�Ŵ����ݿ���ȡ�������ݼ�¼��
        /// </summary>
        protected DataTable myDt;
        //ָʾ������ύ�����Ƿ�ͨ����֤

        /// <summary>
        /// ��ʾ��ǰ�Ĳ������ͣ���ӻ��޸ģ�Ĭ��Ϊ��ӡ�
        /// </summary>
        private OperationType _mode = OperationType.Add;
        /// <summary>
        /// ָ����ǰ�Ĳ������ͣ�����ָ��Ϊ��ӻ��޸ġ�
        /// </summary>
        public OperationType Mode
        {
            get { return _mode; }
            set
            {

                _mode = value;
                if (value == OperationType.Add)
                {
                    btnSubmit.Text = AddText;
                }
                else if (value == OperationType.Modify)
                {
                    btnSubmit.Text = ModifyText;
                }
                else
                {
                    btnSubmit.Text = UnknownText;
                }
            }
        }

        /// <summary>
        /// �ύ��ť�����״̬����ʾ���ı���
        /// </summary>
        public string AddText = "�� ��";
        /// <summary>
        /// �ύ��ť���޸�״̬����ʾ���ı���
        /// </summary>
        public string ModifyText = "�� ��";

        /// <summary>
        /// �ύ��ť��δ֪״̬����ʾ���ı���
        /// </summary>
        public string UnknownText = "δ ֪";

        /// <summary>
        /// �����ݿ���ȡ����ʱ���������ʽ��
        /// </summary>
        private string _conditionExpress = string.Empty;
        /// <summary>
        /// �洢ȡ�����ݵı����ơ�
        /// </summary>
        private string _tableName = string.Empty;

        /// <summary>
        /// ���ڴ洢���е��ύ��ť�����á�
        /// </summary>
        protected System.Web.UI.WebControls.Button btnSubmit = null;

        /// <summary>
        /// ָ��ȡ�����ݵı����ơ�
        /// </summary>
        public string TableName
        {
            set { _tableName = value; }
            get { return _tableName; }
        }

        /// <summary>
        /// ָ�������ݿ���ȡ����ʱ���������ʽ��
        /// </summary>
        public string ConditionExpress
        {
            set { _conditionExpress = value; }
            get { return _conditionExpress; }
        }

        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_doh">���ݿ��������</param>
        /// <param name="_table">ʹ�õ����ݱ�����ơ�</param>
        /// <param name="_btn">���е��ύ��ť��</param>
        public WebFormHandler(XkCms.DataOper.Data.DbOperHandler _doh, string _table, Button _btn)
        {
            TableName = _table;
            doh = _doh;
            btnSubmit = _btn;
            this.CheckArgs();
            if (!btnSubmit.Page.IsPostBack && this.Mode == OperationType.Modify) this.BindWhenModify();
            btnSubmit.Click += new EventHandler(ProcessTheForm);
            btnSubmit.Page.PreRender += new EventHandler(this.Page_PreRender);
        }

        /// <summary>
        /// ��Ҫ�޸�����ʱȡ�����ݿ��еļ�¼��䵽���С�
        /// </summary>
        public void BindWhenModify()
        {
            if (alBinderItems.Count == 0) return;
            BinderItem bi;
            int i = 0;

            StringBuilder sbSqlCmd = new StringBuilder("select top 1 ");
            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                sbSqlCmd.Append(bi.field);
                sbSqlCmd.Append(",");
            }
            sbSqlCmd.Remove(sbSqlCmd.Length - 1, 1);//ȥ�������һ������
            sbSqlCmd.Append(" from ");
            sbSqlCmd.Append(TableName);
            sbSqlCmd.Append(" where 1=1 and ");
            sbSqlCmd.Append(this.ConditionExpress);

            doh.Reset();
            doh.SqlCmd = sbSqlCmd.ToString();
            myDt = doh.GetDataTable();
            //���ָ����¼���������׳��쳣
            if (myDt.Rows.Count == 0) throw new ArgumentException("Record does not exist.");

            DataRow dr = myDt.Rows[0];
            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                bi.SetValue(dr[bi.field].ToString());
            }
            this.OnBindBeforeModifyOk(System.EventArgs.Empty);
        }


        /// <summary>
        /// ����������Ϊ���ʱ��������ӵ����ݿ���
        /// </summary>
        protected void Add()
        {
            if (!DataValid()) return;
            if (alBinderItems.Count == 0) return;
            BinderItem bi;
            int i = 0;
            doh.Reset();

            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                //����ǵ�ѡ��ť��û��ѡ������������ֶ�
                if (bi.o is MyRadioButton)
                {
                    MyRadioButton mrb = (MyRadioButton)bi.o;
                    if (mrb.rb.Checked == false) continue;
                }

                doh.AddFieldItem(bi.field, bi.GetValue());
            }
            
            int id = doh.Insert(this.TableName);
            this.OnAddOk(new XkCms.DataOper.Data.DbOperEventArgs(id));
        }

        /// <summary>
        /// ����������Ϊ�޸�ʱ��������µ����ݿ���
        /// </summary>
        protected void Update()
        {
            if (!DataValid()) return;
            if (alBinderItems.Count == 0) return;
            BinderItem bi;
            int i = 0;
            doh.Reset();
            for (i = 0; i < alBinderItems.Count; i++)
            {
                bi = (BinderItem)alBinderItems[i];
                //����ǵ�ѡ��ť��û��ѡ������������ֶ�
                if (bi.o is MyRadioButton)
                {
                    MyRadioButton mrb = (MyRadioButton)bi.o;
                    if (mrb.rb.Checked == false) continue;
                }

                doh.AddFieldItem(bi.field, bi.GetValue());
            }

            doh.ConditionExpress = this.ConditionExpress;
            doh.Update(this.TableName);
            this.OnModifyOk(System.EventArgs.Empty);
        }

        /// <summary>
        /// �����ı���ؼ������ݿ��ֶεİ�
        /// </summary>
        /// <param name="tb">��Ҫ�󶨵��ı������</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(TextBox tb, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(tb, field, isStringType));
        }

        /// <summary>
        /// ����������ؼ������ݿ��ֶεİ�
        /// </summary>
        /// <param name="ddl">��Ҫ�󶨵����������</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(DropDownList ddl, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(ddl, field, isStringType));
        }

        /// <summary>
        /// ������ѡ��ؼ������ݿ��ֶεİ�
        /// </summary>
        /// <param name="cb">��Ҫ�󶨵ĸ�ѡ�����</param>
        /// <param name="_value">��ѡ��ѡ��ʱ��Ӧ�ֶ�Ӧ����д��ֵ��</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(CheckBox cb, string _value, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(new MyCheckBox(cb, _value), field, isStringType));
        }

        /// <summary>
        /// ������ѡ��ؼ������ݿ��ֶεİ�
        /// </summary>
        /// <param name="rb">��Ҫ�󶨵ĵ�ѡ�����</param>
        /// <param name="_value">��ѡ��ѡ��ʱ��Ӧ�ֶ�Ӧ����д��ֵ��</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(RadioButton rb, string _value, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(new MyRadioButton(rb, _value), field, isStringType));
        }

        /// <summary>
        /// ����Label�ؼ������ݿ��ֶεİ�
        /// </summary>
        /// <param name="lbl">��Ҫ�󶨵�Label����</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(Label lbl, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(lbl, field, isStringType));
        }

        /// <summary>
        /// ����Literal�ؼ������ݿ��ֶεİ�
        /// </summary>
        /// <param name="ltl">��Ҫ�󶨵�Literal����</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(Literal ltl, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(ltl, field, isStringType));
        }

        /// <summary>
        /// �����ַ������������ݿ��ֶεİ󶨡�
        /// </summary>
        /// <param name="s">��Ҫ�󶨵��ַ������á�</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(string s, string field, bool isStringType)
        {

            alBinderItems.Add(new BinderItem(s, field, isStringType));
        }

        /// <summary>
        /// ����һ��ί�ж������ݿ��ֶεİ󶨡�
        /// </summary>
        /// <param name="action">ί�е����ơ�</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        private void AddBind(Action action, string field, bool isStringType)
        {

            alBinderItems.Add(new BinderItem(action, field, isStringType));
        }


        /// <summary>
        /// ����һ��string�������Ե����ݿ��ֶεİ󶨡�
        /// </summary>
        /// <param name="_o">�������ڵĶ���</param>
        /// <param name="_propertyName">���Ե����ơ�</param>
        /// <param name="field">���ݿ��ж�Ӧ�ֶε����ơ�</param>
        /// <param name="isStringType">�Ƿ�Ϊ�ַ������ʡ�</param>
        public void AddBind(object _o, string _propertyName, string field, bool isStringType)
        {
            alBinderItems.Add(new BinderItem(new MyProperty(_o, _propertyName), field, isStringType));
        }

        /// <summary>
        /// ����Ƿ������˱���Ĳ��������������ť����
        /// </summary>
        public void CheckArgs()
        {
            if (TableName == string.Empty)
            {
                throw new ArgumentException("None table name is specified!");
            }
            if (btnSubmit == null)
            {
                throw new ArgumentException("None submit button is specified!");
            }
        }
        /// <summary>
        /// ����ť����¼������������ӻ��޸�
        /// </summary>
        /// <param name="sender">��������</param>
        /// <param name="e">���ݵ��¼�������</param>
        private void ProcessTheForm(object sender, System.EventArgs e)
        {
            if (Mode == OperationType.Add)
            {
                this.Add();
            }
            else if (Mode == OperationType.Modify)
            {
                this.Update();
            }
            else
            {
                throw new ArgumentException("Unkown operation type.");
            }
        }



        /// <summary>
        /// ����֤�����ֽ�֧��һ����֤����ί�������Ȳ��ö���һ����
        /// </summary>
        public delegate bool Validator();
        /// <summary>
        /// �洢ί�ж���
        /// </summary>
        public Validator validator = null;

        /// <summary>
        /// ʹ��ί����֤����֤���ݹ����������Ƿ�Ϸ�
        /// </summary>
        /// <returns>�ύ�������Ƿ�����û�������߼���</returns>
        private bool DataValid()
        {
            bool validOk = true;
            if (validator != null)
            {
                validOk = validator();
            }
            return validOk;
        }

        /// <summary>
        /// �����������¼��������ݱ���ӵ����ݿ�֮�󴥷���
        /// </summary>
        public event System.EventHandler AddOk;
        /// <summary>
        ///�����������¼��������ݱ���ӵ����ݿ�֮�󴥷���
        /// </summary>
        /// <param name="e">�����ɵ��¼�������</param>
        protected virtual void OnAddOk(XkCms.DataOper.Data.DbOperEventArgs e)
        {
            if (AddOk != null)
            {
                AddOk(this, e);
            }
        }

        /// <summary>
        /// �޸���������¼��������ݱ����µ����ݿ�֮�󴥷���
        /// </summary>
        public event System.EventHandler ModifyOk;
        /// <summary>
        /// �޸�����¼��������ݱ����µ����ݿ�֮�󴥷���
        /// </summary>
        /// <param name="e">�޸���ɵ��¼�������</param>
        protected virtual void OnModifyOk(System.EventArgs e)
        {
            if (ModifyOk != null)
            {
                ModifyOk(this, e);
            }
        }

        /// <summary>
        /// ���޸�ʱ�����ݿ��е��ֶ�ֵ�ᱻ��䵽��Ӧ�󶨵Ŀؼ��У����¼��������ɺ���ʾ��ҳ��֮ǰ������
        /// </summary>
        public event System.EventHandler BindBeforeModifyOk;
        /// <summary>
        /// ���޸�ʱ�����ݿ��е��ֶ�ֵ�ᱻ��䵽��Ӧ�󶨵Ŀؼ��У����¼��������ɺ���ʾ��ҳ��֮ǰ������
        /// </summary>
        /// <param name="e">�¼�������</param>
        protected virtual void OnBindBeforeModifyOk(System.EventArgs e)
        {
            if (BindBeforeModifyOk != null)
            {
                BindBeforeModifyOk(this, e);
            }
        }

        /// <summary>
        /// ��ҳ��Ԥ����ʱ���ύ�������֮ǰ������Ƿ���Ҫ�����ݿ��е�������䵽���С�
        /// </summary>
        /// <param name="sender">���ݵĶ���</param>
        /// <param name="e">���ݵ��¼�������</param>
        private void Page_PreRender(object sender, EventArgs e)
        {
            if (this.Mode == OperationType.Modify && !btnSubmit.Page.IsPostBack)
            {
                BindWhenModify();
            }
        }
    }
    /// <summary>
    /// �����ί��ԭ�͡�
    /// </summary>
    public delegate string Action(string s);


    #region �Զ�������֧�ֱ��������
    /// <summary>
    /// ÿ�������ݿ��ֶΰ󶨵Ķ�����BinderItemΪ�����洢�������С�
    /// </summary>
    public class BinderItem
    {
        /// <summary>
        /// ÿ���󶨿ؼ�������object����ʽ���洢�ġ�
        /// </summary>
        public object o;
        /// <summary>
        /// �󶨵����ݿ���ֶ����ơ�
        /// </summary>
        public string field;
        /// <summary>
        /// �Ƿ����ַ������͡�
        /// </summary>
        public bool isStringType;
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_o">��Ҫ�󶨵Ŀؼ�����</param>
        /// <param name="_field">�󶨵������ݱ��ֶ����ơ�</param>
        /// <param name="_isStringType">�Ƿ����ַ������͡�</param>
        public BinderItem(object _o, string _field, bool _isStringType)
        {
            this.o = _o;
            this.field = _field;
            this.isStringType = _isStringType;
        }

        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_o">��Ҫ�󶨵�string�������á�</param>
        /// <param name="_field">�󶨵������ݱ��ֶ����ơ�</param>
        /// <param name="_isStringType">�Ƿ����ַ������͡�</param>
        public BinderItem(string _o, string _field, bool _isStringType)
        {
            this.o = _o;
            this.field = _field;
            this.isStringType = _isStringType;
        }
        /// <summary>
        /// ���ݿؼ����ͻ�ÿؼ���ֵ��
        /// </summary>
        /// <returns>�ؼ���ֵ��</returns>
        public string GetValue()
        {
            //��������
            if (o is MyProperty)
            {
                MyProperty mp = (MyProperty)o;
                System.Type type = mp.po.GetType();
                System.Reflection.PropertyInfo pi = type.GetProperty(mp.propertyName);
                return (string)pi.GetValue(mp.po, null);
                //return type.InvokeMember(mp.propertyName,System.Reflection.BindingFlags.GetProperty,null,mp.po,);
                //return mp.propertyName;
            }

            //�ַ�������
            if (o is String)
            {
                string s = (string)o;
                return s;

            }
            //����ί��
            if (o is Action)
            {
                Action action = (Action)o;
                return action("_g_e_t_");
            }
            //������
            if (o is DropDownList)
            {
                DropDownList ddl = (DropDownList)o;
                return ddl.SelectedValue;

            }
            //��ѡ��
            if (o is MyCheckBox)
            {
                MyCheckBox mcb = (MyCheckBox)o;
                if (mcb.cb.Checked) return mcb.Value; else return "0";

            }
            //��ѡ��ť
            if (o is MyRadioButton)
            {
                MyRadioButton mrb = (MyRadioButton)o;
                if (mrb.rb.Checked) return mrb.Value;
            }
            //�ı���
            if (o is TextBox)
            {
                TextBox tb = (TextBox)o;
                return tb.Text;
            }
            //Label
            if (o is Label)
            {
                Label lbl = (Label)o;
                return lbl.Text;
            }
            //Literal
            if (o is Literal)
            {
                Literal ltl = (Literal)o;
                return ltl.Text;
            }
            return string.Empty;
        }

        /// <summary>
        /// ���ݿؼ������趨�ؼ���ֵ
        /// </summary>
        /// <param name="_value">Ҫ�趨��ֵ��</param>
        public void SetValue(string _value)
        {
            //��������
            if (o is MyProperty)
            {
                MyProperty mp = (MyProperty)o;
                System.Type type = mp.po.GetType();
                System.Reflection.PropertyInfo pi = type.GetProperty(mp.propertyName);
                pi.SetValue(mp.po, _value, null);
                //return type.InvokeMember(mp.propertyName,System.Reflection.BindingFlags.GetProperty,null,mp.po,);
                //return mp.propertyName;
            }

            //�ַ�������
            if (o is String)
            {
                //this.SetString((string)o,_value);
                //return;
                string s = (string)o;
                s = _value;
                return;
            }
            //ί��
            if (o is Action)
            {
                Action action = (Action)o;
                action(_value);
                return;
            }
            //������
            if (o is DropDownList)
            {
                DropDownList ddl = (DropDownList)o;
                ListItem li = ddl.Items.FindByValue(_value);
                if (li != null)
                {
                    ddl.ClearSelection();
                    li.Selected = true;
                }
                return;
            }
            //��ѡ��
            if (o is MyCheckBox)
            {
                MyCheckBox mcb = (MyCheckBox)o;
                if (mcb.Value == _value) mcb.cb.Checked = true;
                return;
            }
            //��ѡ��ť
            if (o is MyRadioButton)
            {
                MyRadioButton mrb = (MyRadioButton)o;
                if (mrb.Value == _value) mrb.rb.Checked = true;
                return;
            }
            //�ı���
            if (o is TextBox)
            {
                TextBox tb = (TextBox)o;
                tb.Text = _value;
                return;
            }
            //Label
            if (o is Label)
            {
                Label lbl = (Label)o;
                lbl.Text = _value;
                return;
            }
            //Literal
            if (o is Literal)
            {
                Literal ltl = (Literal)o;
                ltl.Text = _value;
                return;
            }
        }

        private void SetString(string s, string _value)
        {
            s = _value;
        }

    }

    /// <summary>
    /// ��չ��ѡ�򣬿���ʹCheckBox����Value���ԣ�ѡ��ʱ���֡�
    /// </summary>
    public class MyCheckBox
    {
        /// <summary>
        /// CheckBox����
        /// </summary>
        public CheckBox cb;
        /// <summary>
        /// ѡ��ʱӦ�ñ��ֵ�ֵ��
        /// </summary>
        public string Value;
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_cb">CheckBox����</param>
        /// <param name="_value">ѡ��ʱӦ�ñ��ֵ�ֵ��</param>
        public MyCheckBox(CheckBox _cb, string _value)
        {
            cb = _cb;
            Value = _value;
        }
    }

    /// <summary>
    /// ��չ��ѡ��ť������ʹRadioButton����Value���ԣ�ѡ��ʱ���֡�
    /// </summary>
    public class MyRadioButton
    {
        /// <summary>
        /// RadioButton����
        /// </summary>
        public RadioButton rb;
        /// <summary>
        /// ѡ��ʱӦ�ñ��ֵ�ֵ��
        /// </summary>
        public string Value;
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_rb">RadioButton����</param>
        /// <param name="_value">ѡ��ʱӦ�ñ��ֵ�ֵ��</param>
        public MyRadioButton(RadioButton _rb, string _value)
        {
            rb = _rb;
            Value = _value;
        }
    }

    /// <summary>
    /// ��չ���ԣ��洢һ���������ú���Ҫ�󶨵��������ơ�
    /// </summary>
    public class MyProperty
    {
        /// <summary>
        /// Ҫ���������ڵĶ���
        /// </summary>
        public object po;
        /// <summary>
        /// Ҫ�󶨵��������ơ�
        /// </summary>
        public string propertyName;
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="_o">Ҫ���������ڵĶ���</param>
        /// <param name="_propertyName">Ҫ�󶨵��������ơ�</param>
        public MyProperty(object _o, string _propertyName)
        {
            po = _o;
            propertyName = _propertyName;
        }
    }

    #endregion

}
