namespace Devantler.AgeCLI;

/// <summary>
/// An exception thrown by the AgeCLI library.
/// </summary>
public class AgeException : Exception
{
  /// <summary>
  /// Default constructor.
  /// </summary>
  public AgeException()
  {
  }

  /// <summary>
  /// Constructor with message.
  /// </summary>
  /// <param name="message"></param>
  public AgeException(string message) : base(message)
  {
  }

  /// <summary>
  /// Constructor with message and inner exception.
  /// </summary>
  /// <param name="message"></param>
  /// <param name="innerException"></param>
  public AgeException(string message, Exception innerException) : base(message, innerException)
  {
  }


}
