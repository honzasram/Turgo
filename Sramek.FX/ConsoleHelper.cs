using System.Runtime.InteropServices;

namespace Sramek.FX
{
    public static class ConsoleHelper
    {
        public static void Open()
        {
            ConsoleNative.AllocConsole();
        }

        public static void Close()
        {
            ConsoleNative.FreeConsole();
        }

        static class ConsoleNative
        {
            [DllImport("kernel32")]
            public static extern bool AllocConsole();
            [DllImport("Kernel32")]
            public static extern void FreeConsole();
        }
    }
}