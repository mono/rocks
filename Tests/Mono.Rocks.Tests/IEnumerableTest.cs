//
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

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class IEnumerableTest : BaseRocksFixture {

		[Test]
		public void Join ()
		{
			var data = new [] { 0, 1, 2, 3, 4, 5 };
			var result = "0, 1, 2, 3, 4, 5";

			Assert.AreEqual (result, data.Join (", "));
		}

		[Test]
		public void JoinEmpty ()
		{
			var data = new int [] {};

			Assert.AreEqual (string.Empty, data.Join ());
		}

		[Test]
		public void Repeat ()
		{
			Assert.AreEqual ("foofoofoo", new [] {"foo"}.Repeat (3).Join ());
			Assert.AreEqual ("foobarfoobar", new [] {"foo", "bar"}.Repeat (2).Join ());
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
		public void SelectFromEach_SelfNull ()
		{
			IEnumerable<char> e = null;
			e.SelectFromEach<char, char, char> (null, null);
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
			Assert.AreEqual (new[]{1,2,3,4}.ExceptLast(0).Join (""), "1234");
			Assert.AreEqual (new[]{1,2,3,4}.ExceptLast(1).Join (""), "123");
			Assert.AreEqual (new[]{1,2,3,4}.ExceptLast(2).Join (""), "12");
			Assert.AreEqual (new[]{1,2,3,4}.ExceptLast(3).Join (""), "1");
			Assert.AreEqual (new[]{1,2,3,4}.ExceptLast(4).Join (""), "");
			Assert.AreEqual (new[]{1,2,3,4}.ExceptLast(5).Join (""), "");
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
			Assert.AreEqual (new[]{1,2,3,4}.Intersperse (9).Join (""), "1929394");
			Assert.AreEqual (new[]{'a','z'}.Intersperse ('.').Join (""), "a.z");
#if CRASH
			IEnumerable<IEnumerable<char>> e = new char[][]{ 
				new char[]{'b', 'c', 'd'}, 
				new char[]{'e', 'f', 'g'},
			};
			IEnumerable<char> x = new char[]{'a', 'a'};
			Assert.AreEqual (e.Intersperse (x).Join (""), "bcdaaefg");
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
			int[][] a = new int[][]{
				new int[]{1, 2, 3},
				new int[]{4, 5, 6},
			};
			IEnumerable<IEnumerable<int>> b = a;
#if CRASH
			List<List<int>> c = b.ToList ();
			Assert.AreEqual (c.Count, a.Length);
			Assert.AreEqual (c [0].Count, a [0].Length);
			Assert.AreEqual (c [1].Count, a [1].Length);
			Assert.AreEqual (c [0][0], a [0][0]);
			Assert.AreEqual (c [0][1], a [0][1]);
			Assert.AreEqual (c [0][2], a [0][2]);
			Assert.AreEqual (c [1][0], a [1][0]);
			Assert.AreEqual (c [1][1], a [1][1]);
			Assert.AreEqual (c [1][2], a [1][2]);
#endif
		}
	}
}
