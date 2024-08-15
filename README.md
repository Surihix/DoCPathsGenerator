# DoCPathsGenerator
This a companion tool that helps generating paths for files that do not have a path defined in the filelist file, present in Dirge Of Cerberus Final Fantasy VII.

Please refer to this page for the instructions on using this program:
<br>https://github.com/Surihix/DoCPathsGenerator/blob/master/Docs/ToolUsage.md

## Important
- Currently this program will generate the paths for all the noPath files that are loaded by the game, whenever it loads ``data/zone`` and ``data/event`` folders. path generation support for any remaining noPath files, will be added in a future update.
- The mapping info for each noPath file and its generated path, will be stored in a JSON file, which will be generated inside the `#generatedPaths` folder.

## For Developers
- The path generation algorithm can be found here:
  <br>https://github.com/DoC-Research/Documentation/wiki/%5BResearch%5D-Filecode-and-Path-generation-logic
