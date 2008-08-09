//
// Stream.cs
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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Rocks {

	public static class StreamRocks {

		private static byte[] Take (Stream self, int size)
		{
			byte[] buff = new byte [size];
			int read, offset = 0;

			while ((read = self.Read (buff, offset, buff.Length - offset)) > 0)
				offset += read;

			return buff;
		}

		private static void AssertRead (Stream self)
		{
			Check.Self (self);
			if (!self.CanRead)
				throw new ArgumentException ("Cannot read from stream");
		}

		public static Stream Read (this Stream self, out short value)
		{
			AssertRead (self);

			value = BitConverter.ToInt16 (Take (self, 2), 0);

			return self;
		}

		public static Stream Read (this Stream self, out int value)
		{
			AssertRead (self);

			value = BitConverter.ToInt32 (Take (self, 4), 0);

			return self;
		}

		public static Stream Read (this Stream self, out long value)
		{
			AssertRead (self);

			value = BitConverter.ToInt64 (Take (self, 8), 0);

			return self;
		}

		public static Stream Read (this Stream self, out bool value)
		{
			AssertRead (self);

			value = BitConverter.ToBoolean (Take (self, 1), 0);

			return self;
		}

		public static Stream Read (this Stream self, out double value)
		{
			AssertRead (self);

			value = BitConverter.ToDouble (Take (self, 8), 0);

			return self;
		}

		public static Stream Read (this Stream self, out char value)
		{
			AssertRead (self);

			value = BitConverter.ToChar (Take (self, 2), 0);

			return self;
		}

		public static Stream Read (this Stream self, out float value)
		{
			AssertRead (self);

			value = BitConverter.ToSingle (Take (self, 4), 0);

			return self;
		}

		public static Stream Read (this Stream self, out ushort value)
		{
			AssertRead (self);

			value = BitConverter.ToUInt16(Take (self, 2), 0);

			return self;
		}

		public static Stream Read (this Stream self, out uint value)
		{
			AssertRead (self);

			value = BitConverter.ToUInt32 (Take (self, 4), 0);

			return self;
		}

		public static Stream Read (this Stream self, out ulong value)
		{
			AssertRead (self);

			value = BitConverter.ToUInt64(Take (self, 8), 0);

			return self;
		}

		public static Stream Read (this Stream self, out string value, int length, Encoding encoding)
		{
			AssertRead (self);

			value = encoding.GetString (Take (self, length));

			return self;
		}

		public static Stream Read<TValue> (this Stream self, out TValue value)
		{
			AssertRead (self);

			byte[] buff = Take (self, Marshal.SizeOf (typeof (TValue)));
			GCHandle handle = GCHandle.Alloc(buff, GCHandleType.Pinned);

			try { 
				value = (TValue) Marshal.PtrToStructure (handle.AddrOfPinnedObject (), typeof (TValue)); 
			} finally {
				handle.Free();
			}

			return self;
		}

		public static Stream Write (this Stream self, bool value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		private static void AssertWrite (Stream self)
		{
			Check.Self (self);
			if (!self.CanWrite)
				throw new ArgumentException ("Cannot write to stream");
		}

		private static Stream WriteBytes (Stream self, byte[] value)
		{
			self.Write (value, 0, value.Length);
			return self;
		}

		public static Stream Write (this Stream self, byte value)
		{
			AssertWrite (self);

			return WriteBytes (self, new byte []{value});
		}

		public static Stream Write (this Stream self, char value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, double value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, short value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, int value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, long value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, float value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, ushort value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, uint value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, ulong value)
		{
			AssertWrite (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write<TValue> (this Stream self, TValue value)
		{
			AssertWrite (self);

			byte[] data = new byte [Marshal.SizeOf (typeof (TValue))];

			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Marshal.StructureToPtr (value, handle.AddrOfPinnedObject(), false);
			} finally {
				handle.Free();
			}

			return WriteBytes (self, data);
		}
	}
}
