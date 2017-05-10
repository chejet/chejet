<%@ Page Language="C#" %>

<%@ Import Namespace="System.Net" %>
<%@ Implements Interface="System.Web.UI.ICallbackEventHandler" %>

<script runat="server">
    /// <summary>
    /// 保存目录
    /// </summary>
    private string savePath = null;
    private const string DEFAULT_USER_FILES_PATH = "/UserFiles/";

    /// <summary>
    /// 此处配置允许下载的文件扩展名
    /// <remarks>
    ///     暂未考虑使用动态网页输出的图片如：http://site/image.aspx?uid=00001 这样的URI;
    ///  若要实现此功能可读取流并判断ContentType,将流另存为相应文件格式即可。
    /// </remarks>
    /// </summary>
    private static readonly string[] allowImageExtension = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

    /// <summary>
    /// 此处配置本地（网站）主机名
    /// </summary>
    private string[] localhost;

    private string localImageSrc = string.Empty;

    private void Page_Load(object obj, EventArgs args)
    {
        #region 得到FckEditor配置的上传路径
        savePath = (string)Session["FCKeditor:UserFilesPath"];
        // Check that the user path ends with slash ("/")
        if (!savePath.EndsWith("/"))
            savePath += "/";
        #endregion

        localhost = new string[] { "localhost", "127.0.0.1", Request.Url.Host };

        if (!Page.IsPostBack)
        {
            Response.Write("<script>var localHost=\"http://" + Request.Url.Host + "\";</" + "script>");
            
            ClientScriptManager csm = Page.ClientScript;

            string scripCallServerDownLoad = csm.GetCallbackEventReference(this, "args", "_ReceiveServerData", "context");
            string callbackScriptDwonLoad = "function _CallServerDownLoad(args , context) {" + scripCallServerDownLoad + "; }";
            if (!csm.IsClientScriptBlockRegistered("_CallServerDownLoad"))
            {
                csm.RegisterClientScriptBlock(this.GetType(), "_CallServerDownLoad", callbackScriptDwonLoad, true);
            }
        }
    }

    /// <summary>
    /// 返回数据
    /// </summary>
    /// <remarks>如果处理过程中出现错误，则仍然返回远程路径</remarks>
    /// <returns>服务器端处理后的本地图片路径</returns>
    public string GetCallbackResult()
    {
        return localImageSrc;

    }

    /// <summary>
    /// 处理回调事件 
    /// </summary>
    /// <param name="eventArgument">一个字符串，表示要传递到事件处理程序的事件参数</param>
    public void RaiseCallbackEvent(string eventArgument)
    {

        string remoteImageSrc = eventArgument;

        string fileName = remoteImageSrc.Substring(remoteImageSrc.LastIndexOf("/") + 1);
        string ext = System.IO.Path.GetExtension(fileName);

        if (!IsAllowedDownloadFile(ext))
        {
            //非指定类型图片不进行下载，直接返回原地址。
            localImageSrc = remoteImageSrc;
            return;
        }

        Uri uri = new Uri(remoteImageSrc);
        if (IsLocalSource(uri))
        {
            //本地（本网站下）图片不进行下载，直接返回原地址。
            localImageSrc = remoteImageSrc;
            return;
        }

        try
        {
            string datePath = XkCms.Common.Utils.DirFile.GetDateDir();

            string localDirectory = System.IO.Path.Combine(Server.MapPath(savePath), datePath);
            if (!System.IO.Directory.Exists(localDirectory))
            {
                System.IO.Directory.CreateDirectory(localDirectory);
            }

            string localFilePath = System.IO.Path.Combine(localDirectory, fileName);

            //不存在同名文件则开始下载，若已经存在则不下载该文件，直接返回已有文件路径。
            if (!System.IO.File.Exists(localFilePath))
            {
                Client.DownloadFile(uri, localFilePath);
            }

            localImageSrc = savePath + datePath + "/" + fileName;
        }
        catch
        {
            //下载过程中出现任何异常都不抛出(  有点狠啊 :)  )，仍然用远程图片链接。
            localImageSrc = remoteImageSrc;
        }
    }

    private WebClient client;

    /// <summary>
    /// <see cref="System.Net.WebClient"/>
    /// </summary>
    public WebClient Client
    {
        get
        {
            if (client != null)
            {
                return client;
            }

            client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2;)");

            return client;

        }
    }

    /// <summary>
    /// 判断Uri是否为本地路径
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    private bool IsLocalSource(Uri uri)
    {
        for (int i = localhost.Length; --i >= 0; )
        {
            if (localhost[i].ToLower() == uri.Host.ToLower())
            {
                return true;
            }
        }

        return false;

    }

    /// <summary>
    /// 检测文件类型是否为允许下载的文件类型
    /// </summary>
    /// <param name="extension">扩展名 eg: ".jpg"</param>
    /// <returns></returns>
    private bool IsAllowedDownloadFile(string extension)
    {
        for (int i = allowImageExtension.Length; --i >= 0; )
        {
            if (allowImageExtension[i].ToLower() == extension.ToLower())
            {
                return true;
            }
        }
        return false;
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
     body { margin: 0px; overflow: hidden;  background-color: buttonface;  }
     td { font-size: 11pt; font-family: Arial;text-align: left;}
     #domProgressBarId{
         width: 0%;
         height: 15px;  
         border-right: buttonhighlight 1px solid;
         border-top: buttonshadow 1px solid; 
         border-left: buttonshadow 1px solid; 
         border-bottom: buttonhighlight 1px solid;
         background-color: highlight;
     }
 </style>

    <script type="text/javascript" language="javascript"> 
         
        var RemoteImageRubber = function ( remoteSrcList )
        {
             this._remoteSrcList = remoteSrcList;
             this._totalFilesCount = remoteSrcList.length; 
         }
 
         RemoteImageRubber.prototype.CurrentPercent = function()
         {
             return Math.round( 100 * (1-  this.CurrentFilesCount() / this.TotalFilesCount() ) )+"%";
         }
         
         RemoteImageRubber.prototype.TotalFilesCount = function()
         {
             return this._totalFilesCount;
         }
         
         RemoteImageRubber.prototype.CurrentFilesCount = function()
         {
             return this._remoteSrcList.length;
         }
 
         RemoteImageRubber.prototype.NextFile = function ()
         {
             if(this._remoteSrcList.length >0)
             {
                 var currentRemoteSrc = this._remoteSrcList.shift();
                 _PreCallServer(currentRemoteSrc);
             }
         }
         
    </script>

    <script type="text/javascript" language="javascript">
         
         var oEditor;
         var domProgressBar;
         var domCurrentFile;
         var domAllFilesCount;
         var domAlreadyDownloadFilesCount;
         
         var imageUrls = new Array();
         var remoteList = new Array();
         var localList = new Array(); 
         
         var progressBar;
         
         function Ok()
         {             
             var _imgIndex;
             for(_imgIndex = 0; _imgIndex < imageUrls.length; _imgIndex ++)
             {
                 imageUrls[_imgIndex].src = localList[_imgIndex];
                 imageUrls[_imgIndex].setAttribute("_fcksavedurl", localList[_imgIndex], 0) ;
             }
             return true ;
         }
         
    </script>

    <script language="javascript" type="text/javascript">
     
        function _PreCallServer(currentRemoteSrc)
        {
            var start = currentRemoteSrc.lastIndexOf("/") + 1;
             var end = currentRemoteSrc.length - currentRemoteSrc.lastIndexOf("/");
 
             var currentFileName = currentRemoteSrc.substr(start,end);
                         
             domCurrentFile.innerHTML = currentFileName;         
             _CallServerDownLoad(currentRemoteSrc,'');
         }
         
         function _ReceiveServerData(receiveValue ,context)
         {
             if(receiveValue)
             {
                 var localSrc = receiveValue;
                 localList.push(localSrc);
                 
                 domAlreadyDownloadFilesCount.innerHTML = progressBar.TotalFilesCount() - progressBar.CurrentFilesCount();
                 domProgressBar.style.width = progressBar.CurrentPercent();
                 
                 if( progressBar.CurrentFilesCount() > 0 )
                 {
                    window.setTimeout("progressBar.NextFile()",0);        
                 }
             }            
             
             if(progressBar.CurrentFilesCount() == 0)
             {                
                 window.setTimeout("_reFlush()",500)
             } 
         }
 
         function _StartDownLoad()    
         {   
             var imageUrlss = oEditor.EditorDocument.body.getElementsByTagName("img");
             
             var m;
             var w = 0; 
             for(m = 0; m < imageUrlss.length; m ++)
             {
                if(imageUrlss[m].src.indexOf(localHost) != 0)
                {
                    remoteList[w] = imageUrlss[m].src;
                    imageUrls[w] = imageUrlss[m];
                    w++;
                }
             }
            
             progressBar = new RemoteImageRubber(remoteList);              
             domAllFilesCount.innerHTML = progressBar.TotalFilesCount();
             domAlreadyDownloadFilesCount.innerHTML = progressBar.TotalFilesCount() - progressBar.CurrentFilesCount();
             window.setTimeout("progressBar.NextFile()",0);
             
         }    
 
         function _reFlush()
         {           
             
             domProgressBar.style.width  = "100%";
             
             //display the 'OK' button            
             window.parent.SetOkButton( true ) ;
         }
         
 
    </script>

</head>
<body>
    <form id="aspnetForm" runat="server">
        <div style="width: 300px; padding-left: 10px;">
            <table border="0" cellspacing="0" cellpadding="2">
                <tr>
                    <td nowrap="nowrap" style="width: 290px;">
                        当前文件:&nbsp;<span id="domCurrentFile" style="display: inline; text-overflow: ellipsis"></span></td>
                </tr>
                <tr>
                    <td style="text-align: left; width: 290px;">
                        <div id="domProgressBarId">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap" style="width: 290px;">
                        共有:&nbsp;<span id="domAllFilesCount"></span>&nbsp;&nbsp;个远程图片</td>
                </tr>
                <tr>
                    <td nowrap="nowrap" style="width: 290px;">
                        已下载:&nbsp;<span id="domAlreadyDownloadFilesCount"></span>&nbsp;&nbsp;个。</td>
                </tr>
            </table>
        </div>
    </form>

    <script type="text/javascript" language="javascript">
     window.parent.SetOkButton( false ) ; 
     
     oEditor = window.parent.InnerDialogLoaded().FCK;       
     
     domProgressBar = document.getElementById("domProgressBarId");
     domCurrentFile = document.getElementById("domCurrentFile");
     domAllFilesCount = document.getElementById("domAllFilesCount");
     domAlreadyDownloadFilesCount = document.getElementById("domAlreadyDownloadFilesCount");   
 
     window.setTimeout("_StartDownLoad()",0);  
    </script>

</body>
</html>
