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

	/// <summary>
	///   Provides extension methods on <see cref="T:System.Action{T}"/>,
	///   <see cref="T:System.Func{T,TResult}"/>, and related delegates to 
	///   curry delegate arguments.
	/// </summary>
	/// <remarks>
	///   <para>
	///    Currying is a way to easily transform functions which accept N 
	///    arguments into functions which accept N-1 arguments, by "fixing"
	///    arguments with a value.
	///   </para>
	///   <para>
	///    Since C# functions aren't first class, but 
	///    <see cref="T:System.Delegate"/>s are, we can provide curring on
	///    delegate types instead on functions, which provides something quite
	///    similar.  The <see cref="M:Mono.Rocks.CurryRocks.Curry"/> methods 
	///    allow "fixing" one or more delegate parameters with values, 
	///    returning a new delegate which, when invoke, will pass the fixed 
	///    parameters to the original delegate.
	///   </para>
	///   <code lang="C#">
	///   Func&lt;int,int,int,int&gt; function = Lambda.F ((int a, int b, int c) => a + b + c);
	///   Func&lt;int,int,int&gt;     f_3      = function.Curry (3);
	///   Func&lt;int&gt;             f_321    = function.Curry (3, 2, 1);
	///   Console.WriteLine (f_3 (2, 1));  // prints (3 + 2 + 1) == "6"
	///   Console.WriteLine (f_321 ());    // prints (3 + 2 + 1) == "6"</code>
	///   <para>
	///    All possible argument and return delegate permutations are provided
	///    for the <see cref="T:System.Action{T}"/>, 
	///    <see cref="T:System.Func{T,TResult}"/>, and related types.
	///   </para>
	/// </remarks>
	public static class CurryRocks  {

		/// <typeparam name="T">
		///   A <see cref="T:System.Action{T}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T}"/> to curry.
		/// </param>
		/// <param name="value">
		///   A value of type <typeparamref name="T"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T> (this Action<T> self, T value)
		{
			Check.Self (self);

			return () => self (value);
		}

		/// <typeparam name="T">
		///   A <see cref="T:System.Action{T}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T> (this Action<T> self, Tuple<T> values)
		{
			Check.Self (self);
			return () => self (values._1);
		}

		/// <typeparam name="T">
		///   A <see cref="T:System.Func{T,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T,TResult}"/> to curry.
		/// </param>
		/// <param name="value">
		///   A value of type <typeparamref name="T"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T, TResult> (this Func<T, TResult> self, T value)
		{
			Check.Self (self);

			return () => self (value);
		}

		/// <typeparam name="T">
		///   A <see cref="T:System.Func{T,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T, TResult> (this Func<T, TResult> self, Tuple<T> values)
		{
			Check.Self (self);
			return () => self (values._1);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T1, T2> (this Action<T1, T2> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return () => self (value1, value2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T1, T2> (this Action<T1, T2> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			return () => self (values._1, values._2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return () => self (value1, value2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			return () => self (values._1, values._2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T2}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T2}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T2>
			Curry<T1, T2> (this Action<T1, T2> self, T1 value1)
		{
			Check.Self (self);

			return (value2) => self (value1, value2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T2}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T2}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T2>
			Curry<T1, T2> (this Action<T1, T2> self, Tuple<T1> values)
		{
			Check.Self (self);
			return (value2) => self (values._1, value2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T2, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T2, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T2, TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, T1 value1)
		{
			Check.Self (self);

			return (value2) => self (value1, value2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T2, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T2, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T2, TResult>
			Curry<T1, T2, TResult> (this Func<T1, T2, TResult> self, Tuple<T1> values)
		{
			Check.Self (self);
			return (value2) => self (values._1, value2);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <param name="value3">
		///   A value of type <typeparamref name="T3"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return () => self (value1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2, T3}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			return () => self (values._1, values._2, values._3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <param name="value3">
		///   A value of type <typeparamref name="T3"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return () => self (value1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2, T3}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			return () => self (values._1, values._2, values._3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T3}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T3}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3) => self (value1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T3}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T3}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			return (value3) => self (values._1, values._2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T3, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T3, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3) => self (value1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T3, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T3, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			return (value3) => self (values._1, values._2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T2, T3}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T2, T3}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T2, T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3) => self (value1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T2, T3}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T2, T3}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T2, T3>
			Curry<T1, T2, T3> (this Action<T1, T2, T3> self, Tuple<T1> values)
		{
			Check.Self (self);
			return (value2, value3) => self (values._1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T2, T3, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T2, T3, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T2, T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3) => self (value1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T2, T3, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T2, T3, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T2, T3, TResult>
			Curry<T1, T2, T3, TResult> (this Func<T1, T2, T3, TResult> self, Tuple<T1> values)
		{
			Check.Self (self);
			return (value2, value3) => self (values._1, value2, value3);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <param name="value3">
		///   A value of type <typeparamref name="T3"/> to fix.
		/// </param>
		/// <param name="value4">
		///   A value of type <typeparamref name="T4"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1, T2 value2, T3 value3, T4 value4)
		{
			Check.Self (self);

			return () => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2, T3, T4}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1, T2, T3, T4> values)
		{
			Check.Self (self);
			return () => self (values._1, values._2, values._3, values._4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <param name="value3">
		///   A value of type <typeparamref name="T3"/> to fix.
		/// </param>
		/// <param name="value4">
		///   A value of type <typeparamref name="T4"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1, T2 value2, T3 value3, T4 value4)
		{
			Check.Self (self);

			return () => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2, T3, T4}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1, T2, T3, T4> values)
		{
			Check.Self (self);
			return () => self (values._1, values._2, values._3, values._4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <param name="value3">
		///   A value of type <typeparamref name="T3"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T4}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T4}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return (value4) => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2, T3}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T4}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T4}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			return (value4) => self (values._1, values._2, values._3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <param name="value3">
		///   A value of type <typeparamref name="T3"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T4, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T4, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1, T2 value2, T3 value3)
		{
			Check.Self (self);

			return (value4) => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2, T3}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T4, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T4, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1, T2, T3> values)
		{
			Check.Self (self);
			return (value4) => self (values._1, values._2, values._3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T3, T4}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T3, T4}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3, value4) => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T3, T4}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T3, T4}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			return (value3, value4) => self (values._1, values._2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <param name="value2">
		///   A value of type <typeparamref name="T2"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T3, T4, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T3, T4, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1, T2 value2)
		{
			Check.Self (self);

			return (value3, value4) => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1, T2}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T3, T4, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T3, T4, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1, T2> values)
		{
			Check.Self (self);
			return (value3, value4) => self (values._1, values._2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T2, T3, T4}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T2, T3, T4}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T2, T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3, value4) => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Action{T1,T2,T3,T4}"/> parameter type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Action{T1,T2,T3,T4}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Action{T2, T3, T4}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Action{T2, T3, T4}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Action<T2, T3, T4>
			Curry<T1, T2, T3, T4> (this Action<T1, T2, T3, T4> self, Tuple<T1> values)
		{
			Check.Self (self);
			return (value2, value3, value4) => self (values._1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="value1">
		///   A value of type <typeparamref name="T1"/> to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T2, T3, T4, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T2, T3, T4, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T2, T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, T1 value1)
		{
			Check.Self (self);

			return (value2, value3, value4) => self (value1, value2, value3, value4);
		}

		/// <typeparam name="T1">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T2">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T3">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="T4">
		///   A <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> parameter type.
		/// </typeparam>
		/// <typeparam name="TResult">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> return type.
		/// </typeparam>
		/// <param name="self">
		///   The <see cref="T:System.Func{T1,T2,T3,T4,TResult}"/> to curry.
		/// </param>
		/// <param name="values">
		///   A value of type <see cref="T:Mono.Rocks.Tuple{T1}"/> which contains the values to fix.
		/// </param>
		/// <summary>
		///   Creates a <see cref="T:System.Func{T2, T3, T4, TResult}"/> delegate.
		/// </summary>
		/// <returns>
		///   Returns a <see cref="T:System.Func{T2, T3, T4, TResult}"/> which, when invoked, will
		///   invoke <paramref name="self"/> along with the provided fixed parameters.
		/// </returns>
		public static Func<T2, T3, T4, TResult>
			Curry<T1, T2, T3, T4, TResult> (this Func<T1, T2, T3, T4, TResult> self, Tuple<T1> values)
		{
			Check.Self (self);
			return (value2, value3, value4) => self (values._1, value2, value3, value4);
		}
	}
}
