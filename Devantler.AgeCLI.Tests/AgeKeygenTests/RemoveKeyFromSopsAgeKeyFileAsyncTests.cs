namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.RemoveKeyFromSopsAgeKeyFileAsync(string, CancellationToken)"/> method.
/// </summary>
[Collection("Sequential")]
public class RemoveKeyFromSopsAgeKeyFileTests
{

  /// <summary>
  /// Tests that setting the SOPS_AGE_KEY_FILE environment variable removes the key from the specified file.
  /// </summary>
  [Fact]
  public async Task RemoveKeyFromSopsAgeKeyFileAsync_GivenSopsAgeKeyFileEnvironmentVariable_ShouldRemoveKeyFromFile()
  {
    // Arrange
    string keyPath = "remove-key-from-sops-age-key-file.txt";
    await File.WriteAllTextAsync(keyPath, "age1abc123");
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", keyPath);

    // Act
    await AgeKeygen.RemoveKeyFromSopsAgeKeyFileAsync("age1abc123", CancellationToken.None);

    // Assert
    string result = await File.ReadAllTextAsync(keyPath);
    Assert.Equal("", result);

    // Cleanup
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", null);
    // this step does not always delete the file successfully, why so?
    File.Delete(keyPath);
  }
}
