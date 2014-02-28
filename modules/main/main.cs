// Exec sub modules
exec("./profiles.cs");
exec("./iMain.gui");

function iMain_init()
{
	mainMenuGui.setName("oldMainMenu");
	iMain.setName("mainMenuGui");
	MainMenuButtonsGui.delete();
	canvas.pushDialog("mainMenuGui");
	canvas.popDialog("oldMainMenu");
}

function iMain_timeLoop()
{
	cancel($iMain::TimeLoop);
	
	%time = getSubStr(getDateTime(),9,5);
	%len = strLen(%time);

		%m = " AM";
		%firstColon = strPos(%time,":");
		%hours = getSubStr(%time,0,%firstcolon);
		if(%hours >= 12)
		{
			%hours = %hours - 12;
			if(%hours $= "0")
			   %hours = "12";
			%m = " PM";
			%time = %hours @ getSubStr(%time,%firstColon,%len);
		}

	%date = iMain_getDate();
	iMain_DATETIME.setText("<font:Tahoma bold:16><just:left>" @ %time @ %m @ "<font:Tahoma:16>  " @ %date);
	iMain_AuthText.setText(MM_AuthText.getValue());

	$Blota::TimeLoop = schedule(1000, 0, iMain_timeLoop);
}

function iMain_getDate()
{
	%date = getDateTime();
	%monthNum = getSubStr(%date,0,2);
	switch(%monthNum)
	{
		case 00:
			%month = "lol u broke it";
		case 01:
			%month = "January";
		case 02:
			%month = "Feburary";
		case 03:
			%month = "March";
		case 04:
			%month = "April";
		case 05:
			%month = "May";
		case 06:
			%month = "June";
		case 07:
			%month = "July";
		case 08:
			%month = "August";
		case 09:
			%month = "September";
		case 10:
			%month = "October";
		case 11:
			%month = "November";
		case 12:
			%month = "December";
	}
	%preday = getSubStr(%date, 3, 2);
	if(%preday < 10)
	{
		%day = getSubStr(%date, 4, 1);
	}
	if(%preday >= 10)
	{
		%day = %preday;
	}
	%year = getSubStr(%date, 6, 2);
	%text = %month SPC %day @ "," SPC "20" @ %year;
	return %text;
}
iMain_timeLoop();

package iMainOverride
{
	function GameModeGui::clickBack(%this)
	{
		canvas.PopDialog(%this.getName());
	}

	function MainMenuGUI::showButtons()
	{
		//Nothing
	}
	
	function MainMenuGui::onRender()
	{
	   parent::onRender();
	   if(!$iMain::started)
	   {
         iMain_init();
         ServerBrowser_FetchServerList(0);
         GUIPlus_APIInit();
	   }
         
      $iMain::started = true;
	}
};
activatePackage(iMainOverride);

function iMain_ToggleAuthOpt()
{
	if(iMain_AuthOpt.visible)
	{
		iMain_AuthOpt.setVisible(0);
		return;
	}
	iMain_AuthOpt.setVisible(1);
}

//schedule(4000, 0, iMain_init);