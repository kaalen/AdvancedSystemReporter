using System;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.Reports.Exceptions
{
  /// <summary>
  /// Specific exception type which is meant to be used when an exception 
  /// occurs while handling or interpreting a parameter. The error is logged 
  /// into Sitecore log.
  /// </summary>
  [Obsolete("Consider removing because this class currently isn't used anywhere.")]
  internal class ParameterException : Exception
  {
    public ParameterException() : this(null, null)
    {
    }

    public ParameterException(string message) : this(message, null)
    {
    }


    public ParameterException(string message, Exception innerException)
      : base(message, innerException)
    {
      Log.Error(message, innerException, this.Source);
    }
  }
}