FCKCommands.RegisterCommand(
     'RemoteImageRubber',
     new FCKDialogCommand( 'RemoteImageRubber', 
         FCKLang.RemoteImageRubberBtn, 
         FCKPlugins.Items['remoteimagerubber'].Path + 'remoteimagerubber.aspx', 
         350, 
         200 )
 ) ;
 var oBtn=new FCKToolbarButton('RemoteImageRubber',null,FCKLang.RemoteImageRubberBtn,null,false,true,67);
 FCKToolbarItems.RegisterItem('RemoteImageRubber',oBtn);