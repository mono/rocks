﻿//
// IEnumerableRocks.cs
//
// Author:
//   Jb Evain (jbevain@novell.com)
//
// Copyright (c) 2007 Novell, Inc. (http://www.novell.com)
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
using System.IO;
using System.Linq;
using System.Text;

namespace Mono.Rocks {

	public static class IEnumerableRocks {

		public static string Implode<TSource> (this IEnumerable<TSource> source, string separator)
		{
			Check.Source (source);

			var coll = source as ICollection<TSource>;
			if (coll != null && coll.Count == 0)
				return string.Empty;

			bool needSep = false;
			var s = new StringBuilder ();

			foreach (var element in source) {
				if (needSep && separator != null)
					s.Append (separator);

				s.Append (element);
				needSep = true;
			}

			return s.ToString ();
		}

		public static string Implode<TSource> (this IEnumerable<TSource> source)
		{
			Check.Source (source);

			return Implode (source, null);
		}

		public static IEnumerable<TSource> Repeat<TSource> (this IEnumerable<TSource> self, int number)
		{
			Check.Self (self);

			return CreateRepeatIterator (self, number);
		}

		private static IEnumerable<TSource> CreateRepeatIterator<TSource> (IEnumerable<TSource> self, int number)
		{
			for (int i = 0; i < number; i++) {
				foreach (var element in self)
					yield return element;
			}
		}

		public static string PathCombine (this IEnumerable<string> self)
		{
			Check.Self (self);

			char [] invalid = Path.GetInvalidPathChars ();
			char [] separators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar };

			StringBuilder sb = null;
			string previous = null;
			foreach (string s in self) {
				if (s == null)
					throw new ArgumentNullException ("path");
				if (s.Length == 0)
					continue;
				if (s.IndexOfAny (invalid) != -1)
					throw new ArgumentException ("Illegal character in path");

				if (sb == null) {
					sb = new StringBuilder (s);
					previous = s;
				} else {
					if (Path.IsPathRooted (s)) {
						sb = new StringBuilder (s);
						continue;
					}

					char last = ((IEnumerable<char>) previous).Last ();
					if (!separators.Contains (last))
						sb.Append (Path.DirectorySeparatorChar);

					sb.Append (s);
					previous = s;
				}
			}
			return (sb == null) ? String.Empty : sb.ToString ();
		}

		public static IEnumerable<TSource> Apply<TSource> (this IEnumerable<TSource> self, Action<TSource> action)
		{
			Check.Self (self);
			if (action == null)
				throw new ArgumentNullException ("action");

			return CreateApplyIterator (self, action);
		}

		private static IEnumerable<TSource> CreateApplyIterator<TSource> (IEnumerable<TSource> self, Action<TSource> action)
		{
			foreach (var item in self) {
				action (item);
				yield return item;
			}
		}

		public static void Apply<TSource> (this IEnumerable<TSource> self)
		{
			Check.Self (self);
#pragma warning disable 168, 219
			foreach (var item in self) {
				// ignore t
			}
#pragma warning restore 168, 219
		}

		public static IEnumerable<TSource> Sort<TSource> (this IEnumerable<TSource> source)
		{
			Check.Source (source);

			List<TSource> s = source.ToList ();
			s.Sort ();
			return s;
		}

		public static IEnumerable<TSource> Sort<TSource> (this IEnumerable<TSource> source, Comparison<TSource> comparison)
		{
			Check.Source (source);
			if (comparison == null)
				throw new ArgumentNullException ("comparison");

			List<TSource> s = source.ToList ();
			s.Sort (comparison);
			return s;
		}

		public static IEnumerable<TSource> Sort<TSource> (this IEnumerable<TSource> source, IComparer<TSource> comparer)
		{
			Check.Source (source);
			if (comparer == null)
				throw new ArgumentNullException ("comparer");

			List<TSource> s = source.ToList ();
			s.Sort (comparer);
			return s;
		}

		public static IEnumerable<TResult> 
			SelectFromEach<TFirstSource, TSecondSource, TResult> (
					this IEnumerable<TFirstSource> source1,
					IEnumerable<TSecondSource> source2, 
					Func<TFirstSource, TSecondSource, TResult> selector)
		{
			if (source1 == null)
				throw new ArgumentNullException ("source1");
			if (source2 == null)
				throw new ArgumentNullException ("source2");
			if (selector == null)
				throw new ArgumentNullException ("selector");

			return CreateSelectFromEachIterator (source1, source2, selector);
		}

		private static IEnumerable<TResult>
			CreateSelectFromEachIterator<TFirstSource, TSecondSource, TResult> (
					IEnumerable<TFirstSource> source1, 
					IEnumerable<TSecondSource> source2, 
					Func<TFirstSource, TSecondSource, TResult> selector)
		{
			using (IEnumerator<TFirstSource>  a = source1.GetEnumerator ())
			using (IEnumerator<TSecondSource> b = source2.GetEnumerator ()) {
				while (a.MoveNext () && b.MoveNext ()) {
					yield return selector (a.Current, b.Current);
				}
			}
		}

		public static IEnumerable<TResult> 
			SelectFromEach<TFirstSource, TSecondSource, TThirdSource, TResult> (
					this IEnumerable<TFirstSource> source1,
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					Func<TFirstSource, TSecondSource, TThirdSource, TResult> selector)
		{
			if (source1 == null)
				throw new ArgumentNullException ("source1");
			if (source2 == null)
				throw new ArgumentNullException ("source2");
			if (source3 == null)
				throw new ArgumentNullException ("source3");
			if (selector == null)
				throw new ArgumentNullException ("selector");

			return CreateSelectFromEachIterator (source1, source2, source3, selector);
		}

		private static IEnumerable<TResult>
			CreateSelectFromEachIterator<TFirstSource, TSecondSource, TThirdSource, TResult> (
					IEnumerable<TFirstSource> source1, 
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					Func<TFirstSource, TSecondSource, TThirdSource, TResult> selector)
		{
			using (IEnumerator<TFirstSource>  a = source1.GetEnumerator ())
			using (IEnumerator<TSecondSource> b = source2.GetEnumerator ())
			using (IEnumerator<TThirdSource>  c = source3.GetEnumerator ()) {
				while (a.MoveNext () && b.MoveNext () && c.MoveNext ()) {
					yield return selector (a.Current, b.Current, c.Current);
				}
			}
		}

		public static IEnumerable<TResult> 
			SelectFromEach<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> (
					this IEnumerable<TFirstSource> source1,
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					IEnumerable<TFourthSource> source4, 
					Func<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> selector)
		{
			if (source1 == null)
				throw new ArgumentNullException ("source1");
			if (source2 == null)
				throw new ArgumentNullException ("source2");
			if (source3 == null)
				throw new ArgumentNullException ("source3");
			if (source4 == null)
				throw new ArgumentNullException ("source4");
			if (selector == null)
				throw new ArgumentNullException ("selector");

			return CreateSelectFromEachIterator (source1, source2, source3, source4, selector);
		}

		private static IEnumerable<TResult>
			CreateSelectFromEachIterator<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> (
					IEnumerable<TFirstSource> source1, 
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					IEnumerable<TFourthSource> source4, 
					Func<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> selector)
		{
			using (IEnumerator<TFirstSource>  a = source1.GetEnumerator ())
			using (IEnumerator<TSecondSource> b = source2.GetEnumerator ())
			using (IEnumerator<TThirdSource>  c = source3.GetEnumerator ())
			using (IEnumerator<TFourthSource> d = source4.GetEnumerator ()) {
				while (a.MoveNext () && b.MoveNext () && c.MoveNext () && d.MoveNext ()) {
					yield return selector (a.Current, b.Current, c.Current, d.Current);
				}
			}
		}

		// Haskell init
		public static IEnumerable<TSource> ExceptLast<TSource> (this IEnumerable<TSource> self)
		{
			return ExceptLast (self, 1);
		}

		public static IEnumerable<TSource> ExceptLast<TSource> (this IEnumerable<TSource> self, int count)
		{
			Check.Self (self);
			if (count < 0)
				throw new ArgumentException ("count", "must be >= 0");
			if (count == 0)
				return self;

			return CreateExceptLastIterator (self, count);
		}

		private static IEnumerable<TSource> CreateExceptLastIterator<TSource> (IEnumerable<TSource> self, int count)
		{
			Queue<TSource> ignore = new Queue<TSource> (count);
			foreach (TSource e in self) {
				if (ignore.Count < count)
					ignore.Enqueue (e);
				else if (ignore.Count == count) {
					yield return ignore.Dequeue ();
					ignore.Enqueue (e);
				}
				else
					throw new InvalidOperationException ("should not happen");
			}
		}

		// Haskell intersperse
		public static IEnumerable<TSource> Intersperse<TSource> (this IEnumerable<TSource> self, TSource value)
		{
			Check.Self (self);
			return CreateIntersperseIterator (self, value);
		}

		private static IEnumerable<TSource> CreateIntersperseIterator<TSource> (IEnumerable<TSource> self, TSource value)
		{
			bool insert = false;
			foreach (TSource v in self) {
				if (insert)
					yield return value;
				yield return v;
				insert = true;
			}
		}

		// Haskell intercalate
		public static IEnumerable<TSource> Intersperse<TSource> (this IEnumerable<IEnumerable<TSource>> self, IEnumerable<TSource> between)
		{
			Check.Self (self);
			if (between == null)
				throw new ArgumentNullException ("lists");

			IEnumerable<IEnumerable<TSource>> e = Intersperse<IEnumerable<TSource>> (self, between);
			return Concat (Enumerable.Empty<TSource>(), e);
		}

		// Haskell transpose
		public static IEnumerable<IEnumerable<TSource>> Transpose<TSource> (this IEnumerable<IEnumerable<TSource>> self)
		{
			Check.Self (self);

			IList<IList<TSource>> items = self as IList<IList<TSource>>;
			int max = 0;
			if (items == null) {
				items = new List<IList<TSource>> ();
				foreach (var outer in self) {
					List<TSource> c = new List<TSource> ();
					items.Add (c);
					foreach (var inner in outer) {
						c.Add (inner);
					}
					max = System.Math.Max (max, c.Count);
				}
			} else {
				for (int i = 0; i < items.Count; ++i)
					max = System.Math.Max (max, items [i].Count);
			}

			return CreateTransposeIterator (items, max);
		}

		private static IEnumerable<IEnumerable<TSource>> CreateTransposeIterator<TSource> (IList<IList<TSource>> items, int max)
		{
			for (int j = 0; j < max; ++j)
				yield return CreateTransposeColumnIterator (items, j);
		}

		private static IEnumerable<TSource> CreateTransposeColumnIterator<TSource> (IList<IList<TSource>> items, int column)
		{
			for (int i = 0; i < items.Count; ++i) {
				yield return (items [i].Count > column) 
					? items [i][column] 
					: default (TSource);
			}
		}

		public static List<List<TSource>> ToList<TSource> (this IEnumerable<IEnumerable<TSource>> source)
		{
			Check.Self (source);

			List<List<TSource>> r = new List<List<TSource>> ();
			foreach (IEnumerable<TSource> row in source) {
				List<TSource> items = new List<TSource> ();
				r.Add (items);
				foreach (TSource item in row) {
					items.Add (item);
				}
			}
			return r;
		}

		#region AggregateReverse

		// Haskell: foldr1
		public static TSource AggregateReverse<TSource> (this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			Check.SourceAndFunc (source, func);

			IList<TSource> s = GetList (source);
			if (s.Count == 0)
				throw new InvalidOperationException ("No elements in source list");

			TSource folded = s [s.Count - 1];
			for (int i = s.Count-2; i >= 0; --i) {
				folded = func (folded, s [i]);
			}
			return folded;
		}

		private static IList<TSource> GetList<TSource> (IEnumerable<TSource> source)
		{
			IList<TSource> s = source as IList<TSource>;
			if (s == null) {
				s = new List<TSource> (source);
			}

			return s;
		}

		// Haskell: foldr
		public static TAccumulate AggregateReverse<TSource, TAccumulate> (this IEnumerable<TSource> source,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SourceAndFunc (source, func);

			IList<TSource> s = GetList (source);

			TAccumulate folded = seed;
			for (int i = s.Count-1; i >= 0; --i) {
				folded = func (folded, s [i]);
			}

			return folded;
		}

		public static TResult AggregateReverse<TSource, TAccumulate, TResult> (this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SourceAndFunc (source, func);
			if (resultSelector == null)
				throw new ArgumentNullException ("resultSelector");

			IList<TSource> s = GetList (source);

			var result = seed;
			for (int i = s.Count-1; i >= 0; --i)
				result = func (result, s [i]);

			return resultSelector (result);
		}

		#endregion

		// Haskell concat
		public static IEnumerable<TSource> Concat<TSource> (this IEnumerable<TSource> source, params IEnumerable<TSource>[] sources)
		{
			IEnumerable<IEnumerable<TSource>> e = sources;
			return Concat (source, e);
		}

		public static IEnumerable<TSource> Concat<TSource> (this IEnumerable<TSource> source, IEnumerable<IEnumerable<TSource>> sources)
		{
			Check.Source (source);
			if (sources == null)
				throw new ArgumentNullException ("sources");

			return CreateConcatIterator (source, sources);
		}

		private static IEnumerable<TSource> CreateConcatIterator<TSource> (IEnumerable<TSource> source, IEnumerable<IEnumerable<TSource>> sources)
		{
			foreach (TSource e in source)
				yield return e;
			foreach (IEnumerable<TSource> outer in sources) {
				foreach (TSource e in outer)
					yield return e;
			}
		}

		// Haskell: and
		public static bool And (this IEnumerable<bool> source)
		{
			Check.Self (source);

			return source.Aggregate ((a, b) => a && b);
		}

		// Haskell: or
		public static bool Or (this IEnumerable<bool> source)
		{
			Check.Self (source);

			return source.Aggregate ((a, b) => a || b);
		}

		#region AggregateHistory

		// Haskell: scanl1
		public static IEnumerable<TSource> AggregateHistory<TSource> (this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			Check.SourceAndFunc (source, func);

			return CreateAggregateHistoryIterator (source, func);
		}

		private static IEnumerable<TSource> CreateAggregateHistoryIterator<TSource> (IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			// custom foreach so that we can efficiently throw an exception
			// if zero elements and treat the first element differently
			using (var enumerator = source.GetEnumerator ()) {
				if (!enumerator.MoveNext ())
					throw new InvalidOperationException ("No elements in source list");

				TSource folded;
				yield return (folded = enumerator.Current);
				while (enumerator.MoveNext ())
					yield return (folded = func (folded, enumerator.Current));

				enumerator.Dispose ();
			}
		}

		// Haskell: scanl
		public static IEnumerable<TAccumulate> AggregateHistory<TSource, TAccumulate> (this IEnumerable<TSource> source,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SourceAndFunc (source, func);

			return CreateAggregateHistoryIterator (source, seed, func);
		}

		private static IEnumerable<TAccumulate> CreateAggregateHistoryIterator<TSource, TAccumulate> (IEnumerable<TSource> source,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			TAccumulate folded;
			yield return (folded = seed);
			foreach (TSource element in source)
				yield return (folded = func (folded, element));
		}

		public static IEnumerable<TResult> AggregateHistory<TSource, TAccumulate, TResult> (this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SourceAndFunc (source, func);
			if (resultSelector == null)
				throw new ArgumentNullException ("resultSelector");

			return CreateAggregateHistoryIterator (source, seed, func, resultSelector);
		}

		private static IEnumerable<TResult> CreateAggregateHistoryIterator<TSource, TAccumulate, TResult> (IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			var result = seed;
			yield return resultSelector (result);
			foreach (var e in source)
				yield return resultSelector (result = func (result, e));
		}

		#endregion

		#region AggregateReverseHistory

		// Haskell: scanr1
		public static IEnumerable<TSource> AggregateReverseHistory<TSource> (this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			Check.SourceAndFunc (source, func);

			return CreateAggregateReverseHistoryIterator (source, func);
		}

		private static IEnumerable<TSource> CreateAggregateReverseHistoryIterator<TSource> (IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
		{
			IList<TSource> s = GetList (source);
			if (s.Count == 0)
				throw new InvalidOperationException ("No elements in source list");

			TSource folded;
			yield return (folded = s [s.Count - 1]);
			for (int i = s.Count-2; i >= 0; --i)
				yield return (folded = func (folded, s [i]));
		}

		// Haskell: scanr
		public static IEnumerable<TAccumulate> AggregateReverseHistory<TSource, TAccumulate> (this IEnumerable<TSource> source,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SourceAndFunc (source, func);

			return CreateAggregateReverseHistoryIterator (source, seed, func);
		}

		private static IEnumerable<TAccumulate> CreateAggregateReverseHistoryIterator<TSource, TAccumulate> (IEnumerable<TSource> source,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			IList<TSource> s = GetList (source);

			TAccumulate folded;
			yield return (folded = seed);
			for (int i = s.Count-1; i >= 0; --i)
				yield return (folded = func (folded, s [i]));
		}

		public static IEnumerable<TResult> AggregateReverseHistory<TSource, TAccumulate, TResult> (this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SourceAndFunc (source, func);
			if (resultSelector == null)
				throw new ArgumentNullException ("resultSelector");

			return CreateAggregateReverseHistoryIterator (source, seed, func, resultSelector);
		}

		private static IEnumerable<TResult> CreateAggregateReverseHistoryIterator<TSource, TAccumulate, TResult> (IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			IList<TSource> s = GetList (source);

			var result = seed;
			yield return resultSelector (result);
			for (int i = s.Count-1; i >= 0; --i)
				yield return resultSelector (result = func (result, s [i]));
		}

		#endregion
	}
}
