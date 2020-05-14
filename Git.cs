﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Br1.GitPack
{
    public class Git
    {
        public static string[] Diff(string filter, string target)
        {

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = $" --no-pager diff --no-renames -z --name-only --diff-filter={filter} {target}";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.WorkingDirectory = System.IO.Directory.GetCurrentDirectory();
            p.StartInfo.StandardOutputEncoding = Encoding.UTF8;
            p.StartInfo.StandardErrorEncoding = Encoding.UTF8;
            p.ErrorDataReceived += P_ErrorDataReceived;
            p.OutputDataReceived += P_OutputDataReceived;

            Console.WriteLine("[git " + p.StartInfo.Arguments + "]");

            p.Start();
            string error = p.StandardError.ReadToEnd();
            if (!String.IsNullOrEmpty(error))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(error);
                Console.ResetColor();
                return null;
            }
            //p.StandardOutput.CurrentEncoding = Encoding.UTF8;
            string saida = p.StandardOutput.ReadToEnd();
            // saida = saida.Replace("\0", "\n");

            return saida.Split(new string[] { "\0" }, StringSplitOptions.RemoveEmptyEntries);
            //  p.WaitForExit();

        }

        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("[git] " + e.Data);
        }

        private static void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[git error] " + e.Data);
            Console.ResetColor();
        }
    }
}
