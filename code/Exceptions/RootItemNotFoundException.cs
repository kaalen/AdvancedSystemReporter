using System;

namespace Sitecore.Feature.Reports.Exceptions
{
	public class RootItemNotFoundException : Exception
	{
		public RootItemNotFoundException(string message)
			: base(message)
		{
		}
	}
}
