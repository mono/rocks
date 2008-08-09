//
// Stream.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace Mono.Rocks {

	public static class StreamRocks {

		public static Stream Write (this Stream self, bool value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		private static Stream WriteBytes (Stream self, byte[] value)
		{
			self.Write (value, 0, value.Length);
			return self;
		}

		public static Stream Write (this Stream self, byte value)
		{
			Check.Self (self);

			return WriteBytes (self, new byte []{value});
		}

		public static Stream Write (this Stream self, char value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, double value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, short value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, int value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, long value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, float value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, ushort value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, uint value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write (this Stream self, ulong value)
		{
			Check.Self (self);

			return WriteBytes (self, BitConverter.GetBytes (value));
		}

		public static Stream Write<TValue> (this Stream self, TValue value)
		{
			Check.Self (self);

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
