//PLesiu6
using System;
using IronOcr;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;


/// how to use
///////////////////// place .jpg/.png/.jpeg file with clear numbers in main folder (DegenerationTool)
/// how to use


namespace DegenerationTool
{
    class Program
    {
        static void Main(string[] args)
        {
            //////////////////IMPORTANT///////////////////////////
            bool doYouWantToOpenLinks = true; // change to false to generate text
            //////////////////IMPORTANT///////////////////////////
            
            //Looking for file
            string mainFolder = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();  
            string[] directories = Directory.GetFiles(mainFolder);
            string directory = "";
            //Loop for image file
            for (int i = 0; i < directories.Length; i++)
            {
                if (directories[i].Contains(".png") || directories[i].Contains(".jpg") || directories[i].Contains(".jpeg"))
                {
                    directory = directories[i];
                }
            }
            //Analyzing
            var Result = new IronTesseract().Read(directory);
            //Changing text into numbers
            IronOcr.OcrResult.Word[] sauces = Result.Words;
            List<int> list = new List<int>();
            for (int i = 0; i < sauces.Length; i++)
            {
                int n;
                bool isNumeric = int.TryParse(sauces[i].Text, out n);
                list.Add(n);
            }
            //Generating links
            List<string> links = new List<string>();
            for (int i = 0; i < list.Count; i++)
            {
                string n = $"nhentai.net/g/{list[i]}";
                links.Add(n);
            }
            //Opening links every second to net get timeout of website or generates links to copy
            if (doYouWantToOpenLinks == true)
            {
                for (int i = 0; i < links.Count; i++)
                {
                    OpenUrl($"http://{links[i]}");
                    System.Threading.Thread.Sleep(1000);
                }
            }
            else
            {
                for (int i = 0; i < links.Count; i++)
                {
                    Console.WriteLine($"http://{links[i]}");
                }
            }
        }
        static private void OpenUrl(string url) //not by PLesiu6
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}

