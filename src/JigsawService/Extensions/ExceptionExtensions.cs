using System;
using System.IO;

namespace JigsawService.Extensions
{
    internal static class ExceptionExtensions
    {
        public static void SaveTo(this Exception ex, string path)
        {
            if (File.Exists(path))
                File.Delete(path);
            using var file = File.AppendText(path);
            var current = ex;
            while (current != null)
            {
                file.WriteLine(current.Message);
                file.WriteLine(current.StackTrace);
                file.WriteLine();

                current = current.InnerException;
            }
        }
    }
}