//
// StreamTest.cs
//
// Authors:
//   Jonathan Pryor  <jpryor@novell.com>
//   Bojan Rajkovic  <bojanr@brandeis.edu>
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

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadShort_SelfNull ()
		{
			Stream s = null;
			short value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadInt_SelfNull ()
		{
			Stream s = null;
			int value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadLong_SelfNull ()
		{
			Stream s = null;
			long value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadUShort_SelfNull ()
		{
			Stream s = null;
			ushort value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadUInt_SelfNull ()
		{
			Stream s = null;
			uint value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadULong_SelfNull ()
		{
			Stream s = null;
			ulong value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadBool_SelfNull ()
		{
			Stream s = null;
			bool value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadChar_SelfNull ()
		{
			Stream s = null;
			char value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadDouble_SelfNull ()
		{
			Stream s = null;
			double value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadFloat_SelfNull ()
		{
			Stream s = null;
			float value;
			s.Read (out value);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadString_SelfNull ()
		{
			Stream s = null;
			string value;
			s.Read (out value, 15, Encoding.UTF8);
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void ReadTValue_SelfNull ()
		{
			Stream s = null;
			ConsoleKeyInfo cki;
			s.Read (out cki);
		}

		[Test]
		public void Read ()
		{
			MemoryStream ms = new MemoryStream();	

			Guid guid = Guid.NewGuid ();
			ms.Write (BitConverter.GetBytes ((ushort)2124), 0, 2);
			ms.Write (BitConverter.GetBytes ((uint)150291), 0, 4);
			ms.Write (BitConverter.GetBytes ((ulong)4496977496), 0, 8);
			ms.Write (BitConverter.GetBytes ((short)-5215), 0, 2);
			ms.Write (BitConverter.GetBytes (-125191), 0, 4);
			ms.Write (BitConverter.GetBytes (-4514967426), 0, 8);
			ms.Write (BitConverter.GetBytes ('c'), 0, 2);
			ms.Write (BitConverter.GetBytes (true), 0, 1);
			ms.Write (BitConverter.GetBytes (0.712d), 0, 8);
			ms.Write (BitConverter.GetBytes (1.23f), 0, 4);
			ms.Write (guid);
			ms.Write (Encoding.UTF8.GetBytes ("Hello"));

			ushort us;
			uint ui;
			ulong ul;
			short s;
			int i;
			long l;
			char c;
			bool b;
			double d;
			float f;
			Guid og;
			string str;

			ms.Position = 0;
			ms.Read (out us)
				.Read (out ui)
				.Read (out ul)
				.Read (out s)
				.Read (out i)
				.Read (out l)
				.Read (out c)
				.Read (out b)
				.Read (out d)
				.Read (out f)
				.Read (out og)
				.Read (out str, 5, Encoding.UTF8);

			Assert.AreEqual ((ushort) 2124, us);
			Assert.AreEqual (150291U, ui);
			Assert.AreEqual (4496977496UL, ul);
			Assert.AreEqual ((short)-5215, s);
			Assert.AreEqual (-125191, i);
			Assert.AreEqual (-4514967426L, l);
			Assert.AreEqual ('c', c);
			Assert.AreEqual (true, b);
			Assert.AreEqual (0.712d, d);
			Assert.AreEqual (1.23f, f);
			Assert.AreEqual (guid, og);
		}

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
		public void Write_Bytes_SelfNull ()
		{
			Stream  s      = null;
			byte[] value = new byte[0];
			s.Write (value);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void Write_Bytes_ValueNull ()
		{
			Stream  s      = new MemoryStream ();
			byte[] value = null;
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
				0x3c, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,        // ulong
				0x48, 0x65, 0x6c, 0x6c, 0x6f,                   // "Hello"
				0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,         // Guid-start
				0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,         // Guid-end
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
				.Write (60UL)
				.Write (Encoding.UTF8.GetBytes ("Hello"))
				.Write (new Guid ());
			byte[] b = ms.ToArray();
			AssertAreSame (expected, ms.ToArray ());
		}
	}
}
