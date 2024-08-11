namespace Devantler.AgeCLI.Tests;

/// <summary>
/// Tests for the <see cref="AgeException"/> class.
/// </summary>
public class AgeExceptionTests
{
  /// <summary>
  /// Tests the default constructor.
  /// </summary>
  [Fact]
  public void Constructor_WithNothing_CallsDefaultConstructor()
  {
    // Act
    var exception = new AgeException();

    // Assert
    Assert.NotNull(exception);
  }

  /// <summary>
  /// Tests the constructor with a message.
  /// </summary>
  [Fact]
  public void Constructor_WithMessage_SetsMessageProperty()
  {
    // Arrange
    string message = "Test message";

    // Act
    var exception = new AgeException(message);

    // Assert
    Assert.Equal(message, exception.Message);
  }

  /// <summary>
  /// Tests the constructor with a message and inner exception.
  /// </summary>
  [Fact]
  public void Constructor_WithMessageAndInnerException_SetsMessageAndInnerExceptionProperties()
  {
    // Arrange
    string message = "Test message";
    var innerException = new NotImplementedException();

    // Act
    var exception = new AgeException(message, innerException);

    // Assert
    Assert.Equal(message, exception.Message);
    Assert.Equal(innerException, exception.InnerException);
  }
}

