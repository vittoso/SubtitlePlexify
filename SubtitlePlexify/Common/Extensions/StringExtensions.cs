using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubtitlePlexify.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool EndsWithAny(this string myString, IEnumerable<string> anyList)
        {
            if (string.IsNullOrEmpty(myString))
                return false;
            if (anyList == null || !anyList.Any())
                return false;

            foreach (var anyStr in anyList)
            {
                if (myString.EndsWith(anyStr))
                    return true;
            }
            return false;
        }
    }
}
