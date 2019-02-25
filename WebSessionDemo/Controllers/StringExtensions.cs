using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSessionDemo.Controllers
{
	public static class StringExtensions
	{
		public static bool Contains(this string source, string pattern, StringComparison comparison)
		{
			return source.IndexOf(pattern, comparison) >= 0;
		}
	}
}