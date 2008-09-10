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

		public static IEnumerable<string> Words (this TextReader self)
		{
			return Words (self, TextReaderRocksOptions.None);
		}

		public static IEnumerable<string> Words (this TextReader self, TextReaderRocksOptions options)
		{
			Check.Self (self);
			CheckOptions (options);

			return CreateWordsIterator (self, options);
		}

		private static IEnumerable<string> CreateWordsIterator (TextReader self, TextReaderRocksOptions options)
		{
			StringBuilder buf = new StringBuilder ();
			try {
				int c;
				bool inWord = false;
				while ((c = self.Read ()) >= 0) {
					if (!char.IsWhiteSpace ((char) c)) {
						inWord = true;
						buf.Append ((char) c);
					}
					else {
						if (inWord) {
							yield return buf.ToString ();
							buf.Length = 0;
						}
						inWord = false;
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
