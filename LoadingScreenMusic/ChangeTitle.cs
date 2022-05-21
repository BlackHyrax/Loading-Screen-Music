using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LoadingScreenMusic
{
    internal class ChangeTitle
    {
        //Thx Stackoverflow <3
        [DllImport("user32.dll", EntryPoint = "SetWindowText")]
        public static extern bool thisTitle(IntPtr hwnd, String lpString);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern System.IntPtr CurrentWindow(String className, String windowName);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int message, int wParam, IntPtr lParam);

        private const int WM_SETICON = 0x80;
        private const int ICON_SMALL = 0;
        private const int ICON_BIG = 1;

        static IntPtr Mirrorsim = CurrentWindow(null, "VRChat");
       

        public static void CurrentMusik(string Song)
        {
            string fileex = Path.GetExtension(Song);
            string Clean = Path.GetFileName(Song).Replace(fileex, "");
            thisTitle(Mirrorsim, $"Loading... Now Playinng: {Clean}");
        }
        public static void ChangeIcon()
        {
            try
            {
                Icon icon = new Icon(Directory.GetCurrentDirectory() + "/LoadingScreenMusic/myIcon.ico");
                SendMessage(Mirrorsim, WM_SETICON, ICON_BIG, icon.Handle);
            } catch(Exception er) {
                MelonLoader.MelonLogger.Error(er);
            }

        }
        public static void SetNormal()
        {
            thisTitle(Mirrorsim, $"VRChat - Mirror and Pedo Simulator");
        }
    }
}
