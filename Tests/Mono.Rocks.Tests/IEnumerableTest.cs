﻿//
// IEnumerableTest.cs
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

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class IEnumerableTest : BaseRocksFixture {

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Implode_SourceNull ()
		{
			IEnumerable<int> e = null;
			e.Implode ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Implode_SelectorNull ()
		{
			IEnumerable<int> e = new int[0];
			Func<int, string> f = null;
			e.Implode (null, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Implode_AppenderNull ()
		{
			IEnumerable<int>            e = new int[0];
			Action<StringBuilder, int>  a = null;
			e.Implode (null, a);
		}

		[Test]
		public void Implode ()
		{
			var data = new [] { 0, 1, 2, 3, 4, 5 };
			var result = "0, 1, 2, 3, 4, 5";

			Assert.AreEqual ("", new string[]{}.Implode (", "));
			Assert.AreEqual (result, data.Implode (", "));
			Assert.AreEqual (
					"'foo', 'bar'",
					new[]{"foo", "bar"}.Implode (", ", (b, e) => {b.Append ("'").Append (e).Append ("'");}));
			Assert.AreEqual (
					"'foo', 'bar'",
					new[]{"foo", "bar"}.Implode (", ", e => "'" + e + "'"));
		}

		[Test]
		public void ImplodeEmpty ()
		{
			var data = new int [] {};

			Assert.AreEqual (string.Empty, data.Implode ());
		}

		[Test]
		public void Repeat ()
		{
			Assert.AreEqual ("foofoofoo", new [] {"foo"}.Repeat (3).Implode ());
			Assert.AreEqual ("foobarfoobar", new [] {"foo", "bar"}.Repeat (2).Implode ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Repeat_SelfNull ()
		{
			IEnumerable<string> e = null;
			e.Repeat (0);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void PathCombine_SelfNull ()
		{
			string [] data = null;
			data.PathCombine ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void PathCombine_Null ()
		{
			var data = new [] { "a", "b", null, "c" };
			data.PathCombine ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void PathCombine_InvalidPathChars ()
		{
			// 0x00 (NULL) is the only cross-platform invalid path character
			var data = new [] { "a", "b\x00", "c" };
			data.PathCombine ();
		}

		[Test]
		public void PathCombine ()
		{
			var data = new [] {"a", "b", "c"};
			var result = string.Format ("a{0}b{0}c", Path.DirectorySeparatorChar);
			Assert.AreEqual (result, data.PathCombine ());

			data = new [] { "a", String.Empty, "b", "c" };
			Assert.AreEqual (result, data.PathCombine (), "empty elemetns");

			string rooted = Path.DirectorySeparatorChar + "d";
			data = new [] { "a", rooted };
			Assert.AreEqual (rooted, data.PathCombine (), "rooted path2");

			data = new [] { "a", "b", rooted, "c" };
			string expected = Path.Combine (Path.Combine (Path.Combine ("a", "b"), rooted), "c");
			Assert.AreEqual (expected, data.PathCombine (), "rooted path2 (complex)");

			string end1 = "d" + Path.DirectorySeparatorChar;
			data = new [] { rooted, end1, "e" };
			expected = Path.Combine (Path.Combine (rooted, end1), "e");
			Assert.AreEqual (expected, data.PathCombine (), "DirectorySeparatorChar");

			string end2 = "d" + Path.AltDirectorySeparatorChar;
			data = new [] { rooted, end2, "f" };
			expected = Path.Combine (Path.Combine (rooted, end2), "f");
			Assert.AreEqual (expected, data.PathCombine (), "AltDirectorySeparatorChar");

			data = new [] { "a" };
			Assert.AreEqual (Path.Combine ("a", String.Empty), data.PathCombine (), "single string");

			data = new [] { String.Empty };
			Assert.AreEqual (Path.Combine (String.Empty, String.Empty), data.PathCombine (), "single empty string");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Apply_SelfNull ()
		{
			IEnumerable<char> e = null;
			e.Apply ();
		}

		[Test]
		public void Apply ()
		{
			int count = 0;
			"hello".Select (c => ++count).Apply ();
			Assert.AreEqual (5, count);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ApplyAction_SelfNull ()
		{
			IEnumerable<char> e = null;
			e.Apply (c => Console.WriteLine (c));
		}

		[Test]
		public void ApplyAction ()
		{
			int count = 0;
			Enumerable.Range (0, 10).Apply (n => ++count).Apply ();
			Assert.AreEqual (10, count);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ApplyPairs_SelfNull ()
		{
			IEnumerable<char> e = null;
			e.ApplyPairs ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ApplyPairs_ActionsNull ()
		{
			IEnumerable<char> e = "a";
			e.ApplyPairs (null);
		}

		[Test]
		public void ApplyPairs ()
		{
			string s = null;
			int n = 0;
			double d = 0;
			"word 10 10.5".Words().ApplyPairs (
					v => s = v,
					v => n = int.Parse (v),
					v => d = double.Parse (v)
			).Apply ();
			Assert.AreEqual ("word", s);
			Assert.AreEqual (10, n);
			Assert.AreEqual (10.5, d);
		}

		[Test]
		public void Convert ()
		{
			string s;
			DateTime c;
			double d;
			int n;

			"a 1970-01-01 2 3.14".Words().Convert (out s);
			Assert.AreEqual ("a", s);

			"a 1970-01-01 2 3.14".Words().Convert (out s, out c);
			Assert.AreEqual ("a", s);
			Assert.AreEqual (new DateTime (1970, 1, 1), c);

			"a 1970-01-01 2 3.14".Words().Convert (out s, out c, out n);
			Assert.AreEqual ("a", s);
			Assert.AreEqual (new DateTime (1970, 1, 1), c);
			Assert.AreEqual (2, n);

			"a 1970-01-01 2 3.14".Words().Convert (out s, out c, out n, out d);
			Assert.AreEqual ("a", s);
			Assert.AreEqual (new DateTime (1970, 1, 1), c);
			Assert.AreEqual (2, n);
			Assert.AreEqual (3.14, d);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Sort_SelfNull ()
		{
			IEnumerable<int> e = null;
			e.Sort ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Sort_SelfNull_Comparison ()
		{
			IEnumerable<int> e = null;
			Comparison<int> c = (x,y) => 0;
			e.Sort (c);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Sort_SelfNull_Comparer ()
		{
			IEnumerable<int> e = null;
			IComparer<int> c = Comparer<int>.Default;
			e.Sort (c);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Sort_Comparison_Null ()
		{
			IEnumerable<int> e = new[]{1};
			Comparison<int> c = null;
			e.Sort (c);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Sort_Comparer_Null ()
		{
			IEnumerable<int> e = new[]{1};
			IComparer<int> c = null;
			e.Sort (c);
		}

		[Test]
		public void Sort ()
		{
			Assert.AreEqual (new[]{4, 3, 2, 1}.Sort ().Implode (), "1234");
			Assert.AreEqual (new[]{1, 2, 3, 4}.Sort ((x,y) => x == y ? 0 : x < y ? 1 : -1).Implode (), "4321");
			Assert.AreEqual (new[]{2, 4, 1, 3}.Sort (Comparer<int>.Default).Implode (), "1234");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void OrderByNatural_SelfNull ()
		{
			IEnumerable<int> e = null;
			e.OrderByNatural (x => x.ToString ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void OrderByNatural_FuncNull ()
		{
			IEnumerable<int> e = new[]{1};
			Func<int,string> f = null;
			e.OrderByNatural (f);
		}

		[Test]
		public void OrderByNatural ()
		{
			string[] expected = {
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.11",
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.12",
				"bar",
				"foo",
				"foo",
				"foo1",
				"foo2",
				"foo3",
				"foo4",
				"foo5",
				"foo6",
				"foo7",
				"foo8",
				"foo9",
				"foo10",
			};
			IEnumerable<string> actual = new[]{
				"foo",
				"foo",
				"foo10",
				"foo1",
				"foo4",
				"foo2",
				"foo3",
				"foo9",
				"foo5",
				"foo7",
				"foo8",
				"foo6",
				"bar",
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.12",
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.11",
			}.OrderByNatural (s => s);

			AssertAreSame (expected, actual);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SortNatural_SelfNull ()
		{
			IEnumerable<string> e = null;
			e.SortNatural ();
		}

		[Test]
		public void SortNatural ()
		{
			string[] expected = {
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.11",
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.12",
				"bar",
				"foo",
				"foo",
				"foo1",
				"foo2",
				"foo3",
				"foo4",
				"foo5",
				"foo6",
				"foo7",
				"foo8",
				"foo9",
				"foo10",
			};
			IEnumerable<string> actual = new[]{
				"foo",
				"foo",
				"foo10",
				"foo1",
				"foo4",
				"foo2",
				"foo3",
				"foo9",
				"foo5",
				"foo7",
				"foo8",
				"foo6",
				"bar",
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.12",
				"a.1.b.2.c.3.d.4.e.5.f.6.g.7.h.8.i.9.j.10.k.11",
			}.SortNatural ();

			AssertAreSame (expected, actual);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach2_Source1Null ()
		{
			IEnumerable<int> s1 = null;
			IEnumerable<int> s2 = new[]{2};
			s1.SelectFromEach (s2, (a,b) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach2_Source2Null ()
		{
			IEnumerable<int> s2 = null;
			new[]{1}.SelectFromEach (s2, (a,b) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach2_SelectorNull ()
		{
			IEnumerable<int> s2 = new[]{2};
			Func<int,int,string> f = null;
			new[]{1}.SelectFromEach (s2, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach3_Source1Null ()
		{
			IEnumerable<int> e = null;
			IEnumerable<int> s2 = new[]{1};
			IEnumerable<int> s3 = new[]{2};
			e.SelectFromEach (s2, s3, (a,b,c) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach3_Source2Null ()
		{
			IEnumerable<int> s2 = null;
			IEnumerable<int> s3 = new[]{2};
			new[]{1}.SelectFromEach (s2, s3, (a,b,c) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach3_Source3Null ()
		{
			IEnumerable<int> s2 = new[]{2};
			IEnumerable<int> s3 = null;
			new[]{1}.SelectFromEach (s2, s3, (a,b,c) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach3_SelectorNull ()
		{
			IEnumerable<int> s2 = new[]{2};
			IEnumerable<int> s3 = new[]{3};
			Func<int,int,int,string> f = null;
			new[]{1}.SelectFromEach (s2, s3, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach4_Source1Null ()
		{
			IEnumerable<int> s1 = null;
			IEnumerable<int> s2 = new[]{2};
			IEnumerable<int> s3 = new[]{3};
			IEnumerable<int> s4 = new[]{4};
			s1.SelectFromEach (s2, s3, s4, (a,b,c,d) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach4_Source2Null ()
		{
			IEnumerable<int> s2 = null;
			IEnumerable<int> s3 = new[]{3};
			IEnumerable<int> s4 = new[]{4};
			new[]{1}.SelectFromEach (s2, s3, s4, (a,b,c,d) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach4_Source3Null ()
		{
			IEnumerable<int> s2 = new[]{2};
			IEnumerable<int> s3 = null;
			IEnumerable<int> s4 = new[]{4};
			new[]{1}.SelectFromEach (s2, s3, s4, (a,b,c,d) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach4_Source4Null ()
		{
			IEnumerable<int> s2 = new[]{2};
			IEnumerable<int> s3 = new[]{3};
			IEnumerable<int> s4 = null;
			new[]{1}.SelectFromEach (s2, s3, s4, (a,b,c,d) => "");
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectFromEach4_SelectorNull ()
		{
			IEnumerable<int> s2 = new[]{2};
			IEnumerable<int> s3 = new[]{3};
			IEnumerable<int> s4 = new[]{4};
			Func<int,int,int,int,string> f = null;
			new[]{1}.SelectFromEach (s2, s3, s4, f);
		}

		[Test]
		public void SelectFromEach ()
		{
			List<int>  a = new List<int> {1, 2, 3, 4};
			List<char> b = new List<char> {'a', 'b', 'c', 'd', 'e'};
			var c = a.SelectFromEach (b, (x, y) => new { First = x, Second = y }).ToList ();
			Assert.AreEqual (4, c.Count);
			Assert.AreEqual (1,   c [0].First);
			Assert.AreEqual ('a', c [0].Second);
			Assert.AreEqual (2,   c [1].First);
			Assert.AreEqual ('b', c [1].Second);
			Assert.AreEqual (3,   c [2].First);
			Assert.AreEqual ('c', c [2].Second);
			Assert.AreEqual (4,   c [3].First);
			Assert.AreEqual ('d', c [3].Second);

			Assert.AreEqual ("123",
					new[]{1}.SelectFromEach (new[]{2}, new[]{3}, 
						(x,y,z) => x.ToString () + y.ToString () + z.ToString ()).Implode ());
			Assert.AreEqual ("1234",
					new[]{1}.SelectFromEach (new[]{2}, new[]{3}, new[]{4},
						(w,x,y,z) => w.ToString () + x.ToString () + y.ToString () + z.ToString ()).Implode ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ExceptLast_SelfNull ()
		{
			IEnumerable<char> e = null;
			e.ExceptLast (1);
		}

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void ExceptLast_NegativeLast ()
		{
			new[]{'a'}.ExceptLast (-1);
		}

		[Test]
		public void ExceptLast ()
		{
			Assert.AreEqual ("1234", new[]{1,2,3,4}.ExceptLast(0).Implode ());
			Assert.AreEqual ("123",  new[]{1,2,3,4}.ExceptLast(1).Implode ());
			Assert.AreEqual ("12",   new[]{1,2,3,4}.ExceptLast(2).Implode ());
			Assert.AreEqual ("1",    new[]{1,2,3,4}.ExceptLast(3).Implode ());
			Assert.AreEqual ("",     new[]{1,2,3,4}.ExceptLast(4).Implode ());
			Assert.AreEqual ("",     new[]{1,2,3,4}.ExceptLast(5).Implode ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Intersperse_SelfNull ()
		{
			IEnumerable<char> e = null;
			e.Intersperse ('b');
		}

		[Test]
		public void Intersperse ()
		{
			Assert.AreEqual ("1929394", new[]{1,2,3,4}.Intersperse (9).Implode ());
			Assert.AreEqual ("a.z",     new[]{'a','z'}.Intersperse ('.').Implode ());
#if BNC_400716
			IEnumerable<IEnumerable<char>> e = new char[][]{ 
				new char[]{'b', 'c', 'd'}, 
				new char[]{'e', 'f', 'g'},
			};
			IEnumerable<char> x = new char[]{'a', 'a'};
			Assert.AreEqual ("bcdaaefg", e.Intersperse (x).Implode ());
#endif
		}

		static IEnumerable<char> aa ()
		{
			yield return 'a';
			yield return 'a';
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Transpose_SelfNull ()
		{
			IEnumerable<IEnumerable<char>> e = null;
			e.Transpose ();
		}

		[Test]
		public void Transpose ()
		{
			IEnumerable<IEnumerable<int>> a = new int[][]{
				new int[]{1, 2, 3},
				new int[]{4, 5, 6},
			};
			IEnumerable<IEnumerable<int>> b = a.Transpose ();
			List<List<int>> c = b.ToList ();
			Assert.AreEqual (3, c.Count);
			Assert.AreEqual (2, c [0].Count);
			Assert.AreEqual (2, c [1].Count);
			Assert.AreEqual (2, c [2].Count);
			Assert.AreEqual (1, c [0][0]);
			Assert.AreEqual (4, c [0][1]);
			Assert.AreEqual (2, c [1][0]);
			Assert.AreEqual (5, c [1][1]);
			Assert.AreEqual (3, c [2][0]);
			Assert.AreEqual (6, c [2][1]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ToList_SelfNull ()
		{
			IEnumerable<IEnumerable<char>> e = null;
			e.ToList ();
		}

		[Test]
		public void ToList ()
		{
#if BNC_400716
			int[][] a = new int[][]{
				new int[]{1, 2, 3},
				new int[]{4, 5, 6},
			};
			IEnumerable<IEnumerable<int>> b = a;
			List<List<int>> c = b.ToList ();
			Assert.AreEqual (a.Length, c.Count);
			Assert.AreEqual (a [0].Length, c [0].Count);
			Assert.AreEqual (a [1].Length, c [1].Count);
			Assert.AreEqual (a [0][0], c [0][0]);
			Assert.AreEqual (a [0][1], c [0][1]);
			Assert.AreEqual (a [0][2], c [0][2]);
			Assert.AreEqual (a [1][0], c [1][0]);
			Assert.AreEqual (a [1][1], c [1][1]);
			Assert.AreEqual (a [1][2], c [1][2]);
#endif
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SF_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverse (f);
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void AggregateReverse_SF_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverse (f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SF_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			s.AggregateReverse (f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SSF_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverse (0, f);
		}

		[Test]
		public void AggregateReverse_SSF_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverse (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SSF_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			s.AggregateReverse (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SSFR_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = x => x;
			s.AggregateReverse (0, f, r);
		}

		[Test]
		public void AggregateReverse_SSFR_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = x => x;
			s.AggregateReverse (0, f, r);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SSFR_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			Func<int,int>     r = x => x;
			s.AggregateReverse (0, f, r);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverse_SSFR_ResultNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = null;
			s.AggregateReverse (0, f, r);
		}

		[Test]
		public void AggregateReverse ()
		{
			IEnumerable<int> s = new []{1, 2, 3, 4, 5};
			Assert.AreEqual (-5, s.AggregateReverse ((a,b) => a - b));
			Assert.AreEqual ("54321",
					s.AggregateReverse (new StringBuilder (), (a,b) => a.Append (b)).ToString ());
			Assert.AreEqual (1,
					new int[]{}.AggregateReverse (1, (a,b) => a+b));
			Assert.AreEqual ("54321",
					s.AggregateReverse (new StringBuilder (), (a,b) => a.Append (b), a => a.ToString ()));
			Assert.AreEqual (1,
					new int[]{}.AggregateReverse (1, (a,b) => a+b, x => x));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Concat_SourceNull ()
		{
			IEnumerable<int> s = null;
			IEnumerable<IEnumerable<int>> ss = new []{new []{1}};
			s.Concat (ss);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Concat_SourceesNull ()
		{
			IEnumerable<int> s = new[]{1};
			IEnumerable<IEnumerable<int>> ss = null;
			s.Concat (ss);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Concat_Params_SourceNull ()
		{
			IEnumerable<int> s = null;
			s.Concat (new[]{1});
		}

		[Test]
		public void Concat ()
		{
			IEnumerable<int> s = new[]{1};
			Assert.AreEqual ("1234567",
					s.Concat (new[]{2, 3}, new[]{4, 5}, new[]{6, 7}).Implode ());
#if BNC_400716
			IEnumerable<IEnumerable<int>> ss = new []{
				new[]{2,3},
				new[]{4,5},
				new[]{6,7},
			};
			Assert.AreEqual ("1234567", s.Concat (ss).Implode ());
#endif
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void And_SourceNull ()
		{
			IEnumerable<bool> s = null;
			s.And ();
		}

		[Test]
		public void And ()
		{
			Assert.AreEqual (false,
					new[]{true, false, true, true}.And ());
			Assert.AreEqual (false,
					new[]{false, false, false, false}.And ());
			Assert.AreEqual (true,
					new[]{true, true, true, true}.And ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Or_SourceNull ()
		{
			IEnumerable<bool> s = null;
			s.Or ();
		}

		[Test]
		public void Or ()
		{
			Assert.AreEqual (true,
					new[]{true, false, true, true}.Or ());
			Assert.AreEqual (false,
					new[]{false, false, false, false}.Or ());
			Assert.AreEqual (true,
					new[]{true, true, true, true}.Or ());
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SF_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateHistory (f);
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void AggregateHistory_SF_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			// need .Apply() as check for empty source is delayed until enumeration.
			s.AggregateHistory (f).Apply ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SF_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			s.AggregateHistory (f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SSF_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateHistory (0, f);
		}

		[Test]
		public void AggregateHistory_SSF_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateHistory (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SSF_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			s.AggregateHistory (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SSFR_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = x => x;
			s.AggregateHistory (0, f, r);
		}

		[Test]
		public void AggregateHistory_SSFR_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = x => x;
			s.AggregateHistory (0, f, r);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SSFR_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			Func<int,int>     r = x => x;
			s.AggregateHistory (0, f, r);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateHistory_SSFR_ResultNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = null;
			s.AggregateHistory (0, f, r);
		}

		[Test]
		public void AggregateHistory ()
		{
			IEnumerable<int> s = new []{1, 2, 3, 4, 5};
			Assert.AreEqual (
					"1,-1,-4,-8,-13",
					s.AggregateHistory ((a,b) => a - b).Implode (","));
			Assert.AreEqual (
					",1,12,123,1234,12345",
					s.AggregateHistory (new StringBuilder (), (a,b) => a.Append (b)).Implode (","));
			Assert.AreEqual ("1",
					new int[]{}.AggregateHistory (1, (a,b) => a+b).Implode (","));
			Assert.AreEqual (
					"R,R1,R12,R123,R1234,R12345",
					s.AggregateHistory (new StringBuilder (), 
						(a,b) => a.Append (b), 
						a => "R" + a.ToString ()).Implode (","));
			Assert.AreEqual ("1",
					new int[]{}.AggregateHistory (1, (a,b) => a+b, x => x).Implode (","));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SF_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverseHistory (f);
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void AggregateReverseHistory_SF_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			// need .Apply() as check for empty source is delayed until enumeration.
			s.AggregateReverseHistory (f).Apply ();
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SF_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			s.AggregateReverseHistory (f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SSF_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverseHistory (0, f);
		}

		[Test]
		public void AggregateReverseHistory_SSF_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			s.AggregateReverseHistory (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SSF_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			s.AggregateReverseHistory (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SSFR_SourceNull ()
		{
			IEnumerable<int>  s = null;
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = x => x;
			s.AggregateReverseHistory (0, f, r);
		}

		[Test]
		public void AggregateReverseHistory_SSFR_SourceEmpty ()
		{
			IEnumerable<int>  s = new int[]{};
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = x => x;
			s.AggregateReverseHistory (0, f, r);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SSFR_FuncNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = null;
			Func<int,int>     r = x => x;
			s.AggregateReverseHistory (0, f, r);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void AggregateReverseHistory_SSFR_ResultNull ()
		{
			IEnumerable<int>  s = new[]{1};
			Func<int,int,int> f = (a,b) => a-b;
			Func<int,int>     r = null;
			s.AggregateReverseHistory (0, f, r);
		}

		[Test]
		public void AggregateReverseHistory ()
		{
			IEnumerable<int> s = new []{1, 2, 3, 4, 5};
			Assert.AreEqual (
					"5,1,-2,-4,-5",
					s.AggregateReverseHistory ((a,b) => a - b).Implode (","));
			Assert.AreEqual (
					",5,54,543,5432,54321",
					s.AggregateReverseHistory (new StringBuilder (), (a,b) => a.Append (b)).Implode (","));
			Assert.AreEqual ("1",
					new int[]{}.AggregateReverseHistory (1, (a,b) => a+b).Implode (","));
			Assert.AreEqual (
					"R,R5,R54,R543,R5432,R54321",
					s.AggregateReverseHistory (new StringBuilder (), 
						(a,b) => a.Append (b), 
						a => "R" + a.ToString ()).Implode (","));
			Assert.AreEqual ("1",
					new int[]{}.AggregateReverseHistory (1, (a,b) => a+b, x => x).Implode (","));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectAggregated_SelfNull ()
		{
			IEnumerable<int>               s  = null;
			Func<int, int, Tuple<int,int>> f  = (x,y) => Tuple.Create (x, y);
			s.SelectAggregated (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectAggregated_FuncNull ()
		{
			IEnumerable<int>                      s  = new[]{1};
			Func<int, int, Tuple<int,int>>        f  = null;
			s.SelectAggregated (0, f);
		}

		[Test]
		public void SelectAggregated ()
		{
			IEnumerable<int> s = new []{2, 3, 4, 5};
			Assert.AreEqual (
					"-13:s-1,s-4,s-8,s-13",
					s.SelectAggregated (1, 
						(a,b) => Tuple.Create (a-b, "s" + (a-b)))
					.Aggregate ((r, l) => r + ":" + l.Implode (",")));
			Assert.AreEqual (
					"42,0",
					new int[]{}.SelectAggregated (42, 
						(a,b) => Tuple.Create (a-b, b))
					.Aggregate ((r, l) => r + "," + l.Count));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectReverseAggregated_SelfNull ()
		{
			IEnumerable<int>                s  = null;
			Func<int, int, Tuple<int,int>>  f  = (x,y) => Tuple.Create (x, y);
			s.SelectReverseAggregated (0, f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SelectReverseAggregated_FuncNull ()
		{
			IEnumerable<int>                s  = new[]{1};
			Func<int, int, Tuple<int,int>>  f  = null;
			s.SelectReverseAggregated (0, f);
		}

		[Test]
		public void SelectReverseAggregated ()
		{
			IEnumerable<int> s = new []{1, 2, 3, 4};
			Assert.AreEqual (
					"-5:s1,s-2,s-4,s-5",
					s.SelectReverseAggregated (5, 
						(a,b) => Tuple.Create (a-b, "s" + (a-b)))
					.Aggregate ((r, l) => r + ":" + l.Implode (",")));
			Assert.AreEqual (
					"42,0",
					new int[]{}.SelectReverseAggregated (42, 
						(a,b) => Tuple.Create (a-b, b))
					.Aggregate ((r, l) => r + "," + l.Count));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Cycle_SelfNull ()
		{
			IEnumerable<int> s = null;
			s.Cycle ();
		}

		[Test]
		public void Cycle ()
		{
			// not entirely sure how you sanely test an infinite list...
			var x = new[]{1};
			Assert.AreEqual ("1,1,1,1,1",
					x.Cycle ().Take (5).Implode (","));
			x = new[]{1, 2, 3};
			Assert.AreEqual ("1,2,3,1,2,3,1,2,3,1,2,3,1,2,3",
					x.Cycle ().Take (3*5).Implode (","));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SplitAt_SelfNull ()
		{
			IEnumerable<int> s = null;
			s.SplitAt (0);
		}

		[Test]
		public void SplitAt ()
		{
			Assert.AreEqual ("Hello |World!",
					"Hello World!".SplitAt (6)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("123|45",
					new[]{1,2,3,4,5}.SplitAt (3)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("1|23",
					new[]{1,2,3}.SplitAt (1)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("123|",
					new[]{1,2,3}.SplitAt (3)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("123|",
					new[]{1,2,3}.SplitAt (4)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("|123",
					new[]{1,2,3}.SplitAt (0)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("|123",
					new[]{1,2,3}.SplitAt (-1)
					.Aggregate ((x,y) => x.Implode () + "|" + y.Implode ()));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Span_SelfNull ()
		{
			IEnumerable<int> s = null;
			Func<int, bool>  p = x => true;
			s.Span (p);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Span_FuncNull ()
		{
			IEnumerable<int> s = new[]{1};
			Func<int, bool>  p = null;
			s.Span (p);
		}

		[Test]
		public void Span ()
		{
			Assert.AreEqual ("12|341234",
					new[]{1,2,3,4,1,2,3,4}.Span (e => e < 3)
					.Aggregate ((x, y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("123|",
					new[]{1,2,3}.Span (e => e < 9)
					.Aggregate ((x, y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("|123",
					new[]{1,2,3}.Span (e => e < 0)
					.Aggregate ((x, y) => x.Implode () + "|" + y.Implode ()));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Break_SelfNull ()
		{
			IEnumerable<int> s = null;
			Func<int, bool>  p = x => true;
			s.Break (p);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void Break_FuncNull ()
		{
			IEnumerable<int> s = new[]{1};
			Func<int, bool>  p = null;
			s.Break (p);
		}

		[Test]
		public void Break ()
		{
			Assert.AreEqual ("123|41234",
					new[]{1,2,3,4,1,2,3,4}.Break (e => e > 3)
					.Aggregate ((x, y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("|123",
					new[]{1,2,3}.Break (e => e < 9)
					.Aggregate ((x, y) => x.Implode () + "|" + y.Implode ()));
			Assert.AreEqual ("123|",
					new[]{1,2,3}.Break (e => e > 9)
					.Aggregate ((x, y) => x.Implode () + "|" + y.Implode ()));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SkipPrefix_SelfNull ()
		{
			IEnumerable<int> s = null;
			IEnumerable<int> p = new[]{1};
			s.SkipPrefix (p);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void SkipPrefix_PrefixNull ()
		{
			IEnumerable<int> s = new[]{1};
			IEnumerable<int> p = null;
			s.SkipPrefix (p);
		}

		[Test]
		public void SkipPrefix ()
		{
			Assert.AreEqual ("bar",
					"foobar".SkipPrefix ("foo").Implode ());
			Assert.AreEqual ("",
					"foo".SkipPrefix ("foo").Implode ());
			Assert.AreEqual (null,
					"barfoo".SkipPrefix ("foo"));
			Assert.AreEqual (null,
					"barfoobaz".SkipPrefix ("foo"));
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void HaskellGroup_SelfNull ()
		{
			IEnumerable<int> s = null;
			s.HaskellGroup ();
		}

		[Test]
		public void HaskellGroup ()
		{
			IEnumerable<IEnumerable<char>> e = "Mississippi".HaskellGroup ();
			var l = e.ToList ();
			Assert.AreEqual (8, l.Count);
			AssertAreSame (new[]{'M'},      l [0]);
			AssertAreSame (new[]{'i'},      l [1]);
			AssertAreSame (new[]{'s', 's'}, l [2]);
			AssertAreSame (new[]{'i'},      l [3]);
			AssertAreSame (new[]{'s', 's'}, l [4]);
			AssertAreSame (new[]{'i'},      l [5]);
			AssertAreSame (new[]{'p', 'p'}, l [6]);
			AssertAreSame (new[]{'i'},      l [7]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void InitialSegments_SelfNull ()
		{
			IEnumerable<int> s = null;
			s.InitialSegments ();
		}

		[Test]
		public void InitialSegments ()
		{
			IEnumerable<IEnumerable<char>> e = "abc".InitialSegments ();
			var l = e.ToList ();
			Assert.AreEqual (4, l.Count);
			AssertAreSame (new char[]{},          l [0]);
			AssertAreSame (new[]{'a'},            l [1]);
			AssertAreSame (new[]{'a', 'b'},       l [2]);
			AssertAreSame (new[]{'a', 'b', 'c'},  l [3]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void TrailingSegments_SelfNull ()
		{
			IEnumerable<int> s = null;
			s.TrailingSegments ();
		}

		[Test]
		public void TrailingSegments ()
		{
			IEnumerable<IEnumerable<char>> e = "abc".TrailingSegments ();
			var l = e.ToList ();
			Assert.AreEqual (4, l.Count);
			AssertAreSame (new[]{'a', 'b', 'c'},  l [0]);
			AssertAreSame (new[]{'a', 'b'},       l [1]);
			AssertAreSame (new[]{'a'},            l [2]);
			AssertAreSame (new char[]{},          l [3]);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void HaskellGroupBy_SelfNull ()
		{
			IEnumerable<int>      s = null;
			Func<int, int, bool>  f = (a, b) => true;
			s.HaskellGroupBy (f);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void HaskellGroupBy_FuncNull ()
		{
			IEnumerable<int>      s = new[]{1};
			Func<int, int, bool>  f = null;
			s.HaskellGroupBy (f);
		}
	}
}
