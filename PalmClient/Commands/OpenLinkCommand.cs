using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Security.Policy;

namespace PalmClient.Commands
{
    public class OpenLinkCommand : CommandBase
    {
        public OpenLinkCommand() { }

        public override void Execute(object? parameter) 
        {
            string url = "https://github.com/AarhamH/MVVM-MusicPlayer";

            try
            {
                // For Windows
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Win32Exception)
            {
                try
                {
                    // For macOS
                    Process.Start("open", url);
                }
                catch (Exception)
                {
                    try
                    {
                        // For Linux
                        Process.Start("xdg-open", url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to open link: {ex.Message}");
                    }
                }
            }
        }
    }
}
