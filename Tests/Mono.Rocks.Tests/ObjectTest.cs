//
// ObjectTest.cs
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
using System.Linq;

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class ObjectTest : BaseRocksFixture {

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Match_MatchersNull ()
		{
			Func<string, Maybe<int>>[] m = null;
			"foo".Match (m);
		}

		[Test, ExpectedException (typeof (InvalidOperationException))]
		public void Match_NoMatch ()
		{
			var m = new Func<string, Maybe<int>>[] {
				v => Maybe.When (v == "bar", 42),
				v => Maybe.When (v.Length == 5, 54),
			};
			"foo".Match (m);
		}

		[Test]
		public void Match ()
		{
#if BNC_423791
			Assert.AreEqual ("foo",
				"foo".Match (
					s => Maybe.When (s.Length != 3, "bar!"),
					s => Maybe.Create (s)));
			Assert.AreEqual ("bar!",
				5.Match (
					v => Maybe.When (v != 3, "bar!"),
					v => Maybe.Create (v.ToString ())));
#endif
			var m = new Func<string, Maybe<int>>[] {
				v => Maybe.When (v == "bar",    1),
				v => Maybe.When (v.Length == 5, 2),
				v => Maybe.Create (-1),
			};
			Assert.AreEqual (1, "bar".Match (m));
			Assert.AreEqual (2, "12345".Match (m));
			Assert.AreEqual (-1, "*default*".Match (m));
		}

		[Test]
		public void ToMaybe ()
		{
			Maybe<int> r = 42.ToMaybe ();
			Assert.IsTrue (r.HasValue);
			Assert.AreEqual (42, r.Value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void With_SelectorNull ()
		{
			string                s = "foo";
			Func<string, string>  f = null;
			s.With (f);
		}

		[Test]
		public void With ()
		{
			Assert.AreEqual (3,
				new[]{5, 4, 3, 2, 1}.Sort ().With (c => c.ElementAt (c.Count()/2)));
		}
	}
}
