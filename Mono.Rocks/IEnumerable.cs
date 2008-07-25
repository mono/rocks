//
// IEnumerableRocks.cs
//
// Author:
//   Jb Evain (jbevain@novell.com)
//   Jonathan Pryor  <jpryor@novell.com>
//
// Copyright (c) 2007, 2008 Novell, Inc. (http://www.novell.com)
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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Mono.Rocks {

	public static class IEnumerableRocks {

		public static string Implode<TSource> (this IEnumerable<TSource> self, string separator)
		{
			return Implode (self, separator, (b, e) => {b.Append (e.ToString ());});
		}

		public static string Implode<TSource> (this IEnumerable<TSource> self)
		{
			return Implode (self, null);
		}

		public static string Implode<TSource, TResult> (this IEnumerable<TSource> self, string separator, Func<TSource, TResult> selector)
		{
			if (selector == null)
				throw new ArgumentNullException ("selector");
			return Implode (self, separator, (b, e) => {b.Append (selector (e).ToString ());});
		}

		public static string Implode<TSource> (this IEnumerable<TSource> self, string separator, Action<StringBuilder, TSource> appender)
		{
			Check.Self (self);
			if (appender == null)
				throw new ArgumentNullException ("appender");

			var coll = self as ICollection<TSource>;
			if (coll != null && coll.Count == 0)
				return string.Empty;

			bool needSep = false;
			var s = new StringBuilder ();

			foreach (var element in self) {
				if (needSep && separator != null)
					s.Append (separator);

				appender (s, element);
				needSep = true;
			}

			return s.ToString ();
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

		public static IEnumerable<TSource> ApplyPairs<TSource> (this IEnumerable<TSource> self, params Action<TSource>[] actions)
		{
			Check.Self (self);
			if (actions == null)
				throw new ArgumentNullException ("actions");

			return CreateApplyPairsIterator (self, actions);
		}

		private static IEnumerable<TSource> CreateApplyPairsIterator<TSource> (IEnumerable<TSource> self, IEnumerable<Action<TSource>> actions)
		{
			using (IEnumerator<TSource> s = self.GetEnumerator ())
			using (IEnumerator<Action<TSource>> a = actions.GetEnumerator ()) {
				bool have_s;
				while ((have_s = s.MoveNext ()) && a.MoveNext ()) {
					a.Current (s.Current);
					yield return s.Current;
				}

				if (have_s)
					while (s.MoveNext ())
						yield return s.Current;
			}
		}

		public static void Convert<TSource, TValue> (this IEnumerable<TSource> self, out TValue value)
		{
			TValue v1 = default(TValue);
			ApplyPairs (self, 
					v => v1 = Convert<TSource, TValue> (v)).Apply ();
			value = v1;
		}

		private static TResult Convert<TSource, TResult> (TSource value)
		{
			TypeConverter conv = TypeDescriptor.GetConverter (typeof(TResult));
			return (TResult) conv.ConvertFrom (value);
		}

		public static void Convert<TSource, TValue1, TValue2> (this IEnumerable<TSource> self, out TValue1 value1, out TValue2 value2)
		{
			TValue1 v1 = default(TValue1);
			TValue2 v2 = default(TValue2);
			ApplyPairs (self, 
					v => v1 = Convert<TSource, TValue1> (v),
					v => v2 = Convert<TSource, TValue2> (v)).Apply ();
			value1 = v1;
			value2 = v2;
		}

		public static void Convert<TSource, TValue1, TValue2, TValue3> (this IEnumerable<TSource> self, out TValue1 value1, out TValue2 value2, out TValue3 value3)
		{
			TValue1 v1 = default(TValue1);
			TValue2 v2 = default(TValue2);
			TValue3 v3 = default(TValue3);
			ApplyPairs (self, 
					v => v1 = Convert<TSource, TValue1> (v),
					v => v2 = Convert<TSource, TValue2> (v),
					v => v3 = Convert<TSource, TValue3> (v)).Apply ();
			value1 = v1;
			value2 = v2;
			value3 = v3;
		}

		public static void Convert<TSource, TValue1, TValue2, TValue3, TValue4> (this IEnumerable<TSource> self, out TValue1 value1, out TValue2 value2, out TValue3 value3, out TValue4 value4)
		{
			TValue1 v1 = default(TValue1);
			TValue2 v2 = default(TValue2);
			TValue3 v3 = default(TValue3);
			TValue4 v4 = default(TValue4);
			ApplyPairs (self, 
					v => v1 = Convert<TSource, TValue1> (v),
					v => v2 = Convert<TSource, TValue2> (v),
					v => v3 = Convert<TSource, TValue3> (v),
					v => v4 = Convert<TSource, TValue4> (v)).Apply ();
			value1 = v1;
			value2 = v2;
			value3 = v3;
			value4 = v4;
		}

		public static IEnumerable<TSource> Sort<TSource> (this IEnumerable<TSource> self)
		{
			Check.Self (self);

			List<TSource> s = self.ToList ();
			s.Sort ();
			return s;
		}

		public static IEnumerable<TSource> Sort<TSource> (this IEnumerable<TSource> self, Comparison<TSource> comparison)
		{
			Check.Self (self);
			if (comparison == null)
				throw new ArgumentNullException ("comparison");

			List<TSource> s = self.ToList ();
			s.Sort (comparison);
			return s;
		}

		public static IEnumerable<TSource> Sort<TSource> (this IEnumerable<TSource> self, IComparer<TSource> comparer)
		{
			Check.Self (self);
			if (comparer == null)
				throw new ArgumentNullException ("comparer");

			List<TSource> s = self.ToList ();
			s.Sort (comparer);
			return s;
		}

		public static IEnumerable<TSource> OrderByNatural<TSource> (this IEnumerable<TSource> self, Func<TSource, string> func)
		{
			Check.SelfAndFunc (self, func);

			return self.OrderBy (func, NaturalStringComparer.Default);
		}

		public static IEnumerable<string> SortNatural (this IEnumerable<string> self)
		{
			return Sort (self, NaturalStringComparer.Default);
		}

		public static IEnumerable<TResult> 
			SelectFromEach<TFirstSource, TSecondSource, TResult> (
					this IEnumerable<TFirstSource> self,
					IEnumerable<TSecondSource> source2, 
					Func<TFirstSource, TSecondSource, TResult> selector)
		{
			Check.Self (self);
			if (source2 == null)
				throw new ArgumentNullException ("source2");
			if (selector == null)
				throw new ArgumentNullException ("selector");

			return CreateSelectFromEachIterator (self, source2, selector);
		}

		private static IEnumerable<TResult>
			CreateSelectFromEachIterator<TFirstSource, TSecondSource, TResult> (
					IEnumerable<TFirstSource> self, 
					IEnumerable<TSecondSource> source2, 
					Func<TFirstSource, TSecondSource, TResult> selector)
		{
			using (IEnumerator<TFirstSource>  a = self.GetEnumerator ())
			using (IEnumerator<TSecondSource> b = source2.GetEnumerator ()) {
				while (a.MoveNext () && b.MoveNext ()) {
					yield return selector (a.Current, b.Current);
				}
			}
		}

		public static IEnumerable<TResult> 
			SelectFromEach<TFirstSource, TSecondSource, TThirdSource, TResult> (
					this IEnumerable<TFirstSource> self,
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3,
					Func<TFirstSource, TSecondSource, TThirdSource, TResult> selector)
		{
			Check.Self (self);
			if (source2 == null)
				throw new ArgumentNullException ("source2");
			if (source3 == null)
				throw new ArgumentNullException ("source3");
			if (selector == null)
				throw new ArgumentNullException ("selector");

			return CreateSelectFromEachIterator (self, source2, source3, selector);
		}

		private static IEnumerable<TResult>
			CreateSelectFromEachIterator<TFirstSource, TSecondSource, TThirdSource, TResult> (
					IEnumerable<TFirstSource> self, 
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					Func<TFirstSource, TSecondSource, TThirdSource, TResult> selector)
		{
			using (IEnumerator<TFirstSource>  a = self.GetEnumerator ())
			using (IEnumerator<TSecondSource> b = source2.GetEnumerator ())
			using (IEnumerator<TThirdSource>  c = source3.GetEnumerator ()) {
				while (a.MoveNext () && b.MoveNext () && c.MoveNext ()) {
					yield return selector (a.Current, b.Current, c.Current);
				}
			}
		}

		public static IEnumerable<TResult> 
			SelectFromEach<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> (
					this IEnumerable<TFirstSource> self,
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					IEnumerable<TFourthSource> source4, 
					Func<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> selector)
		{
			if (self == null)
				throw new ArgumentNullException ("self");
			if (source2 == null)
				throw new ArgumentNullException ("source2");
			if (source3 == null)
				throw new ArgumentNullException ("source3");
			if (source4 == null)
				throw new ArgumentNullException ("source4");
			if (selector == null)
				throw new ArgumentNullException ("selector");

			return CreateSelectFromEachIterator (self, source2, source3, source4, selector);
		}

		private static IEnumerable<TResult>
			CreateSelectFromEachIterator<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> (
					IEnumerable<TFirstSource> self, 
					IEnumerable<TSecondSource> source2, 
					IEnumerable<TThirdSource> source3, 
					IEnumerable<TFourthSource> source4, 
					Func<TFirstSource, TSecondSource, TThirdSource, TFourthSource, TResult> selector)
		{
			using (IEnumerator<TFirstSource>  a = self.GetEnumerator ())
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

		public static List<List<TSource>> ToList<TSource> (this IEnumerable<IEnumerable<TSource>> self)
		{
			Check.Self (self);

			List<List<TSource>> r = new List<List<TSource>> ();
			foreach (IEnumerable<TSource> row in self) {
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
		public static TSource AggregateReverse<TSource> (this IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
		{
			Check.SelfAndFunc (self, func);

			IList<TSource> s = GetList (self);
			if (s.Count == 0)
				throw new InvalidOperationException ("No elements in self list");

			TSource folded = s [s.Count - 1];
			for (int i = s.Count-2; i >= 0; --i) {
				folded = func (folded, s [i]);
			}
			return folded;
		}

		private static IList<TSource> GetList<TSource> (IEnumerable<TSource> self)
		{
			IList<TSource> s = self as IList<TSource>;
			if (s == null) {
				s = new List<TSource> (self);
			}

			return s;
		}

		// Haskell: foldr
		public static TAccumulate AggregateReverse<TSource, TAccumulate> (this IEnumerable<TSource> self,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SelfAndFunc (self, func);

			IList<TSource> s = GetList (self);

			TAccumulate folded = seed;
			for (int i = s.Count-1; i >= 0; --i) {
				folded = func (folded, s [i]);
			}

			return folded;
		}

		public static TResult AggregateReverse<TSource, TAccumulate, TResult> (this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SelfAndFunc (self, func);
			if (resultSelector == null)
				throw new ArgumentNullException ("resultSelector");

			IList<TSource> s = GetList (self);

			var result = seed;
			for (int i = s.Count-1; i >= 0; --i)
				result = func (result, s [i]);

			return resultSelector (result);
		}

		#endregion

		// Haskell concat
		public static IEnumerable<TSource> Concat<TSource> (this IEnumerable<TSource> self, params IEnumerable<TSource>[] selfs)
		{
			IEnumerable<IEnumerable<TSource>> e = selfs;
			return Concat (self, e);
		}

		public static IEnumerable<TSource> Concat<TSource> (this IEnumerable<TSource> self, IEnumerable<IEnumerable<TSource>> selfs)
		{
			Check.Self (self);
			if (selfs == null)
				throw new ArgumentNullException ("selfs");

			return CreateConcatIterator (self, selfs);
		}

		private static IEnumerable<TSource> CreateConcatIterator<TSource> (IEnumerable<TSource> self, IEnumerable<IEnumerable<TSource>> selfs)
		{
			foreach (TSource e in self)
				yield return e;
			foreach (IEnumerable<TSource> outer in selfs) {
				foreach (TSource e in outer)
					yield return e;
			}
		}

		// Haskell: and
		public static bool And (this IEnumerable<bool> self)
		{
			Check.Self (self);

			return self.Aggregate ((a, b) => a && b);
		}

		// Haskell: or
		public static bool Or (this IEnumerable<bool> self)
		{
			Check.Self (self);

			return self.Aggregate ((a, b) => a || b);
		}

		#region AggregateHistory

		// Haskell: scanl1
		public static IEnumerable<TSource> AggregateHistory<TSource> (this IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
		{
			Check.SelfAndFunc (self, func);

			return CreateAggregateHistoryIterator (self, func);
		}

		private static IEnumerable<TSource> CreateAggregateHistoryIterator<TSource> (IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
		{
			// custom foreach so that we can efficiently throw an exception
			// if zero elements and treat the first element differently
			using (var enumerator = self.GetEnumerator ()) {
				if (!enumerator.MoveNext ())
					throw new InvalidOperationException ("No elements in self list");

				TSource folded;
				yield return (folded = enumerator.Current);
				while (enumerator.MoveNext ())
					yield return (folded = func (folded, enumerator.Current));

				enumerator.Dispose ();
			}
		}

		// Haskell: scanl
		public static IEnumerable<TAccumulate> AggregateHistory<TSource, TAccumulate> (this IEnumerable<TSource> self,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SelfAndFunc (self, func);

			return CreateAggregateHistoryIterator (self, seed, func);
		}

		private static IEnumerable<TAccumulate> CreateAggregateHistoryIterator<TSource, TAccumulate> (IEnumerable<TSource> self,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			TAccumulate folded;
			yield return (folded = seed);
			foreach (TSource element in self)
				yield return (folded = func (folded, element));
		}

		public static IEnumerable<TResult> AggregateHistory<TSource, TAccumulate, TResult> (this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SelfAndFunc (self, func);
			if (resultSelector == null)
				throw new ArgumentNullException ("resultSelector");

			return CreateAggregateHistoryIterator (self, seed, func, resultSelector);
		}

		private static IEnumerable<TResult> CreateAggregateHistoryIterator<TSource, TAccumulate, TResult> (IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			var result = seed;
			yield return resultSelector (result);
			foreach (var e in self)
				yield return resultSelector (result = func (result, e));
		}

		#endregion

		#region AggregateReverseHistory

		// Haskell: scanr1
		public static IEnumerable<TSource> AggregateReverseHistory<TSource> (this IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
		{
			Check.SelfAndFunc (self, func);

			return CreateAggregateReverseHistoryIterator (self, func);
		}

		private static IEnumerable<TSource> CreateAggregateReverseHistoryIterator<TSource> (IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
		{
			IList<TSource> s = GetList (self);
			if (s.Count == 0)
				throw new InvalidOperationException ("No elements in self list");

			TSource folded;
			yield return (folded = s [s.Count - 1]);
			for (int i = s.Count-2; i >= 0; --i)
				yield return (folded = func (folded, s [i]));
		}

		// Haskell: scanr
		public static IEnumerable<TAccumulate> AggregateReverseHistory<TSource, TAccumulate> (this IEnumerable<TSource> self,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			Check.SelfAndFunc (self, func);

			return CreateAggregateReverseHistoryIterator (self, seed, func);
		}

		private static IEnumerable<TAccumulate> CreateAggregateReverseHistoryIterator<TSource, TAccumulate> (IEnumerable<TSource> self,
			TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
		{
			IList<TSource> s = GetList (self);

			TAccumulate folded;
			yield return (folded = seed);
			for (int i = s.Count-1; i >= 0; --i)
				yield return (folded = func (folded, s [i]));
		}

		public static IEnumerable<TResult> AggregateReverseHistory<TSource, TAccumulate, TResult> (this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			Check.SelfAndFunc (self, func);
			if (resultSelector == null)
				throw new ArgumentNullException ("resultSelector");

			return CreateAggregateReverseHistoryIterator (self, seed, func, resultSelector);
		}

		private static IEnumerable<TResult> CreateAggregateReverseHistoryIterator<TSource, TAccumulate, TResult> (IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
		{
			IList<TSource> s = GetList (self);

			var result = seed;
			yield return resultSelector (result);
			for (int i = s.Count-1; i >= 0; --i)
				yield return resultSelector (result = func (result, s [i]));
		}

		#endregion
		
		// Haskell: mapAccumL
		public static Tuple<TAccumulate, List<TResult>> SelectAggregated<TSource, TAccumulate, TResult> (this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, Tuple<TAccumulate,TResult>> func)
		{
			Check.SelfAndFunc (self, func);

			ICollection<TSource> cself = self as ICollection<TSource>;
			var aggregates = cself != null 
				?  new List<TResult> (cself.Count)
				:  new List<TResult> ();

			var result = seed;
			foreach (var element in self) {
				var r = func (result, element);
				result = r._1;
				aggregates.Add (r._2);
			}

			return new Tuple<TAccumulate, List<TResult>> (result, aggregates);
		}

		// Haskell: mapAccumR
		public static Tuple<TAccumulate, List<TResult>> SelectReverseAggregated<TSource, TAccumulate, TResult> (this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, Tuple<TAccumulate,TResult>> func)
		{
			Check.SelfAndFunc (self, func);

			var s = GetList (self);
			var aggregates = new List<TResult> (s.Count);
			var result = seed;

			for (int i = s.Count-1; i >= 0; --i) {
				var r = func (result, s [i]);
				result = r._1;
				aggregates.Add (r._2);
			}

			return new Tuple<TAccumulate, List<TResult>> (result, aggregates);
		}

		// Haskell: cycle
		public static IEnumerable<TSource> Cycle<TSource> (this IEnumerable<TSource> self)
		{
			Check.Self (self);
			return CreateCycleIterator (self);
		}

		private static IEnumerable<TSource> CreateCycleIterator<TSource> (IEnumerable<TSource> self)
		{
			while (true)
				foreach (var e in self)
					yield return e;
		}

		// Haskell: splitAt
		public static Tuple<IEnumerable<TSource>, IEnumerable<TSource>> SplitAt<TSource> (this IEnumerable<TSource> self, int firstLength)
		{
			Check.Self (self);
			if (firstLength <= 0)
				firstLength = 0;
			return Tuple.Create (self.Take (firstLength), self.Skip (firstLength));
		}

		// Haskell: span
		public static Tuple<IEnumerable<TSource>, IEnumerable<TSource>> Span<TSource> (this IEnumerable<TSource> self, Func<TSource, bool> func)
		{
			Check.SelfAndFunc (self, func);

			return Tuple.Create (self.TakeWhile (func), self.SkipWhile (func));
		}

		// Haskell: break
		public static Tuple<IEnumerable<TSource>, IEnumerable<TSource>> Break<TSource> (this IEnumerable<TSource> self, Func<TSource, bool> func)
		{
			Check.SelfAndFunc (self, func);

			return Tuple.Create (self.TakeWhile (e => !func (e)), self.SkipWhile (e => !func (e)));
		}

		// Haskell: stripPrefix
		public static IEnumerable<TSource> SkipPrefix<TSource> (this IEnumerable<TSource> self, IEnumerable<TSource> prefix)
		{
			Check.Self (self);
			if (prefix == null)
				throw new ArgumentNullException ("prefix");

			using (IEnumerator<TSource> s = self.GetEnumerator ())
			using (IEnumerator<TSource> p = prefix.GetEnumerator ()) {
				int c = 0;
				bool have_s = s.MoveNext(), have_p = p.MoveNext(), have_match = true;
				do {
					++c;
					if ((have_p && !have_s) || !EqualityComparer<TSource>.Default.Equals (s.Current, p.Current)) {
						have_match = false;
						break;
					}
				} while ((have_s = s.MoveNext ()) && (have_p = p.MoveNext ()));
				if (have_match)
					return self.Skip (c);
				return null;
			}
		}

		// Haskell: group
		public static IEnumerable<IEnumerable<TSource>> HaskellGroup<TSource> (this IEnumerable<TSource> self)
		{
			return HaskellGroupBy (self, (a, b) => EqualityComparer<TSource>.Default.Equals (a, b));
		}

		// Haskell: inits
		public static IEnumerable<IEnumerable<TSource>> InitialSegments<TSource> (this IEnumerable<TSource> self)
		{
			Check.Self (self);

			return CreateInitialSegmentsIterator (self);
		}

		private static IEnumerable<IEnumerable<TSource>> CreateInitialSegmentsIterator<TSource> (IEnumerable<TSource> self)
		{
			var e = new List<TSource> ();
			yield return e;
			using (IEnumerator<TSource> s = self.GetEnumerator ()) {
				while (s.MoveNext ()) {
					e.Add (s.Current);
					yield return e;
				}
			}
		}

		// Haskell: tails
		public static IEnumerable<IEnumerable<TSource>> TrailingSegments<TSource> (this IEnumerable<TSource> self)
		{
			Check.Self (self);

			return CreateTrailingSegmentsIterator (self);
		}

		private static IEnumerable<IEnumerable<TSource>> CreateTrailingSegmentsIterator<TSource> (IEnumerable<TSource> self)
		{
			var e = self.ToList ();
			yield return e;
			while (e.Count > 0) {
				e.RemoveAt (e.Count-1);
				yield return e;
			}
		}

		// Haskell: groupBy
		public static IEnumerable<IEnumerable<TSource>> HaskellGroupBy<TSource> (this IEnumerable<TSource> self, Func<TSource, TSource, bool> func)
		{
			Check.SelfAndFunc (self, func);

			return CreateHaskellGroupByIterator (self, func);
		}

		private static IEnumerable<IEnumerable<TSource>> CreateHaskellGroupByIterator<TSource> (IEnumerable<TSource> self, Func<TSource, TSource, bool> func)
		{
			using (IEnumerator<TSource> s = self.GetEnumerator ()) {
				var e = new List<TSource> ();
				while (s.MoveNext ()) {
					if (e.Count == 0)
						e.Add (s.Current);
					else if (func (e [0], s.Current))
						e.Add (s.Current);
					else {
						yield return e;
						e.Clear ();
						e.Add (s.Current);
					}
				}
				if (e.Count > 0)
					yield return e;
			}
		}
	}
}
