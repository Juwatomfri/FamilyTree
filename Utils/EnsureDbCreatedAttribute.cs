using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EnsureDbCreatedAttribute : Attribute
    {
        public EnsureDbCreatedAttribute() {}

        public void Ensure()
        {
            Console.WriteLine(FileBase.FilePath);
            if (!File.Exists(FileBase.FilePath))
            {
                File.WriteAllText(FileBase.FilePath, string.Empty);
            }
        }
    }
}
