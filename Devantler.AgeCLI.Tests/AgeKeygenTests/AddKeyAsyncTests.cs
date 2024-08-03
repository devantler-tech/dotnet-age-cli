namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.AddKeyAsync(bool, CancellationToken)"/> method and the <see cref="AgeKeygen.AddKeyAsync(string, bool, bool, CancellationToken)"/> method.
/// </summary>
public class GenerateKeyAsyncTests
{

  /// <summary>
  /// Tests that the <see cref="AgeKeygen.AddKeyAsync(bool, CancellationToken)"/> method returns an age key.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_ShouldReturnsAgeKey()
  {
    // Act
    string key = await AgeKeygen.AddKeyAsync();

    // Assert
    Assert.DoesNotContain("Public key:", key, StringComparison.Ordinal);
    Assert.Contains("# created:", key, StringComparison.Ordinal);
    Assert.Contains("# public key:", key, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", key, StringComparison.Ordinal);
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygen.AddKeyAsync(bool, CancellationToken)"/> method returns an age key and adds the key to the sops age key file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenBooleanToAddKeyToSopsAgeKeyFile_ShouldReturnAgeKeyAndAddsKeyToSopsAgeKeyFile()
  {
    // Act
    string key = await AgeKeygen.AddKeyAsync(addToSopsAgeKeyFile: true);
    string sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();

    // Assert
    Assert.DoesNotContain("Public key:", key, StringComparison.Ordinal);
    Assert.Contains("# created:", key, StringComparison.Ordinal);
    Assert.Contains("# public key:", key, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", key, StringComparison.Ordinal);
    Assert.Contains(key, sopsAgeKeyFileContents, StringComparison.Ordinal);

    // Cleanup
    await AgeKeygen.RemoveKeyFromSopsAgeKeyFileAsync(key);
    sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(key, sopsAgeKeyFileContents, StringComparison.Ordinal);
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygen.AddKeyAsync(string, bool, bool, CancellationToken)"/> method writes an age key to the specified file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenValidPath_ShouldWriteKeyToFile()
  {
    // Act
    await AgeKeygen.AddKeyAsync("keys.txt", shouldOverwrite: true);
    string keyContents = await AgeKeygen.ShowKeyAsync("keys.txt");

    // Assert
    Assert.DoesNotContain("Public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyContents, StringComparison.Ordinal);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: false);
    Assert.False(File.Exists("keys.txt"));
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygen.AddKeyAsync(string, bool, bool, CancellationToken)"/> method writes an age key to the specified file, and adds the key to the sops age key file.
  /// </summary>
  [Fact]
  public async Task AddKeyAsync_GivenValidPathAndBooleanToAddKeyToSopsAgeKeyFile_ShouldWriteKeyToFileAndAddKeyToSopsAgeKeyFile()
  {
    // Act
    await AgeKeygen.AddKeyAsync("keys.txt", shouldOverwrite: true, addToSopsAgeKeyFile: true);
    string keyContents = await AgeKeygen.ShowKeyAsync("keys.txt");
    string sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();

    // Assert
    Assert.DoesNotContain("Public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyContents, StringComparison.Ordinal);
    Assert.Contains(keyContents, sopsAgeKeyFileContents, StringComparison.Ordinal);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: true);
    Assert.False(File.Exists("keys.txt"));
    sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(keyContents, sopsAgeKeyFileContents, StringComparison.Ordinal);
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygen.AddKeyAsync(string, bool, bool, CancellationToken)"/> method overwrites an existing key when the boolean to overwrite is set to true.
  /// </summary>
  [Fact]
  public async Task AddKeyAsync_GivenValidPathAndBooleanToOverwrite_ShouldOverwriteExistingKey()
  {
    // Arrange
    await AgeKeygen.AddKeyAsync("keys.txt", shouldOverwrite: true);
    string keyContents = await AgeKeygen.ShowKeyAsync("keys.txt");

    // Act
    await AgeKeygen.AddKeyAsync("keys.txt", shouldOverwrite: true);
    string newKeyContents = await AgeKeygen.ShowKeyAsync("keys.txt");

    // Assert
    Assert.DoesNotContain("Public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyContents, StringComparison.Ordinal);
    Assert.DoesNotContain("Public key:", newKeyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", newKeyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", newKeyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", newKeyContents, StringComparison.Ordinal);
    Assert.NotEqual(keyContents, newKeyContents);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: false);
    Assert.False(File.Exists("keys.txt"));
  }
}
