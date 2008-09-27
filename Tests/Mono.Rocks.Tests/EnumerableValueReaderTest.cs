//
// EnumerableValueReaderTest.cs
//
// Author:
//   Jonathan Pryor
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

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class EnumerableValueReaderTest : BaseRocksFixture {

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void EnumerableValueReader_ValuesNull ()
		{
			IEnumerable<int> values = null;
			new EnumerableValueReader<int> (values);
		}

		[Test, ExpectedException (typeof (NotSupportedException))]
		public void Read_UnsupportedT ()
		{
			int n;
			new EnumerableValueReader<DateTime> (new[]{DateTime.Now})
				.Read (out n);
		}

		[Test]
		public void Reads ()
		{
			string a, b, c;
			new EnumerableValueReader<int> (new[]{1, 2, 3})
				.Read (out a).Read (out b).Read (out c);
			Assert.AreEqual ("1", a);
			Assert.AreEqual ("2", b);
			Assert.AreEqual ("3", c);
		}
	}
}
