//
// MaybeTest.cs
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

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class MaybeTest : BaseRocksFixture {

		[Test]
		public void Create ()
		{
			Maybe<int> n = Maybe.Create (42);
			Assert.IsTrue (n.HasValue);
			Assert.AreEqual (42, n.Value);
		}

		[Test, ExpectedException (typeof (InvalidOperationException))]
		public void Maybe_Nothing ()
		{
			Maybe<int> n = new Maybe<int> ();
			Assert.IsFalse (n.HasValue);
			Assert.AreEqual (n, Maybe<int>.Nothing);
			int x = n.Value;
		}

		[Test]
		public void When ()
		{
			var r = Maybe.When (true, 42);
			Assert.IsTrue (r.HasValue);
			Assert.AreEqual (42, r.Value);

			r = Maybe.When (false, 42);
			Assert.IsFalse (r.HasValue);

			bool invoked = false;
			r = Maybe.When (false, () => {invoked = true; return 42;});
			Assert.IsFalse (invoked);
			Assert.IsFalse (r.HasValue);

			r = Maybe.When (true, () => {invoked = true; return 42;});
			Assert.IsTrue (invoked);
			Assert.IsTrue (r.HasValue);
			Assert.AreEqual (42, r.Value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void When_CreatorNull_True ()
		{
			Func<int> f = null;
			var       r = Maybe.When (true, f);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void When_CreatorNull_False ()
		{
			Func<int> f = null;
			var       r = Maybe.When (false, f);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void SelectMany1_SelectorNull ()
		{
			Func<int, Maybe<string>> f = null;
			42.ToMaybe ().SelectMany (f);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void SelectMany2_SelectorNull ()
		{
			Func<int, Maybe<int>>   s  = null;
			Func<int, int, string>  rs = (x, y) => (x+y).ToString ();
			42.ToMaybe ().SelectMany (s, rs);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void SelectMany2_ResultSelectorNull ()
		{
			Func<int, Maybe<int>>    s  = x => 5.ToMaybe ();
			Func<int, int, string>   rs = null;
			42.ToMaybe ().SelectMany (s, rs);
		}

		[Test]
		public void SelectMany ()
		{
			// 1 argument form
			Assert.AreEqual (2.ToMaybe (),
					1.ToMaybe ().SelectMany (x => (x+1).ToMaybe ()));

			// 2 argument form
			Assert.AreEqual (Maybe<int>.Nothing,
					from x in 5.ToMaybe ()
					from y in Maybe<int>.Nothing
					select x + y);
			Assert.AreEqual (Maybe<int>.Nothing, 
					5.ToMaybe().SelectMany(x => Maybe<int>.Nothing,
						(x, y) => x + y));
			Assert.AreEqual (9.ToMaybe (),
					from x in 5.ToMaybe ()
					from y in 4.ToMaybe ()
					select x + y);
			Assert.AreEqual (9.ToMaybe (),
					5.ToMaybe().SelectMany(x => 4.ToMaybe(),
						(x, y) => x + y));
		}

		[Test]
		public new void Equals ()
		{
			Maybe<int> x = 1.ToMaybe ();
			Assert.IsTrue (x.Equals (x));
			Assert.IsFalse (x.Equals (null));
			Assert.IsFalse (x.Equals (Maybe<int>.Nothing));
			Assert.IsTrue (Maybe<int>.Nothing.Equals (Maybe<int>.Nothing));
			Maybe<int> y = 1.ToMaybe ();
			Assert.IsTrue (x.Equals (y));
			Assert.IsTrue (y.Equals (x));
			Maybe<int> z = 1.ToMaybe ();
			Assert.IsTrue (x.Equals (z));
			Assert.IsTrue (y.Equals (z));
		}

		[Test]
		public new void GetHashCode ()
		{
			Assert.AreEqual (0, Maybe<int>.Nothing.GetHashCode ());
			Assert.AreEqual (1.GetHashCode (),
					1.ToMaybe ().GetHashCode ());
		}

		[Test]
		public void GetValueOrDefault ()
		{
			var r = 42.ToMaybe ();
			Assert.AreEqual (42, r.GetValueOrDefault ());
			Assert.AreEqual (42, r.GetValueOrDefault (16));

			r = Maybe<int>.Nothing;
			Assert.AreEqual (0, r.GetValueOrDefault ());
			Assert.AreEqual (16, r.GetValueOrDefault (16));
		}

		[Test]
		public new void ToString ()
		{
			Assert.AreEqual (string.Empty, Maybe<int>.Nothing.ToString ());
			Assert.AreEqual ("42", 42.ToMaybe ().ToString ());
		}
	}
}