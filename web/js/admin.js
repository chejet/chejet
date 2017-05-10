//菜单
function showsubmenu(sid,num)
{
    whichEl = eval("submenu" + sid);
    if (whichEl.style.display == "none")
    {
        for(var i=0;i<num;i++){
            if(eval("submenu" + i).style.display=="")
            {
                eval("submenu" + i + ".style.display=\"none\";");
                eval("menu" + i + ".style.background=\"url(images/title_bg_show.gif)\";");
            }
        }
        eval("submenu" + sid + ".style.display=\"\";");
        eval("menu" + sid + ".style.background=\"url(images/title_bg_hide.gif)\";");
    }
    else
    {
        eval("submenu" + sid + ".style.display=\"none\";");
        eval("menu" + sid + ".style.background=\"url(images/title_bg_show.gif)\";");
    }
}
//标题图片
function changeImg()
{ 
    var imgDrop = $('upimgSelect').options[$('upimgSelect').selectedIndex].value;
    $('txtImg').value=imgDrop;
}
//全选
function CheckAll(form)
{
    for (var i=0;i<form.elements.length;i++)
    {
        var e = form.elements[i];
        if (e.name != 'chkall' && e.type=="checkbox")
            e.checked = $("chkall").checked;
    }
}
//软件平台选择
function ToSystem(addTitle){
    var str=$("txtOperatingSystem").value;
    if (str=="") {
        $("txtOperatingSystem").value=$("txtOperatingSystem").value+addTitle;
    }else{
        if (str.substr(str.length-1,1)=="/"){
            $("txtOperatingSystem").value=$("txtOperatingSystem").value+addTitle;
        }else{
            $("txtOperatingSystem").value=$("txtOperatingSystem").value+"/"+addTitle;
        }
    }
    $("txtOperatingSystem").focus();
}
//以下是软件相关
function AddUrl(){
  var thisurl='下载地址'+($("lbDownUrl").length+1)+'|http://'; 
  var url=prompt('请输入下载地址名称和链接，中间用“|”隔开：',thisurl);
  if(url!=null&&url!=''){$("lbDownUrl").options[$("lbDownUrl").length]=new Option(url,url);}
  UrlListToHidden();
}
function ModifyUrl(){
  if($("lbDownUrl").length==0) return false;
  var thisurl=$("lbDownUrl").value; 
  if (thisurl=='') {alert('请先选择一个下载地址，再点修改按钮！');return false;}
  var url=prompt('请输入下载地址名称和链接，中间用“|”隔开：',thisurl);
  if(url!=thisurl&&url!=null&&url!=''){$("lbDownUrl").options[$("lbDownUrl").selectedIndex]=new Option(url,url);}
  UrlListToHidden();
}
function DelUrl(){
  if($("lbDownUrl").length==0) return false;
  var thisurl=$("lbDownUrl").value; 
  if (thisurl=='') {alert('请先选择一个下载地址，再点删除按钮！');return false;}
  $("lbDownUrl").options[$("lbDownUrl").selectedIndex]=null;
  UrlListToHidden();
}
//以下是图片相关
function AddPhoto(){
  var thisurl='http://'; 
  var url=prompt('请输入图片地址：',thisurl);
  if(url!=null&&url!=''){$("lbDownUrl").options[$("lbDownUrl").length]=new Option(url,url);}
  UrlListToHidden();
}
function ModifyPhoto(){
  if($("lbDownUrl").length==0) return false;
  var thisurl=$("lbDownUrl").value; 
  if (thisurl=='') {alert('请先选择一个图片地址，再点修改按钮！');return false;}
  var url=prompt('请输入图片地址',thisurl);
  if(url!=thisurl&&url!=null&&url!=''){$("lbDownUrl").options[$("lbDownUrl").selectedIndex]=new Option(url,url);}
  UrlListToHidden();
}
function DelPhoto(){
  if($("lbDownUrl").length==0) return false;
  var thisurl=$("lbDownUrl").value; 
  if (thisurl=='') {alert('请先选择一个图片地址，再点删除按钮！');return false;}
  $("lbDownUrl").options[$("lbDownUrl").selectedIndex]=null;
  UrlListToHidden();
}
//软件图片地址相关
function UrlListToHidden()
{
    var urls = "";
    for(var i=0;i< $("lbDownUrl").length; i++){
        if (urls == "")
            urls = $("lbDownUrl").options[i].value;
        else
            urls += "|||" + $("lbDownUrl").options[i].value;
    }
    $("hfDownUrl").value = urls;
}
function UrlHiddenToList()
{
    if($("hfDownUrl").value == "") return;
    var urls = new Array();
    urls = $("hfDownUrl").value.split("|||");
    var length = $("lbDownUrl").length;
    for(var i = length - 1; i > -1; i--){
        $("lbDownUrl").remove(i);
    }
    length = $('upimgSelect').length;
    for(var i = length - 1; i > 0; i--){
        $('upimgSelect').remove(i);
    }
    for(var i = 0; i< urls.length; i++){
        $("lbDownUrl").options[i] = new Option(urls[i], urls[i]);
        var ext = urls[i].toString().substr(urls[i].toString().lastIndexOf('.')+1);
        var exts = "jpgjepgjpggifbmppng";
        if(exts.indexOf(ext) > -1)
            $('upimgSelect').options[i+1] = new Option("Image" + ( i + 1 ), urls[i]);
    }
}
//显示批量上传
function ShowUpload(t)
{
    ShowDialog('MutilFilesUpload.aspx?ChannelId=-1&t='+t,340,180,0);
    UrlHiddenToList();
}
//显示单个上传
function ShowOneUpload(t)
{
    var returnValue = ShowDialog('Uploader.aspx?ChannelId=-1&t='+t,340,180,0);
    if(returnValue)
    {
        var oldvalue=$("hfDownUrl").value;
        if(oldvalue=="")
            $("hfDownUrl").value=returnValue;
        else
            $("hfDownUrl").value=oldvalue+"|||"+returnValue;
        UrlHiddenToList();
    }
}

//显示网页对话框
function ShowDialog(url,width,height,canScroll,dialogArguments)
{
    var sFeatures="";
    
    if(width){
        sFeatures+="dialogWidth:"+width+"px; ";
    }else{
        sFeatures+="dialogWidth:"+400+"px; ";
    }
    
    if(height){
        sFeatures+="dialogHeight:"+height+"px; ";
    }else{
        sFeatures+="dialogHeight:"+250+"px; ";
    }
    
    if(canScroll){
        sFeatures+="scroll:"+canScroll+"; ";
    }else{
        sFeatures+="scroll:"+1+"; ";
    }
    
    if(!dialogArguments){
        dialogArguments=window;
    }
    
    sFeatures+="resizable: no; help: no; status: no;";
    return showModalDialog(url, dialogArguments, sFeatures);
}