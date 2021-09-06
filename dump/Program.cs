using System;
using System.Linq;
using Microsoft.Diagnostics.Runtime;
using DynaMD;

namespace clrmd
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            using var dataTarget = DataTarget.LoadDump("dump.coredump");
            var version = dataTarget.ClrVersions[0];
            Console.WriteLine($"Version: {version}");
            using var runtime = version.CreateRuntime();

            Console.WriteLine($"Clr thread count: {runtime.Threads.Length}");

            var strings = runtime.Heap.EnumerateObjects()
               .Where(x => !x.IsNull && x.Type?.Name == "System.String")
               .Select(x => x.AsString(256));

            Console.WriteLine("Found strings:");

            foreach (var str in strings)
                Console.WriteLine($"\t{str}");
        }
    }
}