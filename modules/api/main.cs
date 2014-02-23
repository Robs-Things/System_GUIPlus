// Api module
// Author: Fluffy

exec("./PlusUpdate.gui");

function GUIPlus_APIInit()
{
   if(!isObject(GUIP_API))
      new TCPObject(GUIP_API);
   
   GUIP_API.mode = 1;
   GUIP_API.connect("api.blockotec.com:80");
}

function GUIP_API::onConnected(%this)
{
   switch(%this.mode)
   {
      case 1:
         %data = "m=a&v=" @ $GUIPlus::Version @ "&n=" @ $pref::Player::NetName;
      case 2:
         %data = "m=u";
   }
   %packet = "POST /api.php HTTP/1.1\r\nHost: api.blockotec.com\r\nConnection: Close\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: " @ strlen(%data) @ "\r\n\r\n" @ %data @ "\r\n";
   %this.send(%packet);
}

function GUIP_API::onLine(%this, %line)
{
   if(getField(%line, 0) $= "901")
   {
      %ver = getField(%line, 1);
      %log = getField(%line, 2);
      %log = strReplace(%log, "<NL>", "\n");
      
      PlusUpdate_CV.setText("Current Version: " @ %ver);
      PlusUpdate_Changelog.setText(%log);
      canvas.pushDialog(PlusUpdate);
      %this.mode = 0;
      %this.disconnect();
   }
   
   if(getWord(%line, 0) $= "Content-Length:" && %this.mode $= "2")
   {
      %this.length = getword(%line, 1);
   }
   if(%line $= "" && %this.mode $= "2")
   {
      %this.setBinarySize(%this.length);
   }
}

function GUIP_API::onBinChunk(%this, %chunk)
{
   %perCent = mFloatLength(%chunk / %this.length * 100, 0);
   PlusUpdate_SetProgress(%perCent);
   
   if(%chunk >= %this.length) 
   {
      %this.saveBufferToFile("Add-Ons/System_GUIPlus.zip");
      %this.disconnect();
      %this.setBinary(false);
      quit();
   }
}

function GUIPlus_DoUpdate()
{
   GUIP_API.mode = 2;
   GUIP_API.connect("api.blockotec.com:80");
   
   PlusUpdate_yep.setVisible(0);
   PlusUpdate_nope.setVisible(0);
   PlusUpdate_LoadingBar.setVisible(1);
   PlusUpdate_SetProgress(0);
}

function PlusUpdate_SetProgress(%percent)
{
   %val = -175 + mCeil(1.85 * %percent);
   PlusUpdate_progressbar.position = %val SPC 4;
   PlusUpdate_Percentage.setText("<font:arial:14>" @ %percent @ "%");
}