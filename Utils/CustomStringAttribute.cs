using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomStringAttribute : Attribute
    {
        public string Value { get; set; }
        public CustomStringAttribute(string value) 
        { 
            Value = value;
        }
    }
}
