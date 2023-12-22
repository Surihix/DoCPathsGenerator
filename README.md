# DoCPathsGenerator
This a companion tool of sorts that helps in generating paths for files that do not have a virtual path defined in the filelist file from the game Dirge Of Cerberus Final Fantasy VII.

Please refer to this page for the instructions on using this program:
<br>https://github.com/Surihix/DoCPathsGenerator/blob/master/Docs/ToolUsage.md

## Important
- Currently this program will generate the paths only for the text bin files and the class files. more path generation support might get added in future updates when I learn more on how the path generation algorithm works with the other files.
- The file names will be the same as what it was when inside the noPaths folder but will now be arranged nicely in sub folders inside the ``#generatedPaths`` folder. there will also be a extension appended to these file names and this combined with the sub folder arrangement, should give enough information to determine the file type and its contents.

## For Developers
- The path generation algorithm can be found here:
  <br>https://github.com/DoC-Research/Documentation/wiki/%5BResearch%5D-Filecode-and-Path-generation-algorithm
