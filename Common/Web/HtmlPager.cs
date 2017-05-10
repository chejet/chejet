using System;
using System.Collections.Generic;
using System.Text;

namespace XkCms.Common.Web
{
    public class HtmlPager
    {
        /// <summary>
        /// д����ҳ
        /// </summary>
        /// <param name="pageCount">ҳ��</param>
        /// <param name="currentPage">��ǰҳ</param>
        public static string GetPager(int pageCount, int currentPage)
        {
            return GetPager(pageCount, currentPage, new string[] { }, new string[] { });
        }

        /// <summary>
        /// д����ҳ
        /// </summary>
        /// <param name="pageCount">ҳ��</param>
        /// <param name="currentPage">��ǰҳ</param>
        /// <param name="FieldName">��ַ������</param>
        /// <param name="FieldValue">��ַ������ֵ</param>
        /// <returns></returns>
        public static string GetPager(int pageCount, int currentPage, string[] FieldName, string[] FieldValue)
        {
            string pString = "";
            for (int i = 0; i < FieldName.Length; i++)
            {
                pString += "&" + FieldName[i].ToString() + "=" + FieldValue[i].ToString();
            }
            int stepNum = 4;
            int pageRoot = 1;
            pageCount = pageCount == 0 ? 1 : pageCount;
            currentPage = currentPage == 0 ? 1 : currentPage;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellpadding=0 cellspacing=1 class=\"pager\">\r<tr>\r");
            sb.Append("<td class=pagerTitle>&nbsp;��ҳ&nbsp;</td>\r");
            sb.Append("<td class=pagerTitle>&nbsp;" + currentPage.ToString() + "/" + pageCount.ToString() + "&nbsp;</td>\r");
            if (currentPage - stepNum < 2)
                pageRoot = 1;
            else
                pageRoot = currentPage - stepNum;
            int pageFoot = pageCount;
            if (currentPage + stepNum >= pageCount)
                pageFoot = pageCount;
            else
                pageFoot = currentPage + stepNum;
            if (pageRoot == 1)
            {
                if (currentPage > 1)
                {
                    sb.Append("<td>&nbsp;<a href='?page=1" + pString + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                    sb.Append("<td>&nbsp;<a href='?page=" + Convert.ToString(currentPage - 1) + pString + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                }
            }
            else
            {
                sb.Append("<td>&nbsp;<a href='?page=1" + pString + "' title='��ҳ'>��ҳ</a>&nbsp;</td>");
                sb.Append("<td>&nbsp;<a href='?page=" + Convert.ToString(currentPage - 1) + pString + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
            }
            for (int i = pageRoot; i <= pageFoot; i++)
            {
                if (i == currentPage)
                {
                    sb.Append("<td class='current'>&nbsp;" + i.ToString() + "&nbsp;</td>\r");
                }
                else
                {
                    sb.Append("<td>&nbsp;<a href='?page=" + i.ToString() + pString + "' title='��" + i.ToString() + "ҳ'>" + i.ToString() + "</a>&nbsp;</td>\r");
                }
                if (i == pageCount)
                    break;
            }
            if (pageFoot == pageCount)
            {
                if (pageCount > currentPage)
                {
                    sb.Append("<td>&nbsp;<a href='?page=" + Convert.ToString(currentPage + 1) + pString + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                    sb.Append("<td>&nbsp;<a href='?page=" + pageCount.ToString() + pString + "' title='βҳ'>βҳ</a>&nbsp;</td>\r");
                }
            }
            else
            {
                sb.Append("<td>&nbsp;<a href='?page=" + Convert.ToString(currentPage + 1) + pString + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                sb.Append("<td>&nbsp;<a href='?page=" + pageCount.ToString() + pString + "' title='βҳ'>βҳ</a>&nbsp;</td>\r");
            }
            sb.Append("</tr>\r</table>");
            return sb.ToString();
        }

        /// <summary>
        /// д����ҳ
        /// </summary>
        /// <param name="pageCount">ҳ��</param>
        /// <param name="currentPage">��ǰҳ</param>
        /// <returns></returns>
        public static string GetHtmlPager(int pageCount, int currentPage, string prefix, string suffix)
        {
            int stepNum = 4;
            int pageRoot = 1;
            pageCount = pageCount == 0 ? 1 : pageCount;
            currentPage = currentPage == 0 ? 1 : currentPage;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellpadding=0 cellspacing=1 class=\"pager\">\r<tr>\r");
            sb.Append("<td class=pagerTitle>&nbsp;��ҳ&nbsp;</td>\r");
            sb.Append("<td class=pagerTitle>&nbsp;" + currentPage.ToString() + "/" + pageCount.ToString() + "&nbsp;</td>\r");
            if (currentPage - stepNum < 2)
                pageRoot = 1;
            else
                pageRoot = currentPage - stepNum;
            int pageFoot = pageCount;
            if (currentPage + stepNum >= pageCount)
                pageFoot = pageCount;
            else
                pageFoot = currentPage + stepNum;
            if (pageRoot == 1)
            {
                if (currentPage > 1)
                {
                    sb.Append("<td>&nbsp;<a href='" + prefix + "1" + suffix + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                    sb.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString(currentPage - 1) +suffix + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                }
            }
            else
            {
                sb.Append("<td>&nbsp;<a href='" + prefix + "1" + suffix + "' title='��ҳ'>��ҳ</a>&nbsp;</td>");
                sb.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString(currentPage - 1) + suffix + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
            }
            for (int i = pageRoot; i <= pageFoot; i++)
            {
                if (i == currentPage)
                {
                    sb.Append("<td class='current'>&nbsp;" + i.ToString() + "&nbsp;</td>\r");
                }
                else
                {
                    sb.Append("<td>&nbsp;<a href='" + prefix + i.ToString() + suffix + "' title='��" + i.ToString() + "ҳ'>" + i.ToString() + "</a>&nbsp;</td>\r");
                }
                if (i == pageCount)
                    break;
            }
            if (pageFoot == pageCount)
            {
                if (pageCount > currentPage)
                {
                    sb.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString(currentPage + 1) + suffix + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                    sb.Append("<td>&nbsp;<a href='" + prefix + pageCount.ToString() + suffix + "' title='βҳ'>βҳ</a>&nbsp;</td>\r");
                }
            }
            else
            {
                sb.Append("<td>&nbsp;<a href='" + prefix + Convert.ToString(currentPage + 1) + suffix + "' title='��ҳ'>��ҳ</a>&nbsp;</td>\r");
                sb.Append("<td>&nbsp;<a href='" + prefix + pageCount.ToString() + suffix + "' title='βҳ'>βҳ</a>&nbsp;</td>\r");
            }
            sb.Append("</tr>\r</table>");
            return sb.ToString();
        }
    }
}
