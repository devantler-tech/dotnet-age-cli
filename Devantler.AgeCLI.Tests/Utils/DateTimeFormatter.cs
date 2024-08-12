using System.Globalization;

namespace Devantler.AgeCLI.Tests.Utils;

/// <summary>
/// A helper class for formatting DateTime objects.
/// </summary>
public static class DateTimeFormatter
{
  /// <summary>
  /// Formats a DateTimeOffset object with a custom offset.
  /// </summary>
  /// <param name="dateTimeOffset"></param>
  public static string FormatDateTimeWithCustomOffset(DateTimeOffset dateTimeOffset)
  {
    string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
    string formattedDateTime = dateTimeOffset.ToString(dateTimeFormat, CultureInfo.InvariantCulture);

    return dateTimeOffset.Offset == TimeSpan.Zero ?
      formattedDateTime + "Z" :
      formattedDateTime + dateTimeOffset.ToString("zzz", CultureInfo.InvariantCulture);
  }
}

