namespace Devantler.AgeCLI.Tests.AgeKeyTests;

/// <summary>
/// Tests for the <see cref="AgeCLI.AddKeyAsync(bool, CancellationToken)"/> method and the <see cref="AgeCLI.AddKeyAsync(string, bool, bool, CancellationToken)"/> method.
/// </summary>
public class GenerateKeyAsyncTests
{

  /// <summary>
  /// Tests that the <see cref="AgeCLI.AddKeyAsync(bool, CancellationToken)"/> method returns a zero exit code and an age key.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_ShouldReturnsAgeKey()
  {
    // Act
    string key = await AgeCLI.AddKeyAsync();

    // Assert
    Assert.DoesNotContain("Public key:", key);
    Assert.Contains("# created:", key);
    Assert.Contains("# public key:", key);
    Assert.Contains("AGE-SECRET-KEY-", key);
  }

  /// <summary>
  /// Tests that the <see cref="AgeCLI.AddKeyAsync(bool, CancellationToken)"/> method returns a zero exit code and an age key when adding the key to the sops age key file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenBooleanToAddKeyToSopsAgeKeyFile_ShouldRAgeKeyAndAddsKeyToSopsAgeKeyFile()
  {
    // Act
    string key = await AgeCLI.AddKeyAsync(addToSopsAgeKeyFile: true);
    string sopsAgeKeyFileContents = await AgeCLI.ShowSopsAgeKeyFileAsync();


    // Assert
    Assert.DoesNotContain("Public key:", key);
    Assert.Contains("# created:", key);
    Assert.Contains("# public key:", key);
    Assert.Contains("AGE-SECRET-KEY-", key);
    Assert.Contains(key, sopsAgeKeyFileContents);

    // Cleanup
    await AgeCLI.RemoveKeyAsync(key, removeFromSopsAgeKeyFile: true);
    sopsAgeKeyFileContents = await AgeCLI.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(key, sopsAgeKeyFileContents);
  }

  /// <summary>
  /// Tests that the <see cref="AgeCLI.AddKeyAsync(string, bool, bool, CancellationToken)"/> method returns a zero exit code and writes the key to the specified file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenValidPath_ShouldWriteKeyToFile()
  {
    // Act
    await AgeCLI.AddKeyAsync("keys.txt", shouldOverwrite: false, addToSopsAgeKeyFile: false, default);
    string keyContents = await AgeCLI.ShowKeyAsync("keys.txt");

    // Assert
    Assert.DoesNotContain("Public key:", keyContents);
    Assert.Contains("# created:", keyContents);
    Assert.Contains("# public key:", keyContents);
    Assert.Contains("AGE-SECRET-KEY-", keyContents);

    // Cleanup
    await AgeCLI.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: false, default);
    Assert.False(File.Exists("keys.txt"));
  }
}
