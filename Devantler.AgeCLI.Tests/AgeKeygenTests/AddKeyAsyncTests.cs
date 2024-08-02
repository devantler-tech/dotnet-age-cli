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
    Assert.DoesNotContain("Public key:", key);
    Assert.Contains("# created:", key);
    Assert.Contains("# public key:", key);
    Assert.Contains("AGE-SECRET-KEY-", key);
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
    Assert.DoesNotContain("Public key:", key);
    Assert.Contains("# created:", key);
    Assert.Contains("# public key:", key);
    Assert.Contains("AGE-SECRET-KEY-", key);
    Assert.Contains(key, sopsAgeKeyFileContents);

    // Cleanup
    await AgeKeygen.RemoveKeyFromSopsAgeKeyFileAsync(key);
    sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(key, sopsAgeKeyFileContents);
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
    Assert.DoesNotContain("Public key:", keyContents);
    Assert.Contains("# created:", keyContents);
    Assert.Contains("# public key:", keyContents);
    Assert.Contains("AGE-SECRET-KEY-", keyContents);

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
    Assert.DoesNotContain("Public key:", keyContents);
    Assert.Contains("# created:", keyContents);
    Assert.Contains("# public key:", keyContents);
    Assert.Contains("AGE-SECRET-KEY-", keyContents);
    Assert.Contains(keyContents, sopsAgeKeyFileContents);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: true);
    Assert.False(File.Exists("keys.txt"));
    sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(keyContents, sopsAgeKeyFileContents);
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygen.AddKeyAsync(string, bool, bool, CancellationToken)"/> method overwrites an existing key when the <paramref name="shouldOverwrite"/> parameter is set to <c>true</c>.
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
    Assert.DoesNotContain("Public key:", keyContents);
    Assert.Contains("# created:", keyContents);
    Assert.Contains("# public key:", keyContents);
    Assert.Contains("AGE-SECRET-KEY-", keyContents);
    Assert.DoesNotContain("Public key:", newKeyContents);
    Assert.Contains("# created:", newKeyContents);
    Assert.Contains("# public key:", newKeyContents);
    Assert.Contains("AGE-SECRET-KEY-", newKeyContents);
    Assert.NotEqual(keyContents, newKeyContents);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: false);
    Assert.False(File.Exists("keys.txt"));
  }
}
