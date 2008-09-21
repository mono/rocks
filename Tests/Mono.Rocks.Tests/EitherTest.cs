//
// EitherTest.cs
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
	public class EitherTest : BaseRocksFixture {

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either2_A_ValueNull ()
		{
			Either<Action, object>.A (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either2_B_ValueNull ()
		{
			Either<Action, object>.B (null);
		}

		[Test]
		public void Either2_Equals ()
		{
			Either<Action, object> e = Either<Action, object>.A (() => {});
			Assert.IsFalse (e.Equals ((object) null));
			Assert.IsFalse (e.Equals ((Either<Action, object>) null));
			Assert.IsFalse (e.Equals (Either<Action, object>.B (new object())));
			Assert.IsTrue (e.Equals (e));
			Assert.IsTrue (Either<Action, object>.B ("foo").Equals (
					Either<Action, object>.B ("foo")));
		}

		[Test]
		public void Either2_Fold_a_Null ()
		{
			Func<Action, int> a = null;
			Func<object, int> b = x => Convert.ToInt32(x);

			var e = Either<Action, object>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b));

			e = Either<Action, object>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b));
		}

		[Test]
		public void Either2_Fold_b_Null ()
		{
			Func<Action, int> a = x => 42;
			Func<object, int> b = null;

			var e = Either<Action, object>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b));

			e = Either<Action, object>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b));
		}

		[Test]
		public void Either2_Fold ()
		{
			Action a = () => {};
			Either<Action, object> e = Either<Action, object>.A (a);
			Assert.AreEqual (a, e.Fold (v => v, v => null));

			e = Either<Action, object>.B ("foo");
			Assert.AreEqual ("foo", e.Fold (v => null, v => v.ToString ()));
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either3_A_ValueNull ()
		{
			Either<Action, object, string>.A (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either3_B_ValueNull ()
		{
			Either<Action, object, string>.B (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either3_C_ValueNull ()
		{
			Either<Action, object, string>.C (null);
		}

		[Test]
		public void Either3_Equals ()
		{
			Either<Action, object, string> e = Either<Action, object, string>.A (() => {});
			Assert.IsFalse (e.Equals ((object) null));
			Assert.IsFalse (e.Equals ((Either<Action, object, string>) null));
			Assert.IsFalse (e.Equals (Either<Action, object, string>.C ("foo")));
			Assert.IsTrue (e.Equals (e));
			Assert.IsTrue (Either<Action, object, string>.C ("foo").Equals (
					Either<Action, object, string>.C ("foo")));
		}

		[Test]
		public void Either3_Fold_a_Null ()
		{
			Func<Action, int> a = null;
			Func<object, int> b = x => Convert.ToInt32(x);
			Func<string, int> c = x => Convert.ToInt32(x);

			var e = Either<Action, object, string>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));

			e = Either<Action, object, string>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));

			e = Either<Action, object, string>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));
		}

		[Test]
		public void Either3_Fold_b_Null ()
		{
			Func<Action, int> a = x => 42;
			Func<object, int> b = null;
			Func<string, int> c = x => Convert.ToInt32(x);

			var e = Either<Action, object, string>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));

			e = Either<Action, object, string>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));

			e = Either<Action, object, string>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));
		}

		[Test]
		public void Either3_Fold_c_Null ()
		{
			Func<Action, int> a = x => 42;
			Func<object, int> b = x => Convert.ToInt32(x);
			Func<string, int> c = null;

			var e = Either<Action, object, string>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));

			e = Either<Action, object, string>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));

			e = Either<Action, object, string>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c));
		}

		[Test]
		public void Either3_Fold ()
		{
			Action a = () => {};
			Either<Action, object, string> e = Either<Action, object, string>.A (a);
			Assert.AreEqual (a, e.Fold (v => v, v => null, v => null));

			e = Either<Action, object, string>.C ("foo");
			Assert.AreEqual ("foo", e.Fold (v => null, v => null, v => v));
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either4_A_ValueNull ()
		{
			Either<Action, object, string, Type>.A (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either4_B_ValueNull ()
		{
			Either<Action, object, string, Type>.B (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either4_C_ValueNull ()
		{
			Either<Action, object, string, Type>.C (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Either4_D_ValueNull ()
		{
			Either<Action, object, string, Type>.D (null);
		}

		[Test]
		public void Either4_Equals ()
		{
			Either<Action, object, string, Type> e = Either<Action, object, string, Type>.D (typeof (object));
			Assert.IsFalse (e.Equals ((object) null));
			Assert.IsFalse (e.Equals ((Either<Action, object, string, Type>) null));
			Assert.IsFalse (e.Equals (Either<Action, object, string, Type>.C ("foo")));
			Assert.IsTrue (e.Equals (e));
			Assert.IsTrue (Either<Action, object, string, Type>.C ("foo").Equals (
					Either<Action, object, string, Type>.C ("foo")));
		}

		[Test]
		public void Either4_Fold_a_Null ()
		{
			Func<Action, int> a = null;
			Func<object, int> b = x => Convert.ToInt32(x);
			Func<string, int> c = x => Convert.ToInt32(x);
			Func<Type,   int> d = x => 42;

			var e = Either<Action, object, string, Type>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.D (typeof (object));
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));
		}

		[Test]
		public void Either4_Fold_b_Null ()
		{
			Func<Action, int> a = x => 42;
			Func<object, int> b = null;
			Func<string, int> c = x => Convert.ToInt32(x);
			Func<Type,   int> d = x => 42;

			var e = Either<Action, object, string, Type>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.D (typeof (object));
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));
		}

		[Test]
		public void Either4_Fold_c_Null ()
		{
			Func<Action, int> a = x => 42;
			Func<object, int> b = x => Convert.ToInt32(x);
			Func<string, int> c = null;
			Func<Type,   int> d = x => 42;

			var e = Either<Action, object, string, Type>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.D (typeof (object));
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));
		}

		[Test]
		public void Either4_Fold_d_Null ()
		{
			Func<Action, int> a = x => 42;
			Func<object, int> b = x => Convert.ToInt32(x);
			Func<string, int> c = x => Convert.ToInt32(x);
			Func<Type,   int> d = null;

			var e = Either<Action, object, string, Type>.A (() => {});
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.B (new object());
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.C ("foo");
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));

			e = Either<Action, object, string, Type>.D (typeof (object));
			AssertException<ArgumentNullException> (() => e.Fold (a, b, c, d));
		}

		[Test]
		public void Either4_Fold ()
		{
			Action a = () => {};
			Either<Action, object, string, Type> e = Either<Action, object, string, Type>.A (a);
			Assert.AreEqual (a, e.Fold (v => v, v => null, v => null, v => null));

			e = Either<Action, object, string, Type>.D (typeof (object));
			Assert.AreEqual (typeof (object), e.Fold (v => null, v => null, v => null, v => v));
		}
	}
}
