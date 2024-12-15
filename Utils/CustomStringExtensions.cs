using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSharpx;

namespace Utils
{
    public static class CustomStringExtensions
    {
        public static string ToCustomString<T>(this T obj) where T : Enum
        {
            Type type = typeof(T);
            var memberInfos = type.GetMember(obj.ToString());
            var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == type);
            var attr = enumValueMemberInfo?.GetCustomAttributes(typeof(CustomStringAttribute), false).First();
            return attr is not null ? ((CustomStringAttribute) attr).Value : type.ToString();
        }

        public static T FromCustomString<T>(this string obj) where T : Enum => 
            Enum.GetValues(typeof(T)).Cast<T>().First(v => v.ToCustomString() == obj);
    }
}
