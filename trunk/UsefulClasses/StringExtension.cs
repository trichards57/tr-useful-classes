//***********************************************************************
// Assembly         : UsefulClasses
// Author           : Tony Richards
// Created          : 08-29-2011
//
// Last Modified By : Tony Richards
// Last Modified On : 09-10-2011
// Description      : 
//
// Copyright (c) 2011, Tony Richards
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// Redistributions of source code must retain the above copyright notice, this list
// of conditions and the following disclaimer.
//
// Redistributions in binary form must reproduce the above copyright notice, this
// list of conditions and the following disclaimer in the documentation and/or other
// materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
// IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
// INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING,
// BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE
// OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
// OF THE POSSIBILITY OF SUCH DAMAGE.
//***********************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace UsefulClasses
{
    /// <summary>
    /// Extension methods for the <see cref="String"/> class.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Wraps the specified string in to a set of lines of specified length.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="width">The width of the line.</param>
        /// <returns>An <see cref="IEnumerable"/> containing the lines the string was wrapped on to.</returns>
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

        /// <summary>
        /// Explodes the specified string.
        /// </summary>
        /// <param name="value">The string to explode.</param>
        /// <param name="splitCharacters">The characters to split the string on.</param>
        /// <returns>An <see cref="IEnumerable"/> containing the parts of the string.</returns>
        public static IEnumerable<string> Explode(this string value, IEnumerable<char> splitCharacters)
        {
            var startIndex = 0;

            while (true)
            {
                var index = value.IndexOfAny(splitCharacters.ToArray(), startIndex);
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
                    yield return nextChar.ToString(CultureInfo.InvariantCulture);
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
