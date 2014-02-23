// Client_GUIPlus
//    A project to make blockland look better
//    Author: Fluffy 
//    Designs by: SC, TheBlackParrot and Fluffy

// Load Modules
$GUIPlus::Version = 0.13;
exec("./modules/main/main.cs");
exec("./modules/loadingGUI/main.cs");
exec("./modules/serverBrowser/main.cs");
exec("./modules/api/main.cs");

// Load Fonts
if(!isFile("base/client/ui/cache/Open Sans Light_36.gft"))
{
   echo("GUI+: Installing Fonts...");
   copyFont("Open Sans Light_36.gft");
   copyFont("Open Sans_16.gft");
   copyFont("Open Sans_18.gft");
   copyFont("Open Sans_20.gft");
   copyFont("Open Sans_40.gft");
}

function copyFont(%name)
{
   fileCopy("./fonts/" @ %name, "base/client/ui/cache/" @ %name);
}