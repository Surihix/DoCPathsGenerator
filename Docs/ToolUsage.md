# Instructions
First you will have to unpack the FILELIST.BIN and the KEL.DAT files with my [WhiteBinTools](https://github.com/Surihix/WhiteBinTools) program. 

Do the following after you have downloaded the WhiteBinTools program:
- Copy the FILELIST.BIN and KEL.DAT files from your Dirge of Cerberus game's iso image.
- Extract the downloaded WhiteBinTools_v1.9.7z file into the same folder where you had copied the above two files.
- Open a command prompt in this folder and type these following arguments:
  <br>`` WhiteBinTools.exe -ff131 -u "FILELIST.BIN" "KEL.DAT" ``

- Once the unpacking has finished, type these following arguments in the command prompt:
  <br>`` WhiteBinTools.exe -ff131 -ufl "FILELIST.BIN" ``
  
- The above argument should unpack the contents of the filelist file into a _FILELIST.BIN folder. you should now be ready to run this DoCPathGenerator program.


<br>Download and extract the DoCPathGenerator program in the same folder where you have the _FIELIST.BIN and _KEL.DAT unpacked folders. 

Run the program via command prompt with these following arguments:
<br>`` DoCPathsGenerator.exe "_FILELIST.BIN" "_KEL.DAT" ``

This should generate the paths for some of the files by copying them all inside a ``#generatedPaths`` folder which will be created next to the unpacked _FILELIST.BIN folder.
