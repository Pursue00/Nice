using System;
using System.Collections.Generic;
using System.Text;

namespace BitsOfStuff
{
    public static class Commons
    {
    [System.Runtime.InteropServices.DllImport("user32.dll")]
        
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        public static string PrtScPath { get; set; }
    }
}
