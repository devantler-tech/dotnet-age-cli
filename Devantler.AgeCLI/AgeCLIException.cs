namespace Devantler.AgeCLI;

/// <summary>
/// An exception thrown by the AgeCLI library.
/// </summary>
public class AgeCLIException(string message) : Exception(message)
{
}
