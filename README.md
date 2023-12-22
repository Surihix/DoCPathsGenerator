# DoCPathsGenerator
This a companion tool of sorts that helps in generating paths for files that do not have a virtual path defined in the filelist file from the game Dirge Of Cerberus Final Fantasy VII.

Please refer to this page for the instructions on using this program:
<br>https://github.com/Surihix/DoCPathsGenerator/blob/master/Docs/ToolUsage.md

## Important
- Currently this program will generate the paths only for the text bin files and the class files. more path generation support might get added when I discover how the algorithm works for the other files.
- The file names will still have the same name as it was in the noPaths folder but will now be arranged nicely inside sub folders and will have a extension. these folders along with the extension should give enough info on what data the file contains.

## For Developers
- The path generation algorithm can be found here:
  <br>https://github.com/DoC-Research/Documentation/wiki/%5BResearch%5D-Filecode-and-Path-generation-algorithm
