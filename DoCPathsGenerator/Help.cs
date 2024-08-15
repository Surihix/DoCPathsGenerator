using System;

namespace DoCPathsGenerator
{
    internal class Help
    {
        public static void ShowCommands()
        {
            Console.WriteLine("Tool actions:");
            Console.WriteLine("-gc = Generates paths and copies files to new generated directory");
            Console.WriteLine("-gm = Generates paths and moves files to new generated directory");
            Console.WriteLine("-c = Checks how many paths are generated");

            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine("Examples:");
            Console.WriteLine("DoCPathsGenerator.exe -g \"FILELIST.BIN\" \"_KEL.DAT\"");
            Console.WriteLine("DoCPathsGenerator.exe -gm \"FILELIST.BIN\" \"_KEL.DAT\"");
            Console.WriteLine("DoCPathsGenerator.exe -c \"_KEL.DAT\" \"#generatedPaths\"");

            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}