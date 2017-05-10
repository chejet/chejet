function MD(d){
	//取到本月的最大日期
	for(var i=31;i>=28;i--){
		var tempDate = new Date(d.getFullYear(),d.getMonth(),i);
		if(tempDate.getFullYear()==d.getFullYear()&&d.getMonth()==tempDate.getMonth())return i;
	}	
}
function WC(d,tobj){
//写日历内容
	var imgpath="images/"; //图片路径
	var seldaypath="";
	var curdaypath="";
	var mstr="一月,二月,三月,四月,五月,六月,七月,八月,九月,十月,十一月,十二月";
	var ccd=new Date();//当日日期
	if(d==""||typeof(d)=="undefined")d=new Date();
	//日历表头
	var ss="";
	ss+="<table style='border:#183789 1px solid;border-collapse: collapse;' border=1 bordercolor='#ffffff' cellspacing=0 cellpadding=3 width='180' bgColor='#ECF2FC'>";
	ss+="<tr height=22>";
	ss+="<td colspan=7 style='text-align:center'>";
	//item1
	//Year List Mode
	ss+="<select style='height:20;width:70px'";
	ss+=" onChange=\"MC(this.value+'-"+(d.getMonth()+1)+"-"+d.getDate()+"','"+tobj+"')\">";
	var yy=parseInt(d.getFullYear());
	for(var j=(yy-50);j<=(yy+50);j++){
		ss+="<option value='"+j+"'";
		ss+=((j==yy)?" selected":"");
		ss+=">"+j+"年</option>";
	}
	ss+="</select>";
	//item2
	//Month List
	ss+="<select style='height:20px;width:70px'";
	ss+=" onChange=\"MC('"+d.getFullYear()+"-'+this.value+'-"+d.getDate()+"','"+tobj+"')\">";
	var msa=mstr.split(",");
	for(var i=0;i<12;i++){
		ss+="<option value="+(i+1);
		ss+=(d.getMonth()==i)?" selected ":"";
		ss+=">"+msa[i]+"</option>";
	}
	ss+="</select>";
	//month list end
	ss+="<input type='button' value='关闭' style='color:red' onclick='CC()'></td></tr>";
	ss+="<tr bgcolor='#183789'>";
	ss+="<td style='text-align:center;'><font style='color:#fff;font-size:9pt'>日</font></td>";
	ss+="<td style='text-align:center'><font style='color:#fff;font-size:9pt'>一</font></td>";
	ss+="<td style='text-align:center'><font style='color:#fff;font-size:9pt'>二</font></td>";
	ss+="<td style='text-align:center'><font style='color:#fff;font-size:9pt'>三</font></td>";
	ss+="<td style='text-align:center'><font style='color:#fff;font-size:9pt'>四</font></td>";
	ss+="<td style='text-align:center'><font style='color:#fff;font-size:9pt'>五</font></td>";
	ss+="<td style='text-align:center'><font style='color:#fff;font-size:9pt'>六</font></td>";
	ss+="</tr>";
	var mbd = new Date(d.getFullYear(),d.getMonth(),1);//指定日期的1号
	var bd=mbd.getDay();//月份开始的星期
	maxdate = MD(d);//指定日期当月的最大天数
	var cd=d.getDate();//当前日期
	var cr=((maxdate+bd)%7==0)?((maxdate+bd)/7):parseInt((maxdate+bd)/7+1);
	//写日历主体
	for(var i=0;i<cr;i++){
		ss+="<tr>";
		for(var j=0;j<7;j++){
			var tv=(i*7+j-bd+1);
			if((i==0&&j<bd)||tv>maxdate){ss+="<td>&nbsp;</td>";}else{
				ss+="<td valign='middle' style='height:12px;text-align:center;cursor:hand;font-size:9pt;' ";
				ss+="onclick=\"RD("+tobj+",'"+d.getFullYear()+"-"+(d.getMonth()+1)+"-"+tv+"')\" ";
				ss+="onmouseover=\"this.style.backgroundColor='#CED7F7'\" onmouseout=\"this.style.backgroundColor=''\">"
				if(d.getFullYear()==ccd.getFullYear()&&d.getMonth()==ccd.getMonth()&&ccd.getDate()==tv){
					//当前日期的显示
					ss+="<font color=red><b>"+tv+"</b></font>";
				}else{
					ss+=((cd==tv)?("<font color='green'><b>"+tv+"</b></font>"):tv);
				}
				ss+="</td>";
			}
		}
		ss+="</tr>";
	}
	//表格结束
	ss+="<tr><td colspan=7 style='text-align:center'>今天：<font style='font-size:9pt;color:#00f'><b>"+ccd.getFullYear()+"-"+(ccd.getMonth()+1)+"-"+ccd.getDate()+"</b></font></td></tr>";
	ss+="</table>";
	return ss;	
}
function SD(sobj,tobj){
	var tt = eval(tobj);
	var ds=tt.value;
	var d;
	if(ds==""){d=new Date();}else{var da=ds.split("-"); if(da[2].length>2)da[2]=da[2].substring(0,2);var d=new Date(da[0],da[1]-1,da[2]);};
	if(typeof(document.all.calendar)!="object"){		
			document.body.insertAdjacentHTML("afterBegin","<div id='calendar' style='position:absolute;display:none;z-index:99;width:175px;margin:0;padding:0;text-align:center;'></div>");
	}else{
		document.all.calendar.style.display="none";
	}
	//document.write(WC("",tobj))
	var ttop=sobj.offsetTop;
	if(sobj.clientHeight!="undefined")ttop+=sobj.clientHeight+5;
	var ttleft=sobj.offsetLeft;
	while(sobj=sobj.offsetParent){ttop+=sobj.offsetTop;ttleft+=sobj.offsetLeft;}
	if((ttleft+300)>screen.width)ttleft=screen.width-300;
	document.all.calendar.innerHTML=WC(d,tobj);
	document.all.calendar.style.left=ttleft;
	document.all.calendar.style.top=ttop;
	document.all.calendar.style.display="";
}
function RD(obj,d){
    var da = new Date();
	obj.value = d + " " + da.toLocaleTimeString();
	CC();
}
function CC(){
	document.all.calendar.style.display="none";
}
function MC(dstr,objstr){
	var da=dstr.split("-");var d=new Date(da[0],da[1]-1,da[2]);
	document.all.calendar.innerHTML=WC(d,objstr);
}