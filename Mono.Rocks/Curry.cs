//
// Curry.cs: C# Action and Func Currying Helpers
// 
// GENERATED CODE: DO NOT EDIT.
//
// To regenerate this code, execute: ./mkcurry -n 4 -o Curry.cs
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
using System.Linq.Expressions;

namespace Mono.Rocks {

	public static class CurryRocks  {

		public static Action
			Curry<T> (this Action<T> self, T value)
		{
			Check.Self (self);

			return () => self (value);
		}

		public static Action
			Curry<T> (this Action<T> self, Tuple<T> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1);
		}

		public static Func<TResult>
			Curry<T, TResult> (this Func<T, TResult> self, T value)
		{
			Check.Self (self);

			return () => self (value);
		}

		public static Func<TResult>
			Curry<T, TResult> (this Func<T, TResult> self, Tuple<T> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1);
		}

		public static Action
			Curry<T1, T2> (this Action<T1, T2> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return () => self (value1, value2);
		}

		public static Action
			Curry<T1, T2> (this Action<T1, T2> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1, values._2);
		}

		public static Func<TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return () => self (value1, value2);
		}

		public static Func<TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1, values._2);
		}

		public static Action<T2>
			Curry<T1, T2> (this Action<T1, T2> self, T1 value1)
		{
			Check.Self (self);

			return (value2) => self (value1, value2);
		}

		public static Action<T2>
			Curry<T1, T2> (this Action<T1, T2> self, Tuple<T1> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value2) => self (values._1, value2);
		}

		public static Func<T2, TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, T1 value1)
		{
			Check.Self (self);

			return (value2) => self (value1, value2);
		}

		public static Func<T2, TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, Tuple<T1> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value2) => self (values._1, value2);
		}

		public static Action
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return () => self (value1, value2, value3);
		}

		public static Action
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1, values._2, values._3);
		}

		public static Func<TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return () => self (value1, value2, value3);
		}

		public static Func<TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1, values._2, values._3);
		}

		public static Action<T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3) => self (value1, value2, value3);
		}

		public static Action<T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value3) => self (values._1, values._2, value3);
		}

		public static Func<T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3) => self (value1, value2, value3);
		}

		public static Func<T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value3) => self (values._1, values._2, value3);
		}

		public static Action<T2, T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3) => self (value1, value2, value3);
		}

		public static Action<T2, T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, Tuple<T1> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value2, value3) => self (values._1, value2, value3);
		}

		public static Func<T2, T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3) => self (value1, value2, value3);
		}

		public static Func<T2, T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, Tuple<T1> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value2, value3) => self (values._1, value2, value3);
		}

		public static Action
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1, T2 value2, T3 value3, T4 value4)
		{
			Check.Self (self);

			return () => self (value1, value2, value3, value4);
		}

		public static Action
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1, T2, T3, T4> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1, values._2, values._3, values._4);
		}

		public static Func<TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1, T2 value2, T3 value3, T4 value4)
		{
			Check.Self (self);

			return () => self (value1, value2, value3, value4);
		}

		public static Func<TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1, T2, T3, T4> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return () => self (values._1, values._2, values._3, values._4);
		}

		public static Action<T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return (value4) => self (value1, value2, value3, value4);
		}

		public static Action<T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value4) => self (values._1, values._2, values._3, value4);
		}

		public static Func<T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return (value4) => self (value1, value2, value3, value4);
		}

		public static Func<T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value4) => self (values._1, values._2, values._3, value4);
		}

		public static Action<T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3, value4) => self (value1, value2, value3, value4);
		}

		public static Action<T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value3, value4) => self (values._1, values._2, value3, value4);
		}

		public static Func<T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3, value4) => self (value1, value2, value3, value4);
		}

		public static Func<T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value3, value4) => self (values._1, values._2, value3, value4);
		}

		public static Action<T2, T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3, value4) => self (value1, value2, value3, value4);
		}

		public static Action<T2, T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value2, value3, value4) => self (values._1, value2, value3, value4);
		}

		public static Func<T2, T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3, value4) => self (value1, value2, value3, value4);
		}

		public static Func<T2, T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1> values)
		{
			Check.Self (self);
			if (values == null)
				throw new ArgumentNullException ("values");

			return (value2, value3, value4) => self (values._1, value2, value3, value4);
		}
	}
}
