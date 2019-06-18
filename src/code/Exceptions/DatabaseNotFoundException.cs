using System;

namespace Sitecore.Feature.Reports.Exceptions
{
  /// <summary>
  /// Specific exception type meant to be used when a database passed in as a 
  /// report parameter isn’t found.
  /// </summary>
  public class DatabaseNotFoundException : Exception
  {
  }
}