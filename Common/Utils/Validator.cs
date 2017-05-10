using System;
using System.Text.RegularExpressions;

namespace XkCms.Common.Utils
{
    /// <summary>
    /// �ṩ������Ҫʹ�õ�һЩ��֤�߼���
    /// </summary>
    public class Validator
    {
        /// <summary>
        /// ���һ���ַ����Ƿ����ת��Ϊ���ڣ�һ��������֤�û��������ڵĺϷ��ԡ�
        /// </summary>
        /// <param name="_value">����֤���ַ�����</param>
        /// <returns>�Ƿ����ת��Ϊ���ڵ�boolֵ��</returns>
        public static bool IsStringDate(string _value)
        {
            DateTime dt;
            try
            {
                dt = DateTime.Parse(_value);
            }
            catch (FormatException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// ���ַ���ת������
        /// </summary>
        /// <param name="_value">�ַ���</param>
        /// <param name="_defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static DateTime StrToDate(string _value, DateTime _defValue)
        {
            if (IsStringDate(_value))
                return Convert.ToDateTime(_value);
            else
                return _defValue;
        }

        /// <summary>
        /// ���һ���ַ����Ƿ��Ǵ����ֹ��ɵģ�һ�����ڲ�ѯ�ַ�����������Ч����֤��
        /// </summary>
        /// <param name="_value">����֤���ַ�������</param>
        /// <returns>�Ƿ�Ϸ���boolֵ��</returns>
        public static bool IsNumberId(string _value)
        {
            return Validator.QuickValidate("^[1-9]*[0-9]*$", _value);
        }

        /// <summary>
        /// ���һ���ַ����Ƿ��Ǵ���ĸ�����ֹ��ɵģ�һ�����ڲ�ѯ�ַ�����������Ч����֤��
        /// </summary>
        /// <param name="_value">����֤���ַ�����</param>
        /// <returns>�Ƿ�Ϸ���boolֵ��</returns>
        public static bool IsLetterOrNumber(string _value)
        {
            return Validator.QuickValidate("^[a-zA-Z0-9_]*$", _value);
        }

        /// <summary>
        /// �ж��Ƿ������֣�����С����������
        /// </summary>
        /// <param name="_value">����֤���ַ�����</param>
        /// <returns>�Ƿ�Ϸ���boolֵ��</returns>
        public static bool IsNumber(string _value)
        {
            return Validator.QuickValidate("^(0|([1-9]+[0-9]*))(.[0-9]+)?$", _value);
        }

        /// <summary>
        /// ���ַ���ת������
        /// </summary>
        /// <param name="_value">�ַ���</param>
        /// <param name="_defValue">Ĭ��ֵ</param>
        /// <returns></returns>
        public static int StrToInt(string _value, int _defValue)
        {
            if (IsNumber(_value))
                return int.Parse(_value);
            else
                return _defValue;
        }

        /// <summary>
        /// ������֤һ���ַ����Ƿ����ָ����������ʽ��
        /// </summary>
        /// <param name="_express">������ʽ�����ݡ�</param>
        /// <param name="_value">����֤���ַ�����</param>
        /// <returns>�Ƿ�Ϸ���boolֵ��</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            System.Text.RegularExpressions.Regex myRegex = new System.Text.RegularExpressions.Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
    }
}
