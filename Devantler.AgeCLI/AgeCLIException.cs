namespace Devantler.AgeCLI;

/// <summary>
/// An exception thrown by the AgeCLI library.
/// </summary>
public class AgeCLIException : Exception
{
  /// <summary>
  /// Default constructor.
  /// </summary>
  public AgeCLIException()
  {
  }

  /// <summary>
  /// Constructor with message.
  /// </summary>
  /// <param name="message"></param>
  public AgeCLIException(string message) : base(message)
  {
  }

  /// <summary>
  /// Constructor with message and inner exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="innerException"></param>
  public AgeCLIException(string message, Exception innerException) : base(message, innerException)
  {
  }


}
