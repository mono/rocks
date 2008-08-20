//
// CurryTest.cs
//
// Author:
//   Jonathan Pryor
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

using NUnit.Framework;

using Mono.Rocks;

#pragma warning disable 0219

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class CurryTest : BaseRocksFixture {

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A1_P1_SelfNull ()
		{
			Action<int> a = null;
			Action b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A1_T1_ValuesNull ()
		{
			Action<int> a = x => {};
			Tuple<int>  v = null;
			Action b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F1_P1_SelfNull ()
		{
			Func<int, int> a = null;
			Func<int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F1_T1_ValuesNull ()
		{
			Func<int, int> a = x => x;
			Tuple<int>     v = null;
			Func<int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A2_P1_SelfNull ()
		{
			Action<int, int> a = null;
			Action<int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A2_T1_ValuesNull ()
		{
			Action<int, int> a = (x,y) => {};
			Tuple<int>       v = null;
			Action<int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A2_P2_SelfNull ()
		{
			Action<int, int> a = null;
			Action b = a.Curry (1, 2);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A2_T2_ValuesNull ()
		{
			Action<int, int> a = (x, y) => {};
			Tuple<int, int>  v = null;
			Action b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F2_P1_SelfNull ()
		{
			Func<int, int, int> a = null;
			Func<int, int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F2_T1_ValuesNull ()
		{
			Func<int, int, int> a = (x,y) => x;
			Tuple<int>          v = null;
			Func<int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F2_P2_SelfNull ()
		{
			Func<int, int, int> a = null;
			Func<int> b = a.Curry (1, 2);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F2_T2_ValuesNull ()
		{
			Func<int, int, int> a = (x, y) => x;
			Tuple<int, int>     v = null;
			Func<int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A3_P1_SelfNull ()
		{
			Action<int, int, int> a = null;
			Action<int, int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A3_T1_ValuesNull ()
		{
			Action<int, int, int> a = (x,y,z) => {};
			Tuple<int>            v = null;
			Action<int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A3_P2_SelfNull ()
		{
			Action<int, int, int> a = null;
			Action<int> b = a.Curry (1, 2);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A3_T2_ValuesNull ()
		{
			Action<int, int, int> a = (x, y, z) => {};
			Tuple<int, int>       v = null;
			Action<int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A3_P3_SelfNull ()
		{
			Action<int, int, int> a = null;
			Action b = a.Curry (1, 2, 3);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A3_T3_ValuesNull ()
		{
			Action<int, int, int> a = (x, y, z) => {};
			Tuple<int, int, int>  v = null;
			Action b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F3_P1_SelfNull ()
		{
			Func<int, int, int, int> a = null;
			Func<int, int, int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F3_T1_ValuesNull ()
		{
			Func<int, int, int, int> a = (x, y, z) => x;
			Tuple<int>               v = null;
			Func<int, int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F3_P2_SelfNull ()
		{
			Func<int, int, int, int> a = null;
			Func<int, int> b = a.Curry (1, 2);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F3_T2_ValuesNull ()
		{
			Func<int, int, int, int> a = (x, y, z) => x;
			Tuple<int, int>          v = null;
			Func<int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F3_P3_SelfNull ()
		{
			Func<int, int, int, int> a = null;
			Func<int> b = a.Curry (1, 2, 3);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F3_T3_ValuesNull ()
		{
			Func<int, int, int, int> a = (x, y, z) => x;
			Tuple<int, int, int>     v = null;
			Func<int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_P1_SelfNull ()
		{
			Action<int, int, int, int> a = null;
			Action<int, int, int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_T1_ValuesNull ()
		{
			Action<int, int, int, int> a = (w, x, y, z) => {};
			Tuple<int>                 v = null;
			Action<int, int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_P2_SelfNull ()
		{
			Action<int, int, int, int> a = null;
			Action<int, int> b = a.Curry (1, 2);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_T2_ValuesNull ()
		{
			Action<int, int, int, int> a = (w, x, y, z) => {};
			Tuple<int, int>            v = null;
			Action<int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_P3_SelfNull ()
		{
			Action<int, int, int, int> a = null;
			Action<int> b = a.Curry (1, 2, 3);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_T3_ValuesNull ()
		{
			Action<int, int, int, int> a = (w, x, y, z) => {};
			Tuple<int, int, int>       v = null;
			Action<int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_P4_SelfNull ()
		{
			Action<int, int, int, int> a = null;
			Action b = a.Curry (1, 2, 3, 4);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_A4_T4_ValuesNull ()
		{
			Action<int, int, int, int> a = (w, x, y, z) => {};
			Tuple<int, int, int, int>  v = null;
			Action b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_P1_SelfNull ()
		{
			Func<int, int, int, int, int> a = null;
			Func<int, int, int, int> b = a.Curry (1);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_T1_ValuesNull ()
		{
			Func<int, int, int, int, int> a = (w, x, y, z) => w;
			Tuple<int>                    v = null;
			Func<int, int, int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_P2_SelfNull ()
		{
			Func<int, int, int, int, int> a = null;
			Func<int, int, int> b = a.Curry (1, 2);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_T2_ValuesNull ()
		{
			Func<int, int, int, int, int> a = (w, x, y, z) => w;
			Tuple<int, int>               v = null;
			Func<int, int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_P3_SelfNull ()
		{
			Func<int, int, int, int, int> a = null;
			Func<int, int> b = a.Curry (1, 2, 3);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_T3_ValuesNull ()
		{
			Func<int, int, int, int, int> a = (w, x, y, z) => w;
			Tuple<int, int, int>          v = null;
			Func<int, int> b = a.Curry (v);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_P4_SelfNull ()
		{
			Func<int, int, int, int, int> a = null;
			Func<int> b = a.Curry (1, 2, 3, 4);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Curry_F4_T4_ValuesNull ()
		{
			Func<int, int, int, int, int> a = (w, x, y, z) => w;
			Tuple<int, int, int, int>     v = null;
			Func<int> b = a.Curry (v);
		}
	}
}
