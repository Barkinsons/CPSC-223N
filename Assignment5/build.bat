del *.dll
del *.exe

csc -t:library -out:Vector.dll Vector.cs

csc -t:library -r:System.Windows.Forms.dll -r:System.Drawing.dll -r:Vector.dll -out:CatMouseUserInterface.dll CatMouseUserInterface.cs

csc -r:System.Windows.Forms.dll -r:CatMouseUserInterface.dll -out:CatMouse.exe CatMouseMain.cs

CatMouse.exe