//
// Tuple.cs
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
using System.Reflection;
using System.Text;

namespace Mono.Rocks {

	public abstract partial class Tuple : IList, IList<object> {

		protected Tuple ()
		{
		}

		public override abstract int GetHashCode ();

		public override abstract bool Equals (object o);

		#region ICollection
		void ICollection.CopyTo (Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ();
			if (index < 0)
				throw new ArgumentOutOfRangeException ();
			if (array.Length - index <= 0 ||
					(array.Length - index) < Count)
				throw new ArgumentException ();
			for (int i = 0; i < Count; ++i) {
				array.SetValue (this [i], index + i);
			}
		}
		bool ICollection.IsSynchronized   {get {return false;}}
		object ICollection.SyncRoot       {get {return this;}}
		#endregion

		#region ICollection<T>
		void ICollection<object>.Add (object item)     {throw new NotSupportedException();}
		void ICollection<object>.Clear ()              {throw new NotSupportedException();}
		bool ICollection<object>.Remove (object item)  {throw new NotSupportedException();}
		bool ICollection<object>.IsReadOnly            {get {return true;}}

		public abstract int Count { get; }

		public bool Contains (object item)
		{
			return IndexOf (item) >= 0;
		}

		public void CopyTo (object[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException ();
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException ();
			if (array.Length - arrayIndex <= 0 ||
					(array.Length - arrayIndex) < Count)
				throw new ArgumentException ();
			for (int i = 0; i < Count; ++i) {
				array [arrayIndex + i] = this [i];
			}
		}
		#endregion

		#region IEnumerable
		IEnumerator IEnumerable.GetEnumerator () {return GetEnumerator ();}
		#endregion

		#region IEnumerable<T>
		public IEnumerator<object> GetEnumerator ()
		{
			for (int i = 0; i < Count; ++i) {
				yield return this [i];
			}
		}
		#endregion

		#region IList
		int IList.Add (object value)                {throw new NotSupportedException();}
		void IList.Clear ()                         {throw new NotSupportedException();}
		void IList.Insert (int index, object value) {throw new NotSupportedException();}
		void IList.Remove (object value)            {throw new NotSupportedException();}
		void IList.RemoveAt (int index)             {throw new NotSupportedException();}
		bool IList.IsFixedSize                      {get {return true;}}
		bool IList.IsReadOnly                       {get {return true;}}
		#endregion

		#region IList<T>
		void IList<object>.Insert (int index, object item) {throw new NotSupportedException();}
		void IList<object>.RemoveAt (int index)            {throw new NotSupportedException();}

		public int IndexOf (object item)
		{
			for (int i = 0; i < Count; ++i) {
				if (object.Equals (this [i], item))
					return i;
			}
			return -1;
		}

		public virtual object this [int index] {
			get {
				throw new IndexOutOfRangeException (index.ToString ());
			}
			set {
				throw new NotSupportedException ("Tuple is read-only");
			}
		}
		#endregion

		protected virtual void AppendValue (StringBuilder buf)
		{
		}

		public override string ToString ()
		{
			StringBuilder buf = new StringBuilder ();
			buf.Append ("(");
			AppendValue (buf);
			buf.Append (")");
			return buf.ToString ();
		}
	}

	public static class TupleRocks
	{
		public static KeyValuePair<TKey, TValue>
			ToKeyValuePair<TKey, TValue> (this Tuple<TKey, TValue> self)
		{
			Check.Self (self);
			return new KeyValuePair<TKey, TValue> (self._1, self._2);
		}
	}
}

