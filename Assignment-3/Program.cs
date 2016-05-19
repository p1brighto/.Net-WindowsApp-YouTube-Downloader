using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Splash screen
            SplashScreen ssSplashscreen = new SplashScreen();
            ssSplashscreen.ShowDialog();

            Application.Run(new YouTubeDownloader());
        }
    }
}
