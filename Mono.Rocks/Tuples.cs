//
// Tuples.cs: Tuple types.
//
// GENERATED CODE: DO NOT EDIT.
// 
// To regenerate this code, execute: ./mktuples -n 4 -o Tuples.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mono.Rocks {

	public abstract partial class Tuple {

		public static Tuple<T>
			Create<T> (T value)
		{
			return new Tuple<T> (value);
		}

		public static Tuple<T1, T2>
			Create<T1, T2> (T1 value1, T2 value2)
		{
			return new Tuple<T1, T2> (value1, value2);
		}

		public static Tuple<T1, T2, T3>
			Create<T1, T2, T3> (T1 value1, T2 value2, T3 value3)
		{
			return new Tuple<T1, T2, T3> (value1, value2, value3);
		}

		public static Tuple<T1, T2, T3, T4>
			Create<T1, T2, T3, T4> (T1 value1, T2 value2, T3 value3, T4 value4)
		{
			return new Tuple<T1, T2, T3, T4> (value1, value2, value3, value4);
		}
	}

	public class Tuple<T>
		: Tuple, IEquatable<Tuple<T>>
	{
		private readonly T value;

		public Tuple (T value)
		{
			this.value = value;
		}

		public T _1 {get{return value;}}

		public override int Count {
			get {return 1;}
		}

		public override int GetHashCode ()
		{
			int hc = 0;
			hc ^= _1.GetHashCode ();
			return hc;
		}

		public override bool Equals (object o)
		{
			Tuple<T> t = o as Tuple<T>;
			if (t == null)
				return false;
			return Equals (t);
		}

		public bool Equals (Tuple<T> o)
		{
			return EqualityComparer<T>.Default.Equals (_1, o._1)
				;
		}

		public override object this [int index] {
			get {
				switch (index) {
					case 0: return _1;
				}
				return base [index];
			}
		}

		public TResult Aggregate<TResult> (Func<T, TResult> func)
		{
			return func (value);
		}

		protected override void AppendValue (StringBuilder buf)
		{
			buf.Append (_1);
		}
	}

	public class Tuple<T1, T2>
		: Tuple, IEquatable<Tuple<T1, T2>>
	{
		private readonly T1 value1;
		private readonly T2 value2;

		public Tuple (T1 value1, T2 value2)
		{
			this.value1 = value1;
			this.value2 = value2;
		}

		public T1 _1 {get{return value1;}}
		public T2 _2 {get{return value2;}}

		public override int Count {
			get {return 2;}
		}

		public override int GetHashCode ()
		{
			int hc = 0;
			hc ^= _1.GetHashCode ();
			hc ^= _2.GetHashCode ();
			return hc;
		}

		public override bool Equals (object o)
		{
			Tuple<T1, T2> t = o as Tuple<T1, T2>;
			if (t == null)
				return false;
			return Equals (t);
		}

		public bool Equals (Tuple<T1, T2> o)
		{
			return EqualityComparer<T1>.Default.Equals (_1, o._1)
				&& EqualityComparer<T2>.Default.Equals (_2, o._2)
				;
		}

		public override object this [int index] {
			get {
				switch (index) {
					case 0: return _1;
					case 1: return _2;
				}
				return base [index];
			}
		}

		public TResult Aggregate<TResult> (Func<T1, T2, TResult> func)
		{
			return func (value1, value2);
		}

		protected override void AppendValue (StringBuilder buf)
		{
			buf.Append (_1);
			buf.Append (", ");
			buf.Append (_2);
		}
	}

	public class Tuple<T1, T2, T3>
		: Tuple, IEquatable<Tuple<T1, T2, T3>>
	{
		private readonly T1 value1;
		private readonly T2 value2;
		private readonly T3 value3;

		public Tuple (T1 value1, T2 value2, T3 value3)
		{
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
		}

		public T1 _1 {get{return value1;}}
		public T2 _2 {get{return value2;}}
		public T3 _3 {get{return value3;}}

		public override int Count {
			get {return 3;}
		}

		public override int GetHashCode ()
		{
			int hc = 0;
			hc ^= _1.GetHashCode ();
			hc ^= _2.GetHashCode ();
			hc ^= _3.GetHashCode ();
			return hc;
		}

		public override bool Equals (object o)
		{
			Tuple<T1, T2, T3> t = o as Tuple<T1, T2, T3>;
			if (t == null)
				return false;
			return Equals (t);
		}

		public bool Equals (Tuple<T1, T2, T3> o)
		{
			return EqualityComparer<T1>.Default.Equals (_1, o._1)
				&& EqualityComparer<T2>.Default.Equals (_2, o._2)
				&& EqualityComparer<T3>.Default.Equals (_3, o._3)
				;
		}

		public override object this [int index] {
			get {
				switch (index) {
					case 0: return _1;
					case 1: return _2;
					case 2: return _3;
				}
				return base [index];
			}
		}

		public TResult Aggregate<TResult> (Func<T1, T2, T3, TResult> func)
		{
			return func (value1, value2, value3);
		}

		protected override void AppendValue (StringBuilder buf)
		{
			buf.Append (_1);
			buf.Append (", ");
			buf.Append (_2);
			buf.Append (", ");
			buf.Append (_3);
		}
	}

	public class Tuple<T1, T2, T3, T4>
		: Tuple, IEquatable<Tuple<T1, T2, T3, T4>>
	{
		private readonly T1 value1;
		private readonly T2 value2;
		private readonly T3 value3;
		private readonly T4 value4;

		public Tuple (T1 value1, T2 value2, T3 value3, T4 value4)
		{
			this.value1 = value1;
			this.value2 = value2;
			this.value3 = value3;
			this.value4 = value4;
		}

		public T1 _1 {get{return value1;}}
		public T2 _2 {get{return value2;}}
		public T3 _3 {get{return value3;}}
		public T4 _4 {get{return value4;}}

		public override int Count {
			get {return 4;}
		}

		public override int GetHashCode ()
		{
			int hc = 0;
			hc ^= _1.GetHashCode ();
			hc ^= _2.GetHashCode ();
			hc ^= _3.GetHashCode ();
			hc ^= _4.GetHashCode ();
			return hc;
		}

		public override bool Equals (object o)
		{
			Tuple<T1, T2, T3, T4> t = o as Tuple<T1, T2, T3, T4>;
			if (t == null)
				return false;
			return Equals (t);
		}

		public bool Equals (Tuple<T1, T2, T3, T4> o)
		{
			return EqualityComparer<T1>.Default.Equals (_1, o._1)
				&& EqualityComparer<T2>.Default.Equals (_2, o._2)
				&& EqualityComparer<T3>.Default.Equals (_3, o._3)
				&& EqualityComparer<T4>.Default.Equals (_4, o._4)
				;
		}

		public override object this [int index] {
			get {
				switch (index) {
					case 0: return _1;
					case 1: return _2;
					case 2: return _3;
					case 3: return _4;
				}
				return base [index];
			}
		}

		public TResult Aggregate<TResult> (Func<T1, T2, T3, T4, TResult> func)
		{
			return func (value1, value2, value3, value4);
		}

		protected override void AppendValue (StringBuilder buf)
		{
			buf.Append (_1);
			buf.Append (", ");
			buf.Append (_2);
			buf.Append (", ");
			buf.Append (_3);
			buf.Append (", ");
			buf.Append (_4);
		}
	}
}
