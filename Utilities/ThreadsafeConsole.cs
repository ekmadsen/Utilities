﻿using System;
using System.Diagnostics;
using JetBrains.Annotations;


namespace ErikTheCoder.Utilities
{
    // This class writes to the console in a thread-safe manner.
    // TODO: Determine how to provide async methods. See https://stackoverflow.com/questions/22664392/await-console-readline.
    [UsedImplicitly]
    public static class ThreadsafeConsole
    {
        private const string _elapsedSecondsFormat = "000.000";
        private const string _threadIdFormat = "00";
        private static readonly object _lock = new object();


        [UsedImplicitly]
        public static void Write(string Message, ConsoleColor Color = ConsoleColor.White)
        {
            lock (_lock)
            {
                Console.ForegroundColor = Color;
                Console.Write(Message);
                Console.ResetColor();
            }
        }


        [UsedImplicitly]
        public static void WriteLine()
        {
            lock (_lock)
            {
                Console.WriteLine();
            }
        }


        [UsedImplicitly]
        public static void WriteLine(string Message, ConsoleColor Color = ConsoleColor.White, Stopwatch Stopwatch = null, bool IncludeThreadName = false)
        {
            var elapsed = Stopwatch is null
                ? string.Empty
                : $"{Stopwatch.Elapsed.TotalSeconds.ToString(_elapsedSecondsFormat)}  ";
            var threadName = IncludeThreadName
                ? $"Thread{System.Threading.Thread.CurrentThread.ManagedThreadId.ToString(_threadIdFormat)}  "
                : string.Empty;
            lock (_lock)
            {
                Console.ForegroundColor = Color;
                Console.WriteLine($"{elapsed}{threadName}{Message}");
                Console.ResetColor();
            }
        }


        [UsedImplicitly]
        public static int Read()
        {
            lock (_lock)
            {
                return Console.Read();
            }
        }


        [UsedImplicitly]
        public static string ReadLine()
        {
            lock (_lock)
            {
                return Console.ReadLine();
            }
        }
    }
}
