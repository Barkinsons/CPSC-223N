del *.dll
del *.exe

csc -t:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:linetrackui.dll linetrackui.cs

csc -r:System.Windows.Forms.dll -r:linetrackui.dll -out:linetrack.exe linetrackmain.cs

linetrack.exe