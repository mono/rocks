//
// TextReaderTest.cs
//
// Author:
//   Jonathan Pryor  <jpryor@novell.com>
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
using System.IO;
using System.Linq;

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class TextReaderTest : BaseRocksFixture {

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Lines_SelfNull ()
		{
			TextReader s = null;
			s.Lines ();
		}

		[Test]
		public void Lines ()
		{
			string[] lines = 
							new StringReader ("hello\nout\rthere\r\nin\nTV\nland!")
							.Lines ().ToArray ();
			Assert.AreEqual (6, lines.Length);
			Assert.AreEqual ("hello", lines [0]);
			Assert.AreEqual ("out",   lines [1]);
			Assert.AreEqual ("there", lines [2]);
			Assert.AreEqual ("in",    lines [3]);
			Assert.AreEqual ("TV",    lines [4]);
			Assert.AreEqual ("land!", lines [5]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Words_SelfNull ()
		{
			TextReader s = null;
			s.Words ();
		}

		[Test]
		public void Words ()
		{
			string[] words = 
				new StringReader ("   skip  leading\r\n\tand trailing\vwhitespace   ")
				.Words ().ToArray ();
			Assert.AreEqual (5, words.Length);
			Assert.AreEqual ("skip",        words [0]);
			Assert.AreEqual ("leading",     words [1]);
			Assert.AreEqual ("and",         words [2]);
			Assert.AreEqual ("trailing",    words [3]);
			Assert.AreEqual ("whitespace",  words [4]);
		}
	}
}
