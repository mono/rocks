//
// Lambda.cs: C# Lambda Expression Helpers.
//
// GENERATED CODE: DO NOT EDIT.
// 
// To regenerate this code, execute: ./mklambda -n 4 -o Lambda.cs
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
// Code License: Public Domain.
//
using System;
using System.Linq.Expressions;

namespace Mono.Rocks {

	public static class Lambda {

		public static Action Func (Action action)
		{
			return action;
		}

		public static Func<TResult> Func<TResult> (Func<TResult> func)
		{
			return func;
		}

		public static Expression<Action> Expression (Expression<Action> expr)
		{
			return expr;
		}

		public static Expression<Func<TResult>> Expression<TResult> (Expression<Func<TResult>> expr)
		{
			return expr;
		}

		public static Action<T>
			Func<T> (Action<T> action)
		{
			return action;
		}

		public static Func<T, TResult>
			Func<T, TResult> (Func<T, TResult> func)
		{
			return func;
		}

		public static Expression<Action<T>>
			Expression<T> (Expression<Action<T>> expr)
		{
			return expr;
		}

		public static Expression<Func<T, TResult>>
			Expression<T, TResult> (Expression<Func<T, TResult>> expr)
		{
			return expr;
		}

		public static Action<T1, T2>
			Func<T1, T2> (Action<T1, T2> action)
		{
			return action;
		}

		public static Func<T1, T2, TResult>
			Func<T1, T2, TResult> (Func<T1, T2, TResult> func)
		{
			return func;
		}

		public static Expression<Action<T1, T2>>
			Expression<T1, T2> (Expression<Action<T1, T2>> expr)
		{
			return expr;
		}

		public static Expression<Func<T1, T2, TResult>>
			Expression<T1, T2, TResult> (Expression<Func<T1, T2, TResult>> expr)
		{
			return expr;
		}

		public static Action<T1, T2, T3>
			Func<T1, T2, T3> (Action<T1, T2, T3> action)
		{
			return action;
		}

		public static Func<T1, T2, T3, TResult>
			Func<T1, T2, T3, TResult> (Func<T1, T2, T3, TResult> func)
		{
			return func;
		}

		public static Expression<Action<T1, T2, T3>>
			Expression<T1, T2, T3> (Expression<Action<T1, T2, T3>> expr)
		{
			return expr;
		}

		public static Expression<Func<T1, T2, T3, TResult>>
			Expression<T1, T2, T3, TResult> (Expression<Func<T1, T2, T3, TResult>> expr)
		{
			return expr;
		}

		public static Action<T1, T2, T3, T4>
			Func<T1, T2, T3, T4> (Action<T1, T2, T3, T4> action)
		{
			return action;
		}

		public static Func<T1, T2, T3, T4, TResult>
			Func<T1, T2, T3, T4, TResult> (Func<T1, T2, T3, T4, TResult> func)
		{
			return func;
		}

		public static Expression<Action<T1, T2, T3, T4>>
			Expression<T1, T2, T3, T4> (Expression<Action<T1, T2, T3, T4>> expr)
		{
			return expr;
		}

		public static Expression<Func<T1, T2, T3, T4, TResult>>
			Expression<T1, T2, T3, T4, TResult> (Expression<Func<T1, T2, T3, T4, TResult>> expr)
		{
			return expr;
		}
	}
}
