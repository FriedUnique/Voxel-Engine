using System;

namespace GameEngine.Core {
    public static class Debug {

        public static DebugLevel level = DebugLevel.Log;

        public enum DebugLevel {
            Info = 0,
            Log = 1,
            Warning = 2,
            Error = 3
        }

        public static void Info(string message) {
            if ((int)level > (int)DebugLevel.Info) return;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"[Info] {message}");
            Console.ResetColor();
        }

        public static void Log(string message) {
            if ((int)level > (int)DebugLevel.Log) return;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"[Log] {message}");
            Console.ResetColor();
        }

        public static void Warning(string message) {
            if ((int)level > (int)DebugLevel.Warning) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[Warning] {message}");
            Console.ResetColor();
        }

        public static void Error(string message) {
            if ((int)level > (int)DebugLevel.Error) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] {message}");
            Console.ResetColor();
        }
    }
}
