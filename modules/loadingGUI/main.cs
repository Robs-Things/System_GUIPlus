// New Loading GUI - Support Code

loadingGui.delete();
exec("./profiles.cs");
exec("./newLoadingGUI.gui");

// Preview Downloader

function NLGUI_fetchPreview(%ip, %port)
{
    if(isObject(NLGUI_ImageFetch))
        NLGUI_ImageFetch.delete();
    
    if(isFile("config/client/GUIPlus/imgCache/detail.jpg"))
        fileDelete("config/client/GUIPlus/imgCache/detail.jpg");

    NLGUI_PreviewImage.setBitmap("Add-ons/system_guiplus/modules/loadingGUI/ui/defPrev.png");
    
    echo("Fetching preview for: " @ %ip SPC %port);
    %ip = strReplace(%ip, ".", "-");
    %port = %port;
    
    %data = %ip @ "_" @ %port;
    
    %packet = %packet @ "GET /image.blockland.us/detail.php?q=" @ %data @ " HTTP/1.1\r\n";
    %packet = %packet @ "Host: imager.blockotec.com\r\n";
    %packet = %packet @ "\r\n";
    
    %tcp = new TCPObject(NLGUI_ImageFetch)
    {
        packet = %packet;
    };
    
    %tcp.connect("imager.blockotec.com:80");
}

function NLGUI_ImageFetch::onConnected(%this)
{
    %this.send(%this.packet);
}

function NLGUI_ImageFetch::onLine(%this, %line)
{
    if(getWord(%line, 0) $= "Content-Length:")
    {
        %this.length = getWord(%line, 1);
    }
    
    if(%line $= "")
    {            
        %this.setBinary(true);
    }
}

function NLGUI_ImageFetch::onBinChunk(%this, %chunk)
{
    cancel(%this.timeout);
	%this.timeout = %this.schedule(500, finish);
}

function NLGUI_ImageFetch::finish(%this)
{
    %this.saveBufferToFile("config/client/GUIPlus/imgCache/detail.jpg");
    NLGUI_PreviewImage.schedule(1000, setBitmap, "config/client/GUIPlus/imgCache/detail.jpg");
    %this.disconnect();
    %this.delete();
}

// Server Details

function NLGUI_SetServerDetails()
{
    NLGUI_HostName.setText("<font:Arial:36><color:7A7A7A>" @ $Blota::NLGUI::SelectedServer["host"]);
    NLGUI_ServerName.setText("<font:Arial:40><color:000000>" @ $Blota::NLGUI::SelectedServer["name"]);
    NLGUI_Players.setText("<font:Arial:20><color:C4C4C4>Players: " @ $Blota::NLGUI::SelectedServer["playerCount"] @ "/" @ $Blota::NLGUI::SelectedServer["maxPlayers"]);
    NLGUI_Bricks.setText("<font:Arial:20><color:C4C4C4>Bricks: " @ $Blota::NLGUI::SelectedServer["brickCount"]);
    NLGUI_Ping.setText("<font:Arial:20><color:C4C4C4>Ping: " @ $Blota::NLGUI::SelectedServer["ping"] @ "ms");
    
    if($Blota::NLGUI::Mode $= "internet")
      NLGUI_fetchPreview($Blota::NLGUI::SelectedServer["IP"], $Blota::NLGUI::SelectedServer["port"]);
      
    if($Blota::NLGUI::Mode $= "local")
      NLGUI_PreviewImage.setBitmap("Add-ons/system_guiplus/modules/loadingGui/ui/local");
    
    NLGUI_PurgePlayerList();
            
    schedule(1000, 0, NLGUI_PingUpdate);
    schedule(2000, 0, LoadingGUI_initiateChat);
}

package blota_joinServerPackage
{
    function JoinServerGui::join(%this)
    {
        parent::join(%this);
        %SDO = ServerInfoGroup.getObject(JS_serverList.getSelectedID());
        $Blota::NLGUI::SelectedServer["name"] = getField(strReplace(%SDO.name, "\'" , "\t"), 1);
        $Blota::NLGUI::SelectedServer["host"] = getField(strReplace(%SDO.name, "\'" , "\t"), 0) @ "\'s";
        $Blota::NLGUI::SelectedServer["playerCount"] = %SDO.currPlayers;
        $Blota::NLGUI::SelectedServer["maxPlayers"] = %SDO.maxPlayers;
        $Blota::NLGUI::SelectedServer["brickCount"] = %SDO.brickCount;
        $Blota::NLGUI::SelectedServer["ping"] = %SDO.ping;
        $Blota::NLGUI::SelectedServer["IP"] = getField(strReplace(%SDO.ip, ":", "\t"), 0);
        $Blota::NLGUI::SelectedServer["port"] = getField(strReplace(%SDO.ip, ":", "\t"), 1);
        $Blota::NLGUI::Mode = "internet";
        
        if(getWord($Blota::NLGUI::SelectedServer["name"], 0) $= "s")
            $Blota::NLGUI::SelectedServer["name"] = restWords($Blota::NLGUI::SelectedServer["name"]);

        NLGUI_SetServerDetails();
    }
    
    function NewPlayerListGui::update(%this, %cl, %name, %BL_ID, %trust, %admin, %score)
	 {
	    parent::update(%this, %cl, %name, %BL_ID, %trust, %admin, %score);
       NLGUI_AddPlayerToList(%name, %BL_ID, %score);
    }
    
    function createServer(%type)
    {
       if(%type $= "internet")
       {
          $Blota::NLGUI::SelectedServer["name"] = $Pref::Server::Name;
          $Blota::NLGUI::SelectedServer["host"] = $pref::Player::NetName;
          $Blota::NLGUI::SelectedServer["playerCount"] = 1;
          $Blota::NLGUI::SelectedServer["maxPlayers"] = $Pref::Server::MaxPlayers;
          $Blota::NLGUI::SelectedServer["brickCount"] = 0;
          $Blota::NLGUI::Mode = "local";
       }
       if(%type $= "SinglePlayer")
       {
          $Blota::NLGUI::SelectedServer["name"] = "Single Player";
          $Blota::NLGUI::SelectedServer["host"] = $pref::Player::LanName;
          $Blota::NLGUI::SelectedServer["playerCount"] = 1;
          $Blota::NLGUI::SelectedServer["maxPlayers"] = 1;
          $Blota::NLGUI::SelectedServer["brickCount"] = 0;
          $Blota::NLGUI::Mode = "local";
       }
       NLGUI_SetServerDetails();
       parent::createServer(%type);
    }
};
activatePackage(blota_joinServerPackage);

function LoadingGUI_initiateChat()
{
    if(isObject(serverConnection))
    {
        NLGUI_BG.add(NewChatHud);
        moveMap.push();
    }
}

function NLGUI_PingUpdate()
{
    cancel($Blota::NLGUI::PingUpdate);
    
   
    if(isObject(ServerConnection))
    { 
        NLGUI_Ping.setText("<font:Arial:20><color:C4C4C4>Ping: " @ serverConnection.getPing() @ "ms");    
        $Blota::NLGUI::PingUpdate = schedule(2000, 0, NLGUI_PingUpdate);
    }
}

function NLGUI_AddPlayerToList(%name, %BL_ID, %score)
{
    $Blota::NLGUI::CurrID++;
    
    %yPos = 20 * $Blota::NLGUI::CurrID;
    %yheight = 20 * ($Blota::NLGUI::CurrID + 2);
    
    %a = new GuiMLTextCtrl() {
      profile = "GuiMLTextProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "3" SPC %yPos;
      extent = "239 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      lineSpacing = "2";
      allowColorChars = "0";
      maxChars = "-1";
      text = "<font:Arial:18><color:666666>" @ %name;
      maxBitmapHeight = "-1";
      selectable = "1";
      autoResize = "1";
   };
   %b = new GuiMLTextCtrl() {
      profile = "GuiMLTextProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "253" SPC %yPos;
      extent = "94 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      lineSpacing = "2";
      allowColorChars = "0";
      maxChars = "-1";
      text = "<font:Arial:18><color:666666>" @ %BL_ID;
      maxBitmapHeight = "-1";
      selectable = "1";
      autoResize = "1";
   };
   %c = new GuiMLTextCtrl() {
      profile = "GuiMLTextProfile";
      horizSizing = "right";
      vertSizing = "bottom";
      position = "354" SPC %yPos;
      extent = "94 18";
      minExtent = "8 2";
      enabled = "1";
      visible = "1";
      clipToParent = "1";
      lineSpacing = "2";
      allowColorChars = "0";
      maxChars = "-1";
      text = "<font:Arial:18><color:666666>" @ %score;
      maxBitmapHeight = "-1";
      selectable = "1";
      autoResize = "1";
   };
   
   NLGUI_Scroll_Canvas.add(%a);
   NLGUI_Scroll_Canvas.add(%b);
   NLGUI_Scroll_Canvas.add(%c);
   
   if(getWord(NLGUI_Scroll_Expander.extent, 1) < %yheight)
        NLGUI_Scroll_Expander.resize(1, 1, 418, %yheight);

}

function NLGUI_PurgePlayerList()
{
    NLGUI_Scroll_Canvas.clear();
    NLGUI_Scroll_Expander.resize(1, 1, 418, 274);
    $Blota::NLGUI::CurrID = 0;
}