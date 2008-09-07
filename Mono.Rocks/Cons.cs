//
// Cons.cs: Thread-Safe, Immutable, Singly Linked List.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mono.Rocks {

	public class Cons<T> : IEnumerable<T>
	{
		private T head;
		private Cons<T> tail;
		private IEnumerator<T> iter;

		public Cons (T head)
		{
			this.head = head;
		}

		public Cons (T head, Cons<T> tail)
		{
			this.head = head;
			this.tail = tail;
		}

		public Cons (IEnumerable<T> collection)
		{
			if (collection == null)
				throw new ArgumentNullException ("collection");

			this.iter = collection.GetEnumerator ();

			if (!iter.MoveNext ()) {
				iter.Dispose ();
				throw new InvalidOperationException ("no elements");
			}

			this.head = iter.Current;

			if (!iter.MoveNext ()) {
				iter.Dispose ();
				iter = null;
			}
		}

		private Cons (IEnumerator<T> iter)
		{
			this.head = iter.Current;
			this.iter = iter;
		}

		public T Head {
			get {return head;}
		}

		public Cons<T> Tail {
			get {
				if (iter != null) {
					lock (iter) {
						if (iter != null) {
							try {
								Cons<T> newTail = new Cons<T> (iter);
								if (!iter.MoveNext ()) {
									newTail.iter = null;
									iter.Dispose ();
								}
								this.tail = newTail;
							} finally {
								iter = null;
							}
						}
					}
				}
				return tail;
			}
		}

		public Cons<T> Append (T value)
		{
			Cons<T> start, c;
			start = c = new Cons<T> (Head);

			Cons<T> end = Tail;
			while (end != null) {
				c.tail = new Cons<T> (end.Head);
				c      = c.tail;
				end    = end.Tail;
			}
			c.tail = new Cons<T> (value);

			return start;
		}

		public Cons<T> Prepend (T value)
		{
			return new Cons<T> (value, this);
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		public IEnumerator<T> GetEnumerator ()
		{
			yield return Head;
			Cons<T> c = Tail;
			while (c != null) {
				yield return c.Head;
				c = c.Tail;
			}
		}

#region LINQ optimizations
		public int Count ()
		{
			return checked((int) LongCount ());
		}

		public T ElementAt (int index)
		{
			if (index < 0)
				throw new ArgumentOutOfRangeException ();
			Cons<T> c = this;
			while (--index >= 0) {
				c = c.Tail;
			}
			return c.Head;
		}

		public T First ()
		{
			return Head;
		}

		public T FirstOrDefault ()
		{
			return Head;
		}

		public long LongCount ()
		{
			long count = 1;
			var t = Tail;
			while (t != null) {
				++count;
				t = t.Tail;
			}
			return count;
		}

		public Cons<T> Reverse ()
		{
			Cons<T> newHead = new Cons<T> (Head);
			Cons<T> t = Tail;
			while (t != null) {
				newHead = new Cons<T> (t.Head, newHead);
				t       = t.Tail;
			}
			return newHead;
		}
#endregion
	}
}

