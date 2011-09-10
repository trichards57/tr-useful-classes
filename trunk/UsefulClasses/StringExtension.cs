using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UsefulClasses
{
    public static class StringExtension
    {
        
        public static IEnumerable<string> Wrap(this string value, int width)
        {
            var words = value.Explode(new[] { ' ' });

            var curLineLength = 0;
            var builder = new StringBuilder();
            foreach (var w in words)
            {
                string word;
                if (curLineLength + w.Length > width)
                {
                    if (curLineLength > 0)
                    {
                        yield return builder.ToString();
                        builder.Clear();
                        curLineLength = 0;
                    }
                    word = w.TrimStart();
                }
                else
                    word = w;
                

                builder.Append(word);
                curLineLength += word.Length;
            }
            yield return builder.ToString();
        }

        public static IEnumerable<string> Explode(this string value, IEnumerable<char> splitChar)
        {
            var startIndex = 0;

            while (true)
            {
                var index = value.IndexOfAny(splitChar.ToArray(), startIndex);
                if (index == -1)
                {
                    yield return value.Substring(startIndex);
                    yield break;
                }

                var word = value.Substring(startIndex, index - startIndex);
                var nextChar = value.Substring(index, 1)[0];
                if (char.IsWhiteSpace(nextChar))
                {
                    yield return word;
                    yield return nextChar.ToString();
                }
                else
                {
                    yield return word + nextChar;
                }

                startIndex = index + 1;
            }
        }
    }
}
