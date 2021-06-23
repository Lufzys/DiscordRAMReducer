using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordRamLimiter
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern bool SetProcessWorkingSetSize(IntPtr proc, int min, int max); // https://pinvoke.net/default.aspx/kernel32/SetProcessWorkingSetSize.html

        public static int GetDiscord()
        {
            int DiscordId = -1;
            long workingSet = 0;
            foreach(Process discord in Process.GetProcessesByName("Discord"))
            {
                if(discord.WorkingSet64 > workingSet)
                {
                    workingSet = discord.WorkingSet64;
                    DiscordId = discord.Id;
                }
            }
            return DiscordId;
        }

        static void Main(string[] args)
        {
            Console.Title = "Discord RAM Limiter";
            Console.Write("Discord RAM Limiter - "); Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("https://github.com/Lufzys");
            while(true)
            {
                if (GetDiscord() != -1)
                {
                    // For this console app
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    // For discord app
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        SetProcessWorkingSetSize(Process.GetProcessById(GetDiscord()).Handle, -1, -1);
                    }

                    Thread.Sleep(15000);
                }
            }
        }
    }
}
