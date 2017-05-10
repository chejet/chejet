using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Collections;
using System.Drawing;

namespace XkCms.WebForm.BaseFunction
{
    /// <summary>
    /// NewsCollection 的摘要说明
    /// </summary>
    public class NewsCollection : BasicPage
    {
        public NewsCollection()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public string GetHttpPage(string url,int timeout,Encoding EnCodeType)
        {
            string strResult = string.Empty;
            if (url.Length < 10)
                return "$UrlIsFalse$";
            try {
                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                MyWebClient.Encoding = EnCodeType;
                strResult = MyWebClient.DownloadString(url);
            }
            catch(Exception)
            {
                strResult = "$GetFalse$";
            }
            return strResult;
        }

        public string GetBody(string pageStr, string strStart, string strEnd, bool inStart, bool inEnd)
        {
            pageStr = pageStr.Trim();
            int start = pageStr.IndexOf(strStart);
            if (strStart.Length==0 || start < 0)
                return "$StartFalse$";
            pageStr = pageStr.Substring(start + strStart.Length, pageStr.Length - start - strStart.Length);
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0)
                return "$EndFalse$";
            string strResult = pageStr.Substring(0, end);
            if (inStart)
                strResult = strStart + strResult;
            if (inEnd)
                strResult += strEnd;
            return strResult.Trim();
        }

        public ArrayList GetArray(string pageStr, string strStart, string strEnd)
        {
            ArrayList linkArray = new ArrayList();
            int start = pageStr.IndexOf(strStart);
            if (strStart.Length == 0 || start < 0)
            {
                linkArray.Add("$StartFalse$");
                return linkArray;
            }
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0)
            {
                linkArray.Add("$EndFalse$");
                return linkArray;
            }
            Regex myRegex = new Regex(@"(" + strStart + ").+?(" + strEnd + ")", RegexOptions.IgnoreCase);
            MatchCollection matches = myRegex.Matches(pageStr);
            foreach (Match match in matches)
                linkArray.Add(match.ToString());
            if (linkArray.Count == 0)
            {
                linkArray.Add("$NoneLink$");
                return linkArray;
            }
            string TempStr = string.Empty;
            for (int i = 0; i < linkArray.Count; i++)
            {
                TempStr = linkArray[i].ToString();
                TempStr = TempStr.Replace(strStart, "");
                TempStr = TempStr.Replace(strEnd, "");
                linkArray[i] = (object)TempStr;
            }
            return linkArray;
        }

        public ArrayList ReplaceSaveRemoteFile(string pageStr, string SavePath, string CDir, string webUrl, string isSave)
        {
            ArrayList replaceArray = new ArrayList();
            Regex imgReg = new Regex(@"<img.+?[^\>]>", RegexOptions.IgnoreCase);
            MatchCollection matches = imgReg.Matches(pageStr);
            string TempStr = string.Empty;
            string TitleImg = string.Empty;
            foreach (Match match in matches)
            {
                if (TempStr != string.Empty)
                    TempStr += "$Array$" + match.ToString();
                else
                    TempStr = match.ToString();
            }
            string[] TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            imgReg = new Regex(@"src\s*=\s*.+?\.(gif|jpg|bmp|jpeg|psd|png|svg|dxf|wmf|tiff)", RegexOptions.IgnoreCase);
            for (int i = 0; i < TempArr.Length; i++)
            {
                matches = imgReg.Matches(TempArr[i]);
                foreach (Match match in matches)
                {
                    if (TempStr != string.Empty)
                        TempStr += "$Array$" + match.ToString();
                    else
                        TempStr = match.ToString();
                }
            }
            if (TempStr.Length > 0)
            {
                imgReg = new Regex(@"src\s*=\s*", RegexOptions.IgnoreCase);
                TempStr = imgReg.Replace(TempStr, "");
            }
            if (TempStr.Length == 0)
            {
                replaceArray.Add(pageStr);
                return replaceArray;
            }
            TempStr = TempStr.Replace("\"", "");
            TempStr = TempStr.Replace("'", "");
            TempStr = TempStr.Replace(" ", "");

            SavePath = SavePath + "/UserFiles/" + XkCms.Common.Utils.DirFile.GetDateDir();
            if (!System.IO.Directory.Exists(SavePath))
                System.IO.Directory.CreateDirectory(SavePath);

            //去掉重复图片
            TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            for (int i = 0; i < TempArr.Length; i++)
            {
                if (TempStr.IndexOf(TempArr[i]) == -1)
                    TempStr += "$Array$" + TempArr[i];
            }
            TempStr = TempStr.Substring(7);

            TempArr = TempStr.Split(new string[] { "$Array$" }, StringSplitOptions.None);
            TempStr = string.Empty;
            string ImageArr = string.Empty;
            for (int i = 0; i < TempArr.Length; i++)
            {
                imgReg = new Regex(TempArr[i]);
                string RemoteFileUrl = DefiniteUrl(TempArr[i], webUrl);
                if (isSave == "1")
                {
                    string fileType = RemoteFileUrl.Substring(RemoteFileUrl.LastIndexOf('.'));
                    string filename = string.Empty;
                    filename = XkCms.Common.Utils.DirFile.GetDateFile();
                    filename += fileType;
                    if (SaveRemotePhoto(SavePath + "/" + filename, RemoteFileUrl))
                        RemoteFileUrl = CDir + "/UserFiles/" + XkCms.Common.Utils.DirFile.GetDateDir() + "/" + filename;
                }
                pageStr = imgReg.Replace(pageStr, RemoteFileUrl);
                if (i == 0)
                {
                    TitleImg = RemoteFileUrl;
                    ImageArr = RemoteFileUrl;
                }
                else
                    ImageArr += "|||" + RemoteFileUrl;
            }
            replaceArray.Add(pageStr);
            replaceArray.Add(TitleImg);
            replaceArray.Add(ImageArr);
            return replaceArray;
        }

        public string DefiniteUrl(string PrimitiveUrl, string ConsultUrl)
        {
            if (ConsultUrl.Substring(0, 7) != "http://")
                ConsultUrl = "http://" + ConsultUrl;
            ConsultUrl = ConsultUrl.Replace(@"\", "/");
            ConsultUrl = ConsultUrl.Replace("://", @":\\");
            PrimitiveUrl = PrimitiveUrl.Replace(@"\", "/");

            if (ConsultUrl.Substring(ConsultUrl.Length - 1) != "/")
            {
                if (ConsultUrl.IndexOf('/') > 0)
                {
                    if (ConsultUrl.Substring(ConsultUrl.LastIndexOf("/"), ConsultUrl.Length - ConsultUrl.LastIndexOf("/")).IndexOf('.') == -1)
                        ConsultUrl += "/";
                }
                else
                    ConsultUrl += "/";
            }
            string[] ConArray = ConsultUrl.Split('/');
            string returnStr = string.Empty;
            string[] PriArray;
            int pi = 0;
            if (PrimitiveUrl.Substring(0, 7) == "http://")
                returnStr = PrimitiveUrl.Replace("://", @":\\");
            else if (PrimitiveUrl.Substring(0, 1) == "/")
                returnStr = ConArray[0] + PrimitiveUrl;
            else if (PrimitiveUrl.Substring(0, 2) == "./")
            {
                PrimitiveUrl = PrimitiveUrl.Substring(PrimitiveUrl.Length - 2, 2);
                if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                    returnStr = ConsultUrl + PrimitiveUrl;
                else
                    returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + PrimitiveUrl;
            }
            else if (PrimitiveUrl.Substring(0, 3) == "../")
            {
                while (PrimitiveUrl.Substring(0, 3) == "../")
                {
                    PrimitiveUrl = PrimitiveUrl.Substring(3);
                    pi++;
                }
                for (int i = 0; i < ConArray.Length - 1 - pi; i++)
                {
                    if (returnStr.Length > 0)
                        returnStr = returnStr + ConArray[i];
                    else
                        returnStr = ConArray[i];
                }
                returnStr = returnStr + PrimitiveUrl;
            }
            else
            {
                if (PrimitiveUrl.IndexOf('/') > -1)
                {
                    PriArray = PrimitiveUrl.Split('/');
                    if (PriArray[0].IndexOf('.') > -1)
                    {
                        if (PrimitiveUrl.Substring(PrimitiveUrl.Length - 1) == "/")
                            returnStr = "http://" + PrimitiveUrl;
                        {
                            if (PriArray[PriArray.Length - 1].IndexOf('.') > -1)
                                returnStr = "http:\\" + PrimitiveUrl;
                            else
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                        }
                    }
                    else
                    {
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                            returnStr = ConsultUrl + PrimitiveUrl;
                        else
                            returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + PrimitiveUrl;
                    }
                }
                else
                {
                    if (PrimitiveUrl.IndexOf('.') > -1)
                    {
                        string lastUrl = ConsultUrl.Substring(ConsultUrl.LastIndexOf('.'));
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                        {
                            if (lastUrl == "com" || lastUrl == "cn" || lastUrl == "net" || lastUrl == "org")
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                            else
                                returnStr = ConsultUrl + PrimitiveUrl;
                        }
                        else
                        {
                            if (lastUrl == "com" || lastUrl == "cn" || lastUrl == "net" || lastUrl == "org")
                                returnStr = "http:\\" + PrimitiveUrl + "/";
                            else
                                returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + "/" + PrimitiveUrl;
                        }
                    }
                    else
                    {
                        if (ConsultUrl.Substring(ConsultUrl.Length - 1) == "/")
                            returnStr = ConsultUrl + PrimitiveUrl + "/";
                        else
                            returnStr = ConsultUrl.Substring(0, ConsultUrl.LastIndexOf('/')) + "/" + PrimitiveUrl + "/";
                    }
                }
            }

            if (returnStr.Substring(0, 1) == "/")
                returnStr = returnStr.Substring(1);
            if (returnStr.Length > 0)
            {
                returnStr = returnStr.Replace("//", "/");
                returnStr = returnStr.Replace(@":\\", "://");
            }
            else
                returnStr = "$False$";
            return returnStr;
        }

        public bool SaveRemotePhoto(string fileName, string RemoteFileUrl)
        {
            try
            {
                WebRequest request = WebRequest.Create(RemoteFileUrl);
                request.Timeout = 20000;
                Stream stream = request.GetResponse().GetResponseStream();
                Image getImage = Image.FromStream(stream);
                getImage.Save(fileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string ScriptHtml(string ConStr, string TagName, int FType)
        {
            Regex myReg;
            switch (FType)
            {
                case 1:
                    myReg = new Regex("<" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
                case 2:
                    myReg = new Regex("<" + TagName + "([^>])*>.*?</" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
                case 3:
                    myReg = new Regex("<" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    myReg = new Regex("</" + TagName + "([^>])*>", RegexOptions.IgnoreCase);
                    ConStr = myReg.Replace(ConStr, "");
                    break;
            }
            return ConStr;
        }

        public string NoHtml(string ConStr)
        {
            Regex myReg = new Regex(@"(\<.[^\<]*\>)", RegexOptions.IgnoreCase);
            ConStr = myReg.Replace(ConStr, "");
            myReg = new Regex(@"(\<\/[^\<]*\>)", RegexOptions.IgnoreCase);
            ConStr = myReg.Replace(ConStr, "");
            return ConStr;
        }

        public string GetPaing(string pageStr, string strStart, string strEnd)
        {
            int end = pageStr.IndexOf(strEnd);
            if (strEnd.Length == 0 || end < 0)
                return "$EndFalse$";
            pageStr = pageStr.Substring(0, end);
            int start = pageStr.LastIndexOf(strStart);
            if (strStart.Length == 0 || start < 0 || start > end)
                return "$StartFalse$";
            pageStr = pageStr.Substring(start + strStart.Length);
            pageStr = pageStr.Replace("\"", "");
            pageStr = pageStr.Replace("'", "");
            pageStr = pageStr.Replace(" ", "");
            pageStr = pageStr.Trim();
            return pageStr;
        }
    }
}