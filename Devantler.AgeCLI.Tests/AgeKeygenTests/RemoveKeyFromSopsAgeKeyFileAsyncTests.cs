namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.RemoveKeyFromSopsAgeKeyFileAsync(string, CancellationToken)"/> method.
/// </summary>
public class RemoveKeyFromSopsAgeKeyFileTests
{

  /// <summary>
  /// Tests that setting the SOPS_AGE_KEY_FILE environment variable removes the key from the specified file.
  /// </summary>
  [Fact]
  public async Task RemoveKeyFromSopsAgeKeyFileAsync_GivenSopsAgeKeyFileEnvironmentVariable_ShouldRemoveKeyFromFile()
  {
    // Arrange
    await File.WriteAllTextAsync("key.txt", "age1abc123");
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", "key.txt");

    // Act
    await AgeKeygen.RemoveKeyFromSopsAgeKeyFileAsync("age1abc123", CancellationToken.None);

    // Assert
    string result = await File.ReadAllTextAsync("key.txt");
    Assert.Equal("", result);

    // Cleanup
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", null);
    File.Delete("key.txt");
  }
}
