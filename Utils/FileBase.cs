using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class FileBase
    {
        public static string FileName { get; } = "PseudoDb.txt";
        public static string FilePath { get; } = Path.Combine(AppContext.BaseDirectory, FileName);
    }
}
