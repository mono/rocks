//
// TextReader.cs
//
// Author:
//   Jonathan Pryor <jpryor@novell.com>
//
// Copyright (c) 2008 Novell, Inc. (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Mono.Rocks {

	[Flags]
	public enum TextReaderRocksOptions {
		None          = 0,
		CloseReader   = 1,
	}

	public static class TextReaderRocks {

		public static IEnumerable<string> Lines (this TextReader self)
		{
			return Lines (self, TextReaderRocksOptions.None);
		}

		public static IEnumerable<string> Lines (this TextReader self, TextReaderRocksOptions options)
		{
			Check.Self (self);
			CheckOptions (options);

			return CreateLineIterator (self, options);
		}

		private static void CheckOptions (TextReaderRocksOptions options)
		{
			if (options != TextReaderRocksOptions.None && options != TextReaderRocksOptions.CloseReader)
				throw new ArgumentException ("options", "Invalid `options' value.");
		}

		private static IEnumerable<string> CreateLineIterator (TextReader self, TextReaderRocksOptions options)
		{
			try {
				string line;
				while ((line = self.ReadLine ()) != null)
					yield return line;
			} finally {
				if ((options & TextReaderRocksOptions.CloseReader) != 0) {
					self.Close ();
				}
			}
		}

		public static IEnumerable<string> Words (this TextReader self, params Func<char, bool>[] levels)
		{
			Check.Levels (levels);
			if (levels.Length == 0)
				levels = GetDefaultLevels ();
			return Words (self, TextReaderRocksOptions.None, levels);
		}

		private static Func<char, bool>[] GetDefaultLevels ()
		{
			return new Func<char, bool>[]{ c => !char.IsWhiteSpace (c) };
		}

		public static IEnumerable<string> Words (this TextReader self, TextReaderRocksOptions options, params Func<char, bool>[] levels)
		{
			Check.Self (self);
			CheckOptions (options);
			Check.Levels (levels);
			if (levels.Length == 0)
				levels = GetDefaultLevels ();

			return CreateWordsIterator (self, options, levels);
		}

		private static IEnumerable<string> CreateWordsIterator (TextReader self, TextReaderRocksOptions options, Func<char, bool>[] levels)
		{
			StringBuilder buf = new StringBuilder ();
			try {
				int c;
				int level = -1;
				while ((c = self.Read ()) >= 0) {
					char ch = (char) c;
					int next_level = levels
						.Select ((l, i) => l (ch) ? i : -1)
						.Where (n => n >= 0)
						.With (e => e.Count() > 0 ? e.Min () : -1);
					if (next_level == level && level >= 0)
						buf.Append ((char) c);
					else if (next_level >= 0) {
						if (buf.Length > 0) {
							yield return buf.ToString ();
							buf.Length = 0;
						}
						level = next_level;
						buf.Append (ch);
					}
					else {
						if (buf.Length > 0) {
							yield return buf.ToString ();
							buf.Length = 0;
						}
						level = -1;
					}
				}
				if (buf.Length > 0)
					yield return buf.ToString ();
			} finally {
				if ((options & TextReaderRocksOptions.CloseReader) != 0) {
					self.Close ();
				}
			}
		}
	}
}
