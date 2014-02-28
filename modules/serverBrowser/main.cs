exec("./profiles.cs");
exec("./ServerBrowser.gui");

JS_serverList.delete();
//Allows us to use the JS_serverList object as our lan browser
$ServerBrowser::Mode = "grid";
// Here we go

// Master Server Parser
// Preview Imaging
// Listing generation
// Support functions
// Favourites

// Master server parser
function ServerBrowser_FetchServerList(%old, %search)
{
   serverBrowser_Canvas.clear();
   ServerBrowser_ListList.clear();
   ServerBrowser_InputClick.setVisible(1);
   
   if(!isObject(ServerBrowser_HTTPObject))
      new HTTPObject(ServerBrowser_HTTPObject);
      
   if(!%old)
      ServerBrowser_flushImgCache("config/client/GUIPlus/serverbrowser/previews/*.jpg");
   
   ServerBrowser_HTTPObject.renderServers = false;
   ServerBrowser_HTTPObject.useOld = %old;
   ServerBrowser_HTTPObject.search = strUpr(%search);
   ServerBrowser_HTTPObject.get("master2.blockland.us:80", "/index.php");
}

function ServerBrowser_HTTPObject::onLine(%this, %line)
{
   if(%line $= "END")
   {
      %this.renderServers = false;
      ServerBrowser_StartPrevDownload(0, %this.useOld);
   }
      
   if(%this.renderServers)
   {
      if(strLen(%this.search) > 0 && strPos(strUpr(%line), %this.search) !$= "-1")
         ServerBrowser_RenderServer(%line);
      else if(strLen(%this.search) $= 0)
         ServerBrowser_RenderServer(%line);
   }
   
   if(%line $= "START")
   {
      %this.renderServers = true;
      $ServerBrowser::NumServers = 0;
   }
}

function ServerBrowser_RenderServer(%rawData)
{
   %server_ip           = getField(%rawData, 0);
   %server_port         = getField(%rawData, 1);
   %server_passworded   = getField(%rawData, 2);
   %server_dedicated    = getField(%rawData, 3);
   %server_servername   = getField(%rawData, 4);
   %server_players      = getField(%rawData, 5);
   %server_maxplayers   = getField(%rawData, 6);
   %server_mapname      = getField(%rawData, 7);
   %server_brickcount   = getField(%rawData, 8);
   
   %serverName = getField(strReplace(%server_servername, "\'" , "\t"), 1);
   
   if(getWord(%serverName, 0) $= "s")
      %serverName = restWords(%serverName);
      
   %host = getField(strReplace(%server_servername, "\'" , "\t"), 0);
   
   //List View
   ServerBrowser_ListList.addRow($ServerBrowser::NumServers, %server_servername TAB strReplace(strReplace(%server_dedicated, "1", "Yes"), "0", "No") TAB strReplace(strReplace(%server_passworded, "1", "Yes"), "0", "No") TAB %server_players @ "/" @ %server_maxplayers TAB %server_brickcount TAB %server_mapname);
   //Grid View
   
   //pos
   %row = mFloor($ServerBrowser::NumServers / 4);
   %col = $ServerBrowser::NumServers - (%row * 4);
   
   %posX = %col * (154 + 10);
   %posY = %row * (214 + 10);
   
   %colour = "222222";
   if(%server_passworded)
      %colour = "f7941d";      
      
   %obj = new GuiSwatchCtrl("ServSwatch_" @ $serverBrowser::NumServers) {
      profile = "GuiDefaultProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = %posX SPC %posY;
      extent = "154 214";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      color = "0 0 0 0";
      ip = %server_ip;
      port = %server_port;
      servName = %serverName;
      servhost = %host;

      new GuiBitmapCtrl("ServPreview_" @ $serverBrowser::NumServers) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "0 0";
         extent = "154 154";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "add-ons/system_guiplus/modules/serverbrowser/ui/placeholder";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
      };
      new GuiMLTextCtrl() {
         profile = "GuiMLTextProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "1 158";
         extent = "400 16";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         lineSpacing = "2";
         allowColorChars = "0";
         maxChars = "-1";
         text = "<font:arial:17><color:" @ %colour @ ">" @ %serverName;
         maxBitmapHeight = "-1";
         selectable = "1";
         autoResize = "1";
      };
      new GuiMLTextCtrl() {
         profile = "GuiMLTextProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "1 174";
         extent = "400 16";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         lineSpacing = "2";
         allowColorChars = "0";
         maxChars = "-1";
         text = "<font:arial:15><color:333333>by<font:arial bold:16> " @ %host;
         maxBitmapHeight = "-1";
         selectable = "1";
         autoResize = "1";
      };
      new GuiMLTextCtrl() {
         profile = "GuiMLTextProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "2 197";
         extent = "82 16";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         lineSpacing = "2";
         allowColorChars = "0";
         maxChars = "-1";
         text = "<color:333333><font:arial bold:16>" @ %server_players @ "/" @ %server_maxplayers @ "<font:arial:16> players";
         maxBitmapHeight = "-1";
         selectable = "1";
         autoResize = "1";
      };
      new GuiBitmapCtrl("ServPingImg_" @ $serverBrowser::NumServers) {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "105 198";
         extent = "17 14";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         bitmap = "./ui/ping_none";
         wrap = "0";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         keepCached = "0";
         mColor = "255 255 255 255";
         mMultiply = "0";
      };
      new GuiMLTextCtrl("ServPing_" @ $serverBrowser::NumServers) {
         profile = "GuiMLTextProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "122 197";
         extent = "33 16";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         lineSpacing = "2";
         allowColorChars = "0";
         maxChars = "-1";
         text = "<font:arial:16><color:e9bc21>???";
         maxBitmapHeight = "-1";
         selectable = "1";
         autoResize = "1";
      };
      new GuiBitmapButtonCtrl("GridButton_" @ $ServerBrowser::NumServers) {
            profile = "GuiDefaultProfile";
            horizSizing = "right";
            vertSizing = "bottom";
            position = "0 0";
            extent = "154 214";
            minExtent = "8 2";
            enabled = "1";
            visible = "1";
            clipToParent = "1";
            command = "ServerBrowser_Join(\"" @ %rawData TAB %host TAB %serverName @ "\");";
            altCommand = "ServerBrowser_CreateRCMenu(" @ $ServerBrowser::NumServers @ ");";
            text = "";
            groupNum = "-1";
            buttonType = "PushButton";
            bitmap = "add-ons/system_guiplus/modules/serverbrowser/ui/buttons/over";
            lockAspectRatio = "0";
            alignLeft = "0";
            alignTop = "0";
            overflowImage = "0";
            mKeepCached = "0";
            mColor = "255 255 255 255";
         };
   };
   $ServerBrowser::NumServers++;
   serverBrowser_Canvas.add(%obj);
   %yResize = (%row + 1) * (214 + 20);
   serverBrowser_Canvas.resize(1, 1, 675, %yResize);
}

function ServerBrowser_StartPrevDownload(%id, %useOld)
{   
   %ip = ("ServSwatch_" @ %id).ip;
   %port = ("ServSwatch_" @ %id).port;
   %ip = strReplace(%ip, ".", "-");
   %data = %ip @ "_" @ %port;
   
   // Check if we've already downloaded a preview
   if(%useOld)
   {
      if(isFile("config/client/GUIPlus/serverbrowser/previews/" @ %data @ ".jpg"))
      {
         //Hold the show! we can save time and fluffy's bandwidth by using an older image!
         ("ServPreview_" @ %id).setBitmap("config/client/GUIPlus/serverbrowser/previews/" @ %data @ ".jpg");
         
         if(%id < $ServerBrowser::NumServers - 1)
	         ServerBrowser_StartPrevDownload(%id + 1, %useOld);
         
         return;
      }
   }
   
   if(isObject(Serverbrowser_PrevTCP))
      ServerBrowser_PrevTCP.delete();
      
   new TCPObject(ServerBrowser_PrevTCP);
   
   ServerBrowser_PrevTCP.useOld = %useOld;
   ServerBrowser_PrevTCP.servID = %id;
   ServerBrowser_PrevTCP.data = %data;
   ServerBrowser_PrevTCP.connect("imager.blockotec.com:80");
}

function ServerBrowser_PrevTCP::onConnected(%this)
{
   %packet = "GET /image.blockland.us/thumb/" @ %this.data @ ".jpg HTTP/1.1\r\nHost:imager.blockotec.com\r\n\r\n";
   %this.send(%packet);
}

function ServerBrowser_PrevTCP::onLine(%this, %line)
{
   if(firstWord(%line) $= "Content-Length:")
   {
      %this.size = getWord(%line, 1);
      if(%this.size < 400)
      {
         ("ServPreview_" @ %this.servID).schedule(200, setBitmap, "add-ons/system_guiplus/modules/serverbrowser/ui/placeholder2");
         %this.disconnect();
         %this.size = 0;
         if(%this.servID < $ServerBrowser::NumServers - 1)
	         ServerBrowser_StartPrevDownload(%this.servID + 1, %this.useold);
	         
         return;
      }
   }
      
   if(%line $= "" && %this.size > 0)
      %this.setBinarySize(%this.size);
}

function ServerBrowser_PrevTCP::onBinChunk(%this, %chunk)
{
   if(%chunk < %this.size)
		return;

	%this.saveBufferToFile("config/client/GUIPlus/serverbrowser/previews/" @ %this.data @ ".jpg");
	%this.disconnect();
	
	("ServPreview_" @ %this.servID).schedule(200, setBitmap, "config/client/GUIPlus/serverbrowser/previews/" @ %this.data @ ".jpg");
	
	if(%this.servID < $ServerBrowser::NumServers - 1)
	   ServerBrowser_StartPrevDownload(%this.servID + 1, %this.useOld);
}

function ServerBrowser_Join(%data)
{
   // Set up loading GUI module
   $Blota::NLGUI::SelectedServer["name"] =         getField(%data, 11);
   $Blota::NLGUI::SelectedServer["host"] =         getField(%data, 10);
   $Blota::NLGUI::SelectedServer["playerCount"] =  getField(%data, 5);
   $Blota::NLGUI::SelectedServer["maxPlayers"] =   getField(%data, 6);
   $Blota::NLGUI::SelectedServer["brickCount"] =   getField(%data, 8);
   $Blota::NLGUI::SelectedServer["ping"] = "???";
   $Blota::NLGUI::SelectedServer["IP"] =           getField(%data, 0);
   $Blota::NLGUI::SelectedServer["port"] =         getField(%data, 1);
   $Blota::NLGUI::Mode = "internet";

   NLGUI_SetServerDetails();
   
   if(!getField(%data, 2))
   {
      connectToServer(getField(%data, 0) @ ":" @ getField(%data, 1), "", 1, 1);
      return;
   }
   
   //Connecting with password
   MJ_txtIP.setValue(getField(%data, 0) @ ":" @ getField(%data, 1));
   MJ_txtIP.enabled = false;
   canvas.PushDialog(ManualJoin);
}

// Support functions

function ServerBrowser_toggleViewPane()
{
   %toggleState = ServerBrowser_ViewPanel.visible;
   
   if($ServerBrowser::Mode $= "grid")
   {
      if(%toggleState)
      {
         ServerBrowser_ViewPanel.setVisible(0);
         ServerBrowser_viewButton.setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/buttons/viewG");
      }
      else
      {
         ServerBrowser_ViewPanel.setVisible(1);
         ServerBrowser_viewButton.setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/buttons/viewGi");
      }
   }
   
   if($ServerBrowser::Mode $= "list")
   {
      if(%toggleState)
      {
         ServerBrowser_ViewPanel.setVisible(0);
         ServerBrowser_viewButton.setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/buttons/viewL");
      }
      else
      {
         ServerBrowser_ViewPanel.setVisible(1);
         ServerBrowser_viewButton.setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/buttons/viewLi");
      }
   }
}

function ServerBrowser_flushImgCache(%path)
{
    for (%i = findFirstFile(%path); strLen(%i) > 0; %i = findNextFile(%path)) 
    {
        fileDelete(%i);
    }
}

function ServerBrowser_SetGridView()
{
   $ServerBrowser::Mode = "grid";
   ServerBrowser_ListPane.setVisible(0);
   serverBrowser_scroll.setVisible(1);
   ServerBrowser_toggleViewPane();
   ServerBrowser_viewButton.setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/buttons/viewG");
}

function ServerBrowser_SetSecView()
{
   //Not enabled yet
}

function ServerBrowser_SetListView()
{
   $ServerBrowser::Mode = "list";
   ServerBrowser_ListPane.setVisible(1);
   serverBrowser_scroll.setVisible(0);
   ServerBrowser_toggleViewPane();
   ServerBrowser_viewButton.setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/buttons/viewL");
}

function ServerBrowser_SortList(%col)
{
   %bool = (ServerBrowser_SA @ %col).sortDir;
   %bool = !%bool;
   (ServerBrowser_SA @ %col).sortDir = %bool;
   ServerBrowser_ListList.sort(%col, %bool);
   
   if(%bool)
      (ServerBrowser_SA @ %col).setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/sortu");
   else
      (ServerBrowser_SA @ %col).setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/sortd");
}

function ServerBrowser_SortNUMList(%col)
{
   %bool = (ServerBrowser_SA @ %col).sortDir;
   %bool = !%bool;
   (ServerBrowser_SA @ %col).sortDir = %bool;
   ServerBrowser_ListList.sortNumerical(%col, %bool);
   
   if(%bool)
      (ServerBrowser_SA @ %col).setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/sortu");
   else
      (ServerBrowser_SA @ %col).setBitmap("add-ons/system_guiplus/modules/serverbrowser/ui/sortd");
}

function ServerBrowser_JoinButton()
{
   %id = ServerBrowser_ListList.getSelectedID();
   if(%id $= "-1")
      return;
      
   %cmd = ("GridButton_" @ %id).command;
   eval(%cmd);
}

function ServerBrowser_CreateRCMenu(%id)
{
   %pos = (getWord(canvas.getCursorPos(), 0) - 10) SPC (getWord(canvas.getCursorPos(), 1) - 10);
   
   %obj = new GuiWindowCtrl(minmen) {
      profile = "LoadingGuiWindowProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = %pos;
      extent = "122 38";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      maxLength = "255";
      resizeWidth = "0";
      resizeHeight = "0";
      canMove = "0";
      canClose = "0";
      canMinimize = "0";
      canMaximize = "0";
      minSize = "50 50";

      new GuiBitmapButtonCtrl() {
         profile = "GuiDefaultProfile";
         horizSizing = "right";
         vertSizing = "bottom";
         position = "2 2";
         extent = "117 34";
         minExtent = "8 2";
         enabled = "1";
         visible = "1";
         clipToParent = "1";
         command = "ServerBrowser_AddFav(" @ %id @ "); minmen.delete(); cancel($SB::CCPSched);";
         groupNum = "-1";
         buttonType = "PushButton";
         bitmap = "Add-Ons/System_GuiPlus/modules/serverBrowser/ui/buttons/fav";
         text = "";
         lockAspectRatio = "0";
         alignLeft = "0";
         alignTop = "0";
         overflowImage = "0";
         mKeepCached = "0";
         mColor = "255 255 255 255";
      };
   };
   ServerBrowser.add(%obj);
   ServerBrowser_CheckCursorPos(%obj);
}

function ServerBrowser_CheckCursorPos(%obj)
{
   cancel($SB::CCPSched);
   %curPos = canvas.getCursorPos();
   
   if(getWord(%curPos, 0) > getWord(%obj.position, 0) && getWord(%curPos, 0) < (getWord(%obj.position, 0) + getWord(%obj.extent, 0)) && getWord(%curPos, 1) > getWord(%obj.position, 1) && getWord(%curPos, 1) < (getWord(%obj.position, 1) + getWord(%obj.extent, 1)))
   {
      $SB::CCPSched = schedule(50, 0, ServerBrowser_CheckCursorPos, %obj);
      return;
   }
   %obj.delete();
}

function ServerBrowser_AddFav(%id)
{
   %ip = ("ServSwatch_" @ %id).ip;
   %port = ("ServSwatch_" @ %id).port;
   %name = ("ServSwatch_" @ %id).servName;
   %host = ("ServSwatch_" @ %id).ServHost;
   
   %line = %ip TAB %port TAB %host TAB %name;
   %check = %ip TAB %port;
   
   if(isFile("config/client/guiplus/fav.dat"))
   {
      %fo = new fileObject();
      %fo.openForRead("config/client/guiplus/fav.dat");
      %abort = false;
      while(!%fo.isEOF())
      {
         %file_line = %fo.readLine();
         if(%check $= (getField(%file_line, 0) TAB getField(%file_line, 1)))
         {
            echo("Trying to add duplicate favourite entry. Aborting...");
            %abort = true;
            break;
         }
      }
      %fo.close();
      %fo.delete();
      
      if(%abort)
         return;
   }
   
   %fo2 = new fileObject();
   %fo2.openForAppend("config/client/guiplus/fav.dat");
   %fo2.writeLine(%line);
   
   %fo2.close();
   %fo2.delete();
}

function ServerBrowser_INTTab()
{
   ServerBrowser_CloseALLPanes();
   serverBrowser_scroll.setVisible(1);
   ServerBrowser_viewButton.setVisible(1);
   ServerBrowser_RefreshButton.command = "ServerBrowser_FetchServerList(1);";
   ServerBrowser_TAB_Internet.setBitmap("Add-ons/System_GuiPlus/modules/serverBrowser/ui/buttons/tabc");
}

function ServerBrowser_LANTab()
{
   ServerBrowser_CloseALLPanes();
   ServerBrowser_LANPane.setVisible(1);
   JoinServerGui.queryLan();
   ServerBrowser_RefreshButton.command = "JoinServerGui.queryLan();";
   ServerBrowser_TAB_LAN.setBitmap("Add-ons/System_GuiPlus/modules/serverBrowser/ui/buttons/tabc");
}

function ServerBrowser_FAVTab()
{
   ServerBrowser_CloseALLPanes();
   serverBrowser_Favscroll.setVisible(1);
   ServerBrowser_RefreshButton.command = "ServerBrowser_RefreshFavList();";
   ServerBrowser_TAB_FAV.setBitmap("Add-ons/System_GuiPlus/modules/serverBrowser/ui/buttons/tabc");
}

function ServerBrowser_CloseALLPanes()
{
   serverBrowser_scroll.setVisible(0);
   ServerBrowser_ListPane.setVisible(0);
   ServerBrowser_LANPane.setVisible(0);
   serverBrowser_Favscroll.setVisible(0);
   ServerBrowser_viewButton.setVisible(0);
   ServerBrowser_TAB_LAN.setBitmap("Add-ons/System_GuiPlus/modules/serverBrowser/ui/buttons/tab");
   ServerBrowser_TAB_FAV.setBitmap("Add-ons/System_GuiPlus/modules/serverBrowser/ui/buttons/tab");
   ServerBrowser_TAB_Internet.setBitmap("Add-ons/System_GuiPlus/modules/serverBrowser/ui/buttons/tab");
}

function ServerBrowser_RefreshFavList()
{
   %fo = new FileObject();
   %fo.openForRead("config/client/guiplus/fav.dat");
   
   while(!%fo.isEOF())
   {
      %line = %fo.readLine();
      
      ServerBrowser_RenderServer();
   }
}