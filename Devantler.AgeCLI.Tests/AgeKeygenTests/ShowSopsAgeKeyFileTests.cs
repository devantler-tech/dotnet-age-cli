namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.ShowSopsAgeKeyFileAsync(CancellationToken)"/> method.
/// </summary>
public class ShowSopsAgeKeyFileTests
{

  /// <summary>
  /// Tests that setting the SOPS_AGE_KEY_FILE environment variable returns the contents from the specified file.
  /// </summary>
  [Fact]
  public async Task ShowSopsAgeKeyFileAsync_GivenSopsAgeKeyFileEnvironmentVariable_ShouldReturnContentsFromFile()
  {
    // Arrange
    string keyPath = "show-sops-age-key-file.txt";
    await File.WriteAllTextAsync(keyPath, "age1abc123");
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", keyPath);

    // Act
    string result = await AgeKeygen.ShowSopsAgeKeyFileAsync(CancellationToken.None);

    // Assert
    Assert.Equal("age1abc123", result);

    // Cleanup
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", null);
    File.Delete(keyPath);
  }
}
