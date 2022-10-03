using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public static class UtilityFunctions
    {
        public static string[] Split(string wordList)
        {
            var spaces = new char[] { ' ', '\t', '\r', '\n' };
            var inputs = new string(wordList.ToCharArray()
                .Where(c => !char.IsPunctuation(c)).ToArray()).Split(spaces, StringSplitOptions.RemoveEmptyEntries);
            return inputs;
        }
    }
}
