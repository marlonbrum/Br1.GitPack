﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCreatorGit
{
    class Program
    {
        private static string[] GitDiff(string filter, string target)
        {

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = $"diff --no-renames --name-only --diff-filter={filter} {target}";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            p.Start();
            string error = p.StandardError.ReadToEnd();
            if (!String.IsNullOrEmpty(error))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ResetColor();
                return null;
            }

            string saida = p.StandardOutput.ReadToEnd();
            saida = saida.Replace("\r", "");

            return saida.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

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

        private static string CreateBuildFolder(string buildFolder)
        {
            int maxNumber = 0;
            foreach(string dir in Directory.EnumerateDirectories(buildFolder, "b???"))
            {
                int number = -1;
                if (int.TryParse(dir.Substring(dir.Length - 3), out number))
                    maxNumber = Math.Max(maxNumber, number);
            }

           string folderName = Path.Combine(buildFolder, "b" + (maxNumber + 1).ToString("000"));
            Directory.CreateDirectory(folderName);
            return folderName;
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

                if (!Directory.Exists(buildFolder))
                {
                    Console.WriteLine("Cant find folder " + buildFolder);
                    //Console.ReadKey();
                    return;
                }

                Console.WriteLine("Running GitDiff...");
                string[] copiar = GitDiff("ACM", targets);
                string[] excluir = GitDiff("D", targets);

                if (copiar.Length == 0 && excluir.Length == 0)
                    Console.WriteLine("No modifications found");
                else
                {
                    string folder = CreateBuildFolder(buildFolder);
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

                            File.Copy(sArq, sDest);
                        }
                    }

                    if (excluir.Length > 0)
                    {
                        string content = "Excluir os arquivos abaixo: \r\n\r\n";
                        for (int i = 0; i < excluir.Length; i++)
                            content += excluir[i];

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

            //  Console.ReadKey();
        }
    }
}
