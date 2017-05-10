
function escape2(str){
	return escape(str).replace(/\+/g,"%2b");
}

function addVote(id,mtype,btn)
{
    btn.value="请在提交...";
    btn.disabled=true;
    var voteNum;
    var rbVote=eval("document.voteform"+id).elements["vote"];
    for(var i=0;i<rbVote.length;i++)
    {
        if(rbVote[i].checked){
            if(!voteNum)
                voteNum=rbVote[i].value;
            else
                voteNum += "," + rbVote[i].value;
        }
    }
    if(!voteNum){
        alert("请先选择项目!");
        btn.disabled=false;
        btn.value="投票";
        return;
    }
    var option={
		parameters:"oper=addVote&id="+id+"&mtype="+mtype+"&vote="+voteNum+"&time="+Date(),
		method:"get",
		onSuccess:function(transport){
			var rp=transport.responseText;
			if(rp=="ok"){ 
			    alert("成功,谢谢你的投票!");
                btn.value="已投票";
			} else {
			    alert(rp);
			    btn.disabled=false;
                btn.value="投票";
			}
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
		}
	}
	var request=new Ajax.Request(InstallDir+"ajax.aspx",option);
	return;
}

function getVote(id,sobj)
{
    if($("voteDisplay") && $("voteDisplay").style.display!="none")
    {
        $("voteDisplay").style.display="none";
        return;
    }
    var h,tooltip,s,w;
    if(!$("voteDisplay"))
    {
        h=document.createElement("div");
        h.setAttribute("id","voteDisplay");
        h.className="siteInfo";
        h.style.position="absolute";
        document.getElementsByTagName("body")[0].appendChild(h);
        w=CreateEl("ul","voteClose");
        w.innerHTML="<input type=button onclick=\"getVote(0,0)\" value=关闭>";
        $("voteDisplay").appendChild(w);
        tooltip=CreateEl("ul","tooltip");
        
        tooltip.innerHTML="正在加载数据...";
        $("voteDisplay").appendChild(tooltip);
    }
    $("voteDisplay").style.display="block";
    
    var ttop=sobj.offsetTop;
    if(sobj.clientHeight!="undefined")ttop+=sobj.clientHeight+5;
    var ttleft=sobj.offsetLeft;
    while(sobj=sobj.offsetParent){ttop+=sobj.offsetTop;ttleft+=sobj.offsetLeft;}
    if((ttleft+300)>screen.width)ttleft=screen.width-300;
    $("voteDisplay").style.top=ttop-30;
    $("voteDisplay").style.left=ttleft+40;

    var option={
		parameters:"oper=getVote&id="+id+"&time="+Date(),
		method:"get",
		onSuccess:function(transport){
			var rp=transport.responseText;
			document.getElementsByClassName("tooltip",$("voteDisplay"))[0].innerHTML=rp;
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
		}
	}
	var request=new Ajax.Request(InstallDir+"ajax.aspx",option);
	return;
}
function CreateEl(t,c){
    var x=document.createElement(t);
    x.className=c;
    x.style.display="block";
    return(x);
}
function getViewNum(id)
{
    var rp;
    var option={
		parameters:"oper=getViewNum&id="+id+"&cType="+ChannelType+"&time="+Date(),
		method:"get",
		onSuccess:function(transport){
			rp=transport.responseText;
			$("getViewNum"+id).innerHTML=rp;
		},
		onFailure:function(transport){
			$("getViewNum"+id).innerHTML=0;
		}
	}
	var request=new Ajax.Request(InstallDir+"ajax.aspx",option);
}
function getReviewNum(id)
{
    var option={
		parameters:"oper=getReviewNum&id="+id+"&cType="+ChannelType+"&time="+Date(),
		method:"get",
		onSuccess:function(transport){
		    var rp=transport.responseText;
			$("getReviewNum"+id).innerHTML=rp;
		},
		onFailure:function(transport){
			$("getReviewNum"+id).innerHTML=0;
		}
	}
	var request=new Ajax.Request(InstallDir+"ajax.aspx",option);
}
function addFav(id)
{
    var option={
		parameters:"oper=addFav&id="+id+"&cType="+ChannelType+"&channel="+ChannelId+"&time="+Date(),
		method:"get",
		onSuccess:function(transport){
		    var rp=transport.responseText;
			if(rp=="ok")
			    alert("添加收藏成功!");
			else
			    alert(rp);
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
		}
	}
	
	var request=new Ajax.Request(InstallDir+"ajax.aspx",option);
	return;
}
function getLoginBar(w)
{
//参数说明:w=-1,0,1分别指"退出","初始化"和"登陆"
//x=0,1,2分别表示首页或动态,列表页和内容页
    var uName="";
    var uPass="";
	var vCode="";
    if(w==1)
    {
        uName=$("loginBarName").value;
        uPass=$("loginBarPass").value;
		vCode=$("txtCheckCode").value;
        if(!uName || !uPass || !vCode)return;
        $("btnLoginBarBtn").disabled=true;
    }
    
    var option={
		parameters:"name="+escape2(uName)+"&pass="+escape2(uPass)+"&code="+vCode+"&state="+w+"&time="+Date(),
		method:"post",
		onSuccess:function(transport){
		    var rp=transport.responseText;
		    if(rp!="")
		    {
			    if(rp=="ok")
			        location.reload();
			    else if(rp.indexOf("error=")>-1)
			    {
			        alert(rp.substr(6,rp.length-5));
			    }
			    else
			    {
			        $("loginBarPass").value="";
			        $("loginBarContent").innerHTML=rp;
			    }
			}
			$("btnLoginBarBtn").disabled=false;
		},
		onFailure:function(transport){
			//alert("数据提交失败，请检查网络或重试。");
		}
	}
	var request=new Ajax.Request(InstallDir+"ajax.aspx?oper=login",option);
	return;
}
function addReview(id)
{
//参数说明 x=0,1表示动态和静态页
    var uName=$("reviewName").value;
    var content=$("reviewContent").value;
    if(!uName || !content || content.length<10) {
        alert("评论字符太少!");
        return;
    }
    if(content.length>250){
        alert("评论字符太多");
        return;
    }
    $("btnAddReview").disabled=true;
    
    var option={
		parameters:"id="+id+"&name="+escape2(uName)+"&content="+escape2(content)+"&stype="+ChannelType,
		method:"post",
		onSuccess:function(transport){
		    var rp=transport.responseText;
			if(rp=="ok")
			{
			    $("reviewName").value="";
			    $("reviewContent").value="";
			    alert("发表成功!");
			}
			else if(rp == "okb")
			{
			    $("reviewName").value="";
			    $("reviewContent").value="";
			    alert("发表成功,但需要管理员审核!");
			}
			else
			{
			    alert(rp);
			    $("btnAddReview").disabled=false;
			}
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
			$("btnAddReview").disabled=false;
		}
	}
	
	var request=new Ajax.Request(InstallDir+"ajax.aspx?oper=addReview&time="+Date(),option);
	return;
}

function GetContentPage(page, pCount)
{
    for(var i=1; i<= pCount; i++)
    {
        $("ContentBodyPart"+i).style.display='none';
    }
    var nods = $("ContentBodyPart"+page).childNodes;
    for(var i=0;i<nods.length;i++)
    {
        if(nods[i].nodeName=="#comment")
            $("ContentBodyPart"+page).innerHTML=nods[i].nodeValue;
    }
    $("ContentBodyPart"+page).style.display='';
    var imgs=$("ContentBodyPart"+page).getElementsByTagName("img");
    for(var i=0; i<imgs.length;i++){
        imgs[i].onclick = function(){window.open(InstallDir + "Redirect.aspx?url=" + this.src); };
        imgs[i].style.cursor = "pointer";
        imgs[i].onmousewheel = function(){
            var zoom=parseInt(this.style.zoom, 10)||100;
            zoom+=event.wheelDelta/12;
            if (zoom>0)
                this.style.zoom=zoom+'%';
        }
    }
}

function CheckUserExist()
{
    $("btnReg").disabled=true;
    var uName=$("txtUserName").value;
    if(uName=="")return;
    var option={
		parameters:"name="+escape2(uName),
		method:"post",
		onSuccess:function(transport){
		    var rp=transport.responseText;
			if(rp=="ok")
			{
			    $("btnReg").disabled=false;
			}
			else
			{
			    alert("\"" + uName + "\"已经存在，请重新填写用户名！");
			    $("txtUserName").value="";
			    $("txtUserName").focus();
			}
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
			$("btnReg").disabled=true;
		}
	}
	
	var request=new Ajax.Request(InstallDir+"ajax.aspx?oper=checkUserExist&time="+Date(),option);
	return;
}
function CheckMailExist()
{
    $("btnReg").disabled=true;
    var mail=$("txtUserMail").value;
    if(mail=="")return;
    var option={
		parameters:"mail="+escape2(mail),
		method:"post",
		onSuccess:function(transport){
		    var rp=transport.responseText;
			if(rp=="ok")
			{
			    $("btnReg").disabled=false;
			}
			else
			{
			    alert("\"" + mail + "\"已经被使用，请重新填写邮箱！");
			    $("txtUserMail").value="";
			    $("txtUserMail").focus();
			}
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
			$("btnReg").disabled=true;
		}
	}
	
	var request=new Ajax.Request(InstallDir+"ajax.aspx?oper=checkMailExist&time="+Date(),option);
	return;
}

function CheckValidateCode()
{
    $("btnReg").disabled=true;
    var code = $("txtCheckCode").value;
    if(code=="")return;
    var option={
		parameters:"code="+code,
		method:"post",
		onSuccess:function(transport){
		    var rp=transport.responseText;
			if(rp=="ok")
			{
			    $("btnReg").disabled=false;
			}
			else
			{
			    alert("验证码错误，请重新填写！");
			    $("txtCheckCode").value="";
			    $("txtCheckCode").focus();
			    $("ValidateCode").src=InstallDir+"validateimg.aspx";
			}
		},
		onFailure:function(transport){
			alert("数据提交失败，请检查网络或重试。");
			$("btnReg").disabled=true;
		}
	}
	
	var request=new Ajax.Request(InstallDir+"ajax.aspx?oper=checkValidateCode&time="+Date(),option);
	return;
}

function setFrameHeight(obj)
{
 var win=obj;
 if (document.getElementById)
 {
  if (win && !window.opera)
  {
   if (win.contentDocument && win.contentDocument.body.offsetHeight)

    win.height = win.contentDocument.body.offsetHeight;
   else if(win.Document && win.Document.body.scrollHeight)
    win.height = win.Document.body.scrollHeight;
  }
 }
}