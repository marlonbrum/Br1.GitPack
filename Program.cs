using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildCreatorGit
{
    class Program
    {
        private static string[] GitDiff(string filter, string source = "master", string dest = "dev")
        {

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = $"diff --no-renames --name-only --diff-filter={filter} {source}..{dest}";
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

            return saida.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static string CreateBuildFolder()
        {

        }

        static void Main(string[] args)
        {
            string[] copiar = GitDiff("ACM");

            Console.WriteLine("Arquivos para copiar: ");
            for (int i = 0; i < copiar.Length; i++)
                Console.WriteLine(copiar[i]);

            Console.WriteLine("");
            Console.WriteLine("Arquivos para excluir: ");

            string[] excluir = GitDiff("D");
            for (int i = 0; i < excluir.Length; i++)
                Console.WriteLine(excluir[i]);


            Console.ReadKey();
        }
    }
}
