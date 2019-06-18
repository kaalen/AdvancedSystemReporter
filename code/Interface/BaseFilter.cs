using Sitecore.Diagnostics;

namespace Sitecore.Feature.Reports.Interface
{
	public abstract class BaseFilter : BaseReportObject
	{
		public abstract bool Filter(object element);

		private static BaseFilter Create(string type)
		{
			return BaseReportObject.CreateObject(type) as BaseFilter;
		}

		internal static BaseFilter Create(string type, string parameters)
		{
			Assert.ArgumentNotNull(type, "type");
			Assert.ArgumentNotNull(parameters, "parameters");
			BaseFilter oFilter = BaseFilter.Create(type);
			oFilter.AddParameters(parameters);
			return oFilter;
		}

	}
}
