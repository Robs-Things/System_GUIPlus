new GuiControlProfile(ServerBrowserA13) 
{
   fontType = "Arial Bold";
   fontSize = "16";
   fontColor = "20 20 20 255";
   justify = "center";
   textOffset = "3 0";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   returnTab = "0";
   numbersOnly = "0";
};

new GuiControlProfile(ServerScroll)
{
   hasBitmapArray = true;
   bitmap = "Add-ons/System_GUIPlus/modules/serverBrowser/ui/scrollProfile.png";
};

new GuiControlProfile(SearchTextProfile) {
   fontType = "Arial";
   fontSize = "16";
   fontColor = "20 20 20 255";
   justify = "center";
   textOffset = "3 0";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   returnTab = "0";
   numbersOnly = "0";
   cankeyfocus = "1";
   selectable = "1";
};

new GuiControlProfile(GuiPlusWindowProfile) {
   tab = "0";
   canKeyFocus = "0";
   mouseOverSelected = "0";
   modal = "1";
   opaque = "1";
   fillColor = "255 255 255 255";
   fillColorHL = "200 200 200 255";
   fillColorNA = "200 200 200 255";
   border = "2";
   borderThickness = "1";
   borderColor = "0 0 0 255";
   borderColorHL = "128 128 128 255";
   borderColorNA = "64 64 64 255";
   fontType = "Arial";
   fontSize = "18";
   fontColors[0] = "255 255 255 255";
   fontColors[1] = "255 255 255 255";
   fontColors[2] = "0 0 0 255";
   fontColors[3] = "200 200 200 255";
   fontColors[4] = "0 0 204 255";
   fontColors[5] = "85 26 139 255";
   fontColors[6] = "0 0 0 0";
   fontColors[7] = "0 0 0 0";
   fontColors[8] = "0 0 0 0";
   fontColors[9] = "0 0 0 0";
   fontColor = "255 255 255 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "0 0 0 255";
   fontColorSEL = "200 200 200 255";
   fontColorLink = "0 0 204 255";
   fontColorLinkHL = "85 26 139 255";
   doFontOutline = "0";
   fontOutlineColor = "255 255 255 255";
   justify = "left";
   textOffset = "16 9";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   returnTab = "0";
   numbersOnly = "0";
   cursorColor = "0 0 0 255";
   bitmap = "Add-ons/System_GUIPlus/modules/main/images/window.png";
   hasBitmapArray = "1";
};