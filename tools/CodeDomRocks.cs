//
// Lambdas.cs
//
// Author:
//   Jonathan Pryor (jpryor@novell.com)
//
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com)
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
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;

namespace Mono.Rocks.Tools
{
	public static class Seq {
		public static IEnumerable Expand (object o)
		{
			IEnumerable e;
			var s = o as string;
			if (s != null)
				yield return s;
			else if ((e = o as IEnumerable) != null)
				foreach (var obj in e)
					foreach (object oo in Expand (obj))
						yield return oo;
			else
				yield return o;
		}
	}

	public static class CodeDomRocks
	{
		public static void AddRange (this CodeCommentStatementCollection self, IEnumerable<CodeCommentStatement> comments)
		{
			foreach (var c in comments)
				self.Add (c);
		}

		public static void AddRange (this CodeTypeParameterCollection self, IEnumerable<CodeTypeParameter> ps)
		{
			foreach (var p in ps)
				self.Add (p);
		}

		public static void AddRange (this CodeCommentStatementCollection self, params object[] comments)
		{
			foreach (var comment in Seq.Expand (comments))
				self.Add (new CodeCommentStatement (comment.ToString ()));
		}

		public static void AddDocs (this CodeCommentStatementCollection self, params object[] comments)
		{
			foreach (var comment in Seq.Expand (comments))
				self.Add (new CodeCommentStatement (comment.ToString (), true));
		}
	}
}
