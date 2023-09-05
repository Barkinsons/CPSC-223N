echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile Fahruserinterface.cs to create the file: Fahruserinterface.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -out:Fahruserinterface.dll Fahruserinterface.cs

echo Compile Fahrmain.cs to create the file: Fahrmain.dll
mcs -r:System -r:System.Windows.Forms.dll -r:Fahruserinterface.dll -out:Fahr.exe Fahrmain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 1 program.
./Fahr.exe