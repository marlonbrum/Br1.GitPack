using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br1.GitPack
{
    class Program
    {

        private static void PrintHelp()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("     GitPack pointA..pointB buildFolder");
            Console.WriteLine("         Generate a folder with changes from pointA in code to pointB");
            Console.WriteLine("");
            Console.WriteLine("     GitPack pointA buildFolder");
            Console.WriteLine("         Generate a folder with changes from pointA in code to last version");
            Console.WriteLine("");
            Console.WriteLine("     pointA e pointB could be a commit, branch or tag");
        }

        private static string CreateBuildFolder(string buildFolder, bool keepFiles)
        {
            if (!keepFiles && Directory.Exists(buildFolder))
                Directory.Delete(buildFolder, true);                       
           
            Directory.CreateDirectory(buildFolder);
            return buildFolder;
        }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    PrintHelp();
                    //Console.ReadKey();
                    return;
                }

                // First parameter: targets
                string targets = args[0];

                // Second parameter: Build folder
                string buildFolder = args[1];

                bool keepFiles = false;
                // Option: Keep files
                foreach (string arg in args)
                    if (arg == "-keep")
                        keepFiles = true;

                /*
                if (!Directory.Exists(buildFolder))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Cant find folder " + buildFolder);
                    Console.ResetColor();
                    //Console.ReadKey();
                    return;
                }*/

                Console.WriteLine("Running GitDiff...");
                string[] copiar = Git.Diff("ACM", targets);
                string[] excluir = Git.Diff("D", targets);

                if (copiar.Length == 0 && excluir.Length == 0)
                    Console.WriteLine("No modifications found");
                else
                {
                    string folder = CreateBuildFolder(buildFolder, keepFiles);
                    Console.WriteLine("Creating build in folder " + folder);

                    if (copiar.Length > 0)
                    {
                        if (copiar.Length == 1)
                            Console.WriteLine("1 file to copy");
                        else
                            Console.WriteLine(copiar.Length + " files to copy");

                        Console.WriteLine("Copying files: ");
                        for (int i = 0; i < copiar.Length; i++)
                        {
                            string sArq = copiar[i].Replace("/", "\\");
                            Console.WriteLine(sArq);
                            string sDest = Path.Combine(folder, sArq);

                            Directory.CreateDirectory(Path.GetDirectoryName(sDest));

                            if (Directory.Exists(sArq))
                                Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(sArq, sDest);
                            else
                                File.Copy(sArq, sDest);
                        }
                    }

                    if (excluir.Length > 0)
                    {
                        string content = "Excluir os arquivos abaixo: \r\n\r\n";
                        for (int i = 0; i < excluir.Length; i++)
                            content += excluir[i];

                        File.WriteAllText(Path.Combine(folder, "deleteList.txt"), content);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("There are some files that need to be deleted. Check deleteList.txt");
                        Console.ResetColor();
                    }

                }
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERRO]");
                Console.WriteLine(ex.GetType().FullName);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ResetColor();
            }
            finally
            {
                if (args.Contains("-wait"))
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
            }
        }
    }
}
