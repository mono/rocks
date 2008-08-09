//
// StreamTest.cs
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using NUnit.Framework;

using Mono.Rocks;

namespace Mono.Rocks.Tests {

	[TestFixture]
	public class StreamTest : BaseRocksFixture {

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Boolean_SelfNull ()
		{
			Stream  s      = null;
			bool   value = true;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Byte_SelfNull ()
		{
			Stream  s      = null;
			byte   value = (byte) 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Char_SelfNull ()
		{
			Stream  s      = null;
			char   value = '\0';
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Double_SelfNull ()
		{
			Stream  s      = null;
			double value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Int16_SelfNull ()
		{
			Stream  s      = null;
			short  value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Int32_SelfNull ()
		{
			Stream  s      = null;
			int    value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Int64_SelfNull ()
		{
			Stream  s      = null;
			long   value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Single_SelfNull ()
		{
			Stream  s      = null;
			float  value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_UInt16_SelfNull ()
		{
			Stream  s      = null;
			ushort value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_UInt32_SelfNull ()
		{
			Stream  s      = null;
			uint   value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_UInt64_SelfNull ()
		{
			Stream  s      = null;
			ulong  value = 0;
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_TValue_SelfNull ()
		{
			Stream  s       = null;
			DateTime value = new DateTime ();
			s.Write (value);
		}

		[Test]
		public void Write ()
		{
			byte[] expected = { 
				0x1,                                            // bool
				0x1,                                            // byte
				0x61, 0x0,                                      // char
				0x1f, 0x85, 0xeb, 0x51, 0xb8, 0x1e, 0x9, 0x40,  // double
				0xa, 0x0,                                       // short
				0x14, 0x0, 0x0, 0x0,                            // int
				0x1e, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,        // long
				0x48, 0xe1, 0xa, 0x40,                          // float
				0x28, 0x0,                                      // ushort
				0x32, 0x0, 0x0, 0x0,                            // uint
				0x3c, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0         // ulong
			};

			MemoryStream ms = new MemoryStream ();
			ms.Write (true)
				.Write ((byte) 1)
				.Write ('a')
				.Write (3.14)
				.Write ((short) 10)
				.Write (20)
				.Write (30L)
				.Write (2.17F)
				.Write ((ushort) 40)
				.Write (50U)
				.Write (60UL);
			byte[] b = ms.ToArray();
			AssertAreSame (expected, ms.ToArray ());
		}
	}
}
