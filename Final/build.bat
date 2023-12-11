del *.dll
del *.exe

csc -t:library -out:Vector.dll Vector.cs

csc -t:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:Vector.dll -out:FinalUserInterface.dll FinalUserInterface.cs

csc -r:System.Windows.Forms.dll -r:FinalUserInterface.dll -out:Final.exe FinalMain.cs

Final.exe