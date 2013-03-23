using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public static class SystemExtensions
    {
        public static bool ContainsIgnoreCase(this string str, string substr)
        {
            if (str == null)
                return false;
            return (str.IndexOf(substr, 0, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            return false;
        }
    }
}
