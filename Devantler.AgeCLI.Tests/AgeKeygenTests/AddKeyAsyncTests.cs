namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.AddKeyAsync(bool, CancellationToken)"/> method and the <see cref="AgeKeygen.AddKeyAsync(string, bool, bool, CancellationToken)"/> method.
/// </summary>
[Collection("Sequential")]
public class AddKeyAsyncTests
{

  /// <summary>
  /// Tests that an age key is returned.
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
  /// Tests that an age key is returned and that the key is added to the sops age key file.
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
  /// Tests that an age key is written to the specified file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenValidPath_ShouldWriteKeyToFile()
  {
    // Act
    await AgeKeygen.AddKeyAsync("add-key-async.txt", shouldOverwrite: true);
    string keyContents = await AgeKeygen.ShowKeyAsync("add-key-async.txt");

    // Assert
    Assert.DoesNotContain("Public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyContents, StringComparison.Ordinal);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync("add-key-async.txt", removeFromSopsAgeKeyFile: false);
    Assert.False(File.Exists("add-key-async.txt"));
  }

  /// <summary>
  /// Tests that an age key is written to the specified file, and that the key is added to the sops age key file.
  /// </summary>
  [Fact]
  public async Task AddKeyAsync_GivenValidPathAndBooleanToAddKeyToSopsAgeKeyFile_ShouldWriteKeyToFileAndAddKeyToSopsAgeKeyFile()
  {
    // Arrange
    string keyPath = "add-key-async.txt";

    // Act
    await AgeKeygen.AddKeyAsync(keyPath, shouldOverwrite: true, addToSopsAgeKeyFile: true);
    string keyContents = await AgeKeygen.ShowKeyAsync(keyPath);
    string sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();

    // Assert
    Assert.DoesNotContain("Public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyContents, StringComparison.Ordinal);
    Assert.Contains(keyContents, sopsAgeKeyFileContents, StringComparison.Ordinal);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync(keyPath, removeFromSopsAgeKeyFile: true);
    Assert.False(File.Exists(keyPath));
    sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(keyContents, sopsAgeKeyFileContents, StringComparison.Ordinal);
  }

  /// <summary>
  /// Tests that an age key is written to the specified file, and that the key is added to the sops age key file from the SOP_AGE_KEY_FILE environment variable.
  /// </summary>
  [Fact]
  public async Task AddKeyAsync_GivenValidPathAndBooleanToAddKeyToSopsAgeKeyFileFromEnvironmentVariable_ShouldWriteKeyToFileAndAddKeyToSopsAgeKeyFile()
  {
    // Arrange
    string keyPath = "add-key-async.txt";
    string sopsAgeKeyFile = "add-key-async-sops-age-key-file.txt";
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", sopsAgeKeyFile);

    // Act
    await AgeKeygen.AddKeyAsync(keyPath, shouldOverwrite: true, addToSopsAgeKeyFile: true);
    string keyContents = await AgeKeygen.ShowKeyAsync(keyPath);
    string sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();

    // Assert
    Assert.DoesNotContain("Public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# created:", keyContents, StringComparison.Ordinal);
    Assert.Contains("# public key:", keyContents, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyContents, StringComparison.Ordinal);
    Assert.Contains(keyContents, sopsAgeKeyFileContents, StringComparison.Ordinal);

    // Cleanup
    await AgeKeygen.RemoveKeyAsync(keyPath, removeFromSopsAgeKeyFile: true);
    Assert.False(File.Exists(keyPath));
    sopsAgeKeyFileContents = await AgeKeygen.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(keyContents, sopsAgeKeyFileContents, StringComparison.Ordinal);
    File.Delete(sopsAgeKeyFile);
    Assert.False(File.Exists(sopsAgeKeyFile));
    Environment.SetEnvironmentVariable("SOPS_AGE_KEY_FILE", null);
  }

  /// <summary>
  /// Tests that an existing key is overwritten when the boolean to overwrite is set to true.
  /// </summary>
  [Fact]
  public async Task AddKeyAsync_GivenValidPathAndBooleanToOverwrite_ShouldOverwriteExistingKey()
  {
    // Arrange
    await AgeKeygen.AddKeyAsync("add-key-async.txt", shouldOverwrite: true);
    string keyContents = await AgeKeygen.ShowKeyAsync("add-key-async.txt");

    // Act
    await AgeKeygen.AddKeyAsync("add-key-async.txt", shouldOverwrite: true);
    string newKeyContents = await AgeKeygen.ShowKeyAsync("add-key-async.txt");

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
    await AgeKeygen.RemoveKeyAsync("add-key-async.txt", removeFromSopsAgeKeyFile: false);
    Assert.False(File.Exists("add-key-async.txt"));
  }

  /// <summary>
  /// Tests that an <see cref="AgeException"/> is thrown when the path is invalid.
  /// </summary>
  [Fact]
  public async Task AddKeyAsync_GivenInvalidPath_ShouldThrowAgeException()
  {
    // Act
    static async Task Act() => await AgeKeygen.AddKeyAsync("invalid-path//add-key-async.txt").ConfigureAwait(false);

    // Assert
    _ = await Assert.ThrowsAsync<AgeException>(Act);
  }
}
