new GuiControlProfile(iMain_AuthTextProfile) 
{
   fontType = "Tahoma";
   fontSize = "16";
   fontColors[0] = "255 255 255 255";
   fontColors[1] = "255 255 255 255";
   fontColors[2] = "255 255 255 255";
   fontColors[3] = "255 255 255 255";
   fontColors[4] = "255 255 255 255";
   fontColors[5] = "255 255 255 255";
   fontColors[6] = "0 0 0 0";
   fontColors[7] = "0 0 0 0";
   fontColors[8] = "0 0 0 0";
   fontColors[9] = "0 0 0 0";
   fontColor = "255 255 255 255";
   fontColorHL = "255 255 255 255";
   fontColorNA = "255 0 0 255";
   fontColorSEL = "200 200 200 255";
   fontColorLink = "0 0 204 255";
   fontColorLinkHL = "85 26 139 255";
   doFontOutline = "1";
   fontOutlineColor = "70 70 70 255";
   justify = "center";
   textOffset = "3 0";
   autoSizeWidth = "0";
   autoSizeHeight = "0";
   returnTab = "0";
   numbersOnly = "0";
   cursorColor = "0 0 0 255";
   bitmap = "base/client/ui/BlockWindow";
};

new GuiControlProfile(iMainGlassWindowProfile)
{
   opaque = true;
   border = 0;
   justify = "center";
   fillColor = "255 255 255 12";
   text = "";
   fontColor = "255 255 255 255";
   fontSize = 12;
   fontType = "Verdana";
   bitmap = "~/System_GuiPlus/modules/main/images/misc/glassWindowArray.png";
   hasBitmapArray = true;
};

new GuiControlProfile(iMainBlotaCheckBox)
{
   opaque = false;
   fillColor = "232 232 232";
   border = false;
   borderColor = "0 0 0";
   fontColor = "255 255 255 255";
   fontSize = 14;
   fontType = "Tahoma";
   fontColorHL = "32 100 100";
   fixedExtent = true;
   justify = "left";
   bitmap = "~/System_GuiPlus/modules/main/images/misc/coolCheck.png";
   hasBitmapArray = true;
   doFontOutline = "1";
   fontOutlineColor = "70 70 70 255";
};

new GuiControlProfile(iMainScrollProfile)
{
   hasBitmapArray = true;
   bitmap = "~/System_GuiPlus/modules/main/images/misc/scrollArray.png";
};