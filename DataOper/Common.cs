using System;
using System.Text.RegularExpressions;

namespace XkCms.DataOper
{
    /// <summary>
    /// 枚举，作为Web中常用的用户操作类型。常用于权限相关的判断。
    /// </summary>
    public enum OperationType : byte { Add, Modify, Delete, Audit, Enable };
}