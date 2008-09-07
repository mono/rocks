//
// ConsTest.cs
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
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class ConsTest : BaseRocksFixture {

		[Test]
		public void Append ()
		{
			var list = new Cons<int> (1).Append (2).Append (3).Append (4);
			Assert.AreEqual (4, list.Count());
			Assert.AreEqual ("1,2,3,4", list.Implode (","));
		}

		[Test]
		public void Prepend ()
		{
			var list = new Cons<int> (1).Prepend (2).Prepend (3).Prepend (4);
			Assert.AreEqual (4, list.Count());
			Assert.AreEqual ("4,3,2,1", list.Implode (","));
		}

		[Test, ExpectedException (typeof (ArgumentOutOfRangeException))]
		public void ElementAt_NegativeIndex ()
		{
			Cons<int> c = new Cons<int> (42);
			c.ElementAt (-1);
		}

		[Test]
		public void ElementAt ()
		{
			var a = new Cons<int> (1);
			var b = new Cons<int> (2, a);
			var c = new Cons<int> (3, b);
			var d = new Cons<int> (4, c);

			Assert.AreEqual (4, d.Count());
			Assert.AreEqual (4, d.ElementAt (0));
			Assert.AreEqual (3, d.ElementAt (1));
			Assert.AreEqual (2, d.ElementAt (2));
			Assert.AreEqual (1, d.ElementAt (3));
		}

		[Test]
		public void Reverse ()
		{
			Assert.AreEqual ("4,3,2,1",
					new[]{1, 2, 3, 4}.ToCons ().Reverse ().Implode (","));
		}
	}
}
