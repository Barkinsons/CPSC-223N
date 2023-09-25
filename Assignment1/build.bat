del *.dll
del *.exe

csc -t:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:Fahruserinterface.dll Fahruserinterface.cs

csc -r:System.Windows.Forms.dll -r:Fahruserinterface.dll -out:Fahr.exe Fahrmain.cs

Fahr.exe
