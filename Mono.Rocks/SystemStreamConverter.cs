//
// SystemStreamConverter.cs
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Mono.Rocks {

	public sealed class SystemStreamConverter : IValueReader<SystemStreamConverter>, IValueWriter<SystemStreamConverter>
	{
		internal SystemStreamConverter (Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException ("stream");

			BaseStream = stream;
			buffer = new byte [8];
		}

		public Stream BaseStream { get; private set; }

		private void AssertRead ()
		{
			if (!BaseStream.CanRead)
				throw new InvalidOperationException ("Cannot read from stream");
		}

		private void AssertWrite ()
		{
			if (!BaseStream.CanWrite)
				throw new InvalidOperationException ("Cannot write to stream");
		}

		private readonly byte[] buffer;

		private void FillBuffer (byte[] buffer, int size)
		{
			int read, offset = 0;

			while ((read = BaseStream.Read (buffer, offset, size - offset)) > 0) {
				offset += read;
			}

			if (read == -1)
				throw new EndOfStreamException ();
		}

		private byte[] Take (int size)
		{
			byte[] buff = size <= buffer.Length ? buffer : new byte [size];

			FillBuffer (buff, size);

			return buff;
		}

		public SystemStreamConverter Read (out bool value)
		{
			AssertRead ();

			value = BitConverter.ToBoolean (Take (1), 0);

			return this;
		}

		public SystemStreamConverter Read (out byte value)
		{
			AssertRead ();

			int b = BaseStream.ReadByte ();
			if (b == -1)
				throw new EndOfStreamException ();

			value = (byte) b;

			return this;
		}

		public SystemStreamConverter Read (out short value)
		{
			AssertRead ();

			value = BitConverter.ToInt16 (Take (2), 0);

			return this;
		}

		public SystemStreamConverter Read (out int value)
		{
			AssertRead ();

			value = BitConverter.ToInt32 (Take (4), 0);

			return this;
		}

		public SystemStreamConverter Read (out long value)
		{
			AssertRead ();

			value = BitConverter.ToInt64 (Take (8), 0);

			return this;
		}

		public SystemStreamConverter Read (out double value)
		{
			AssertRead ();

			value = BitConverter.ToDouble (Take (8), 0);

			return this;
		}

		public SystemStreamConverter Read (out char value)
		{
			AssertRead ();

			value = BitConverter.ToChar (Take (2), 0);

			return this;
		}

		public SystemStreamConverter Read (out float value)
		{
			AssertRead ();

			value = BitConverter.ToSingle (Take (4), 0);

			return this;
		}

		public SystemStreamConverter Read (byte[] value)
		{
			AssertRead ();

			FillBuffer (value, value.Length);

			return this;
		}

		[CLSCompliant (false)]
		public SystemStreamConverter Read (out ushort value)
		{
			AssertRead ();

			value = BitConverter.ToUInt16 (Take (2), 0);

			return this;
		}

		[CLSCompliant (false)]
		public SystemStreamConverter Read (out uint value)
		{
			AssertRead ();

			value = BitConverter.ToUInt32 (Take (4), 0);

			return this;
		}

		[CLSCompliant (false)]
		public SystemStreamConverter Read (out ulong value)
		{
			AssertRead ();

			value = BitConverter.ToUInt64 (Take (8), 0);

			return this;
		}

		public SystemStreamConverter Read (out string value)
		{
			throw new NotSupportedException ();
		}

		public SystemStreamConverter Read (int size, Encoding encoding, out string value)
		{
			AssertRead ();

			value = encoding.GetString (Take (size));

			return this;
		}

		public SystemStreamConverter Write (bool value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (byte value)
		{
			AssertWrite ();

			BaseStream.WriteByte (value);

			return this;
		}

		public SystemStreamConverter Write (char value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (double value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (short value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (int value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (long value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (float value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (byte[] value)
		{
			AssertWrite ();
			if (value == null)
				throw new ArgumentNullException ("value");

			BaseStream.Write (value, 0, value.Length);
			return this;
		}

		[CLSCompliant (false)]
		public SystemStreamConverter Write (ushort value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		[CLSCompliant (false)]
		public SystemStreamConverter Write (uint value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		[CLSCompliant (false)]
		public SystemStreamConverter Write (ulong value)
		{
			AssertWrite ();

			return Write (BitConverter.GetBytes (value));
		}

		public SystemStreamConverter Write (string value)
		{
			throw new NotSupportedException ();
		}
	}

	public static class SystemStreamConverterRocks
	{
		public static SystemStreamConverter Read<TValue> (this SystemStreamConverter self, out TValue value)
		{
			Check.Self (self);

			byte[] data = new byte [Marshal.SizeOf (typeof (TValue))];
			self.Read (data);
			GCHandle handle = GCHandle.Alloc (data, GCHandleType.Pinned);

			try { 
				value = (TValue) Marshal.PtrToStructure (handle.AddrOfPinnedObject (), typeof (TValue)); 
			} finally {
				handle.Free();
			}

			return self;
		}

		public static SystemStreamConverter Write<TValue> (this SystemStreamConverter self, TValue value)
		{
			Check.Self (self);

			byte[] data = new byte [Marshal.SizeOf (typeof (TValue))];

			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				Marshal.StructureToPtr (value, handle.AddrOfPinnedObject(), false);
			} finally {
				handle.Free();
			}

			return self.Write (data);
		}
	}
}

