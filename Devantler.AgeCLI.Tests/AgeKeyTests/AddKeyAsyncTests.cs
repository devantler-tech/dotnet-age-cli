namespace Devantler.AgeCLI.Tests.AgeKeyTests;

/// <summary>
/// Tests for the <see cref="AgeKeygenCLI.AddKeyAsync(bool, CancellationToken)"/> method and the <see cref="AgeKeygenCLI.AddKeyAsync(string, bool, bool, CancellationToken)"/> method.
/// </summary>
public class GenerateKeyAsyncTests
{

  /// <summary>
  /// Tests that the <see cref="AgeKeygenCLI.AddKeyAsync(bool, CancellationToken)"/> method returns an age key.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_ShouldReturnsAgeKey()
  {
    // Act
    string key = await AgeKeygenCLI.AddKeyAsync();

    // Assert
    Assert.DoesNotContain("Public key:", key);
    Assert.Contains("# created:", key);
    Assert.Contains("# public key:", key);
    Assert.Contains("AGE-SECRET-KEY-", key);
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygenCLI.AddKeyAsync(bool, CancellationToken)"/> method returns an age key and adds the key to the sops age key file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenBooleanToAddKeyToSopsAgeKeyFile_ShouldReturnAgeKeyAndAddsKeyToSopsAgeKeyFile()
  {
    // Act
    string key = await AgeKeygenCLI.AddKeyAsync(addToSopsAgeKeyFile: true);
    string sopsAgeKeyFileContents = await AgeKeygenCLI.ShowSopsAgeKeyFileAsync();

    // Assert
    Assert.DoesNotContain("Public key:", key);
    Assert.Contains("# created:", key);
    Assert.Contains("# public key:", key);
    Assert.Contains("AGE-SECRET-KEY-", key);
    Assert.Contains(key, sopsAgeKeyFileContents);

    // Cleanup
    await AgeKeygenCLI.RemoveKeyFromSopsAgeKeyFileAsync(key);
    sopsAgeKeyFileContents = await AgeKeygenCLI.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(key, sopsAgeKeyFileContents);
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygenCLI.AddKeyAsync(string, bool, bool, CancellationToken)"/> method writes an age key to the specified file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenValidPath_ShouldWriteKeyToFile()
  {
    // Act
    await AgeKeygenCLI.AddKeyAsync("keys.txt", shouldOverwrite: true);
    string keyContents = await AgeKeygenCLI.ShowKeyAsync("keys.txt");

    // Assert
    Assert.DoesNotContain("Public key:", keyContents);
    Assert.Contains("# created:", keyContents);
    Assert.Contains("# public key:", keyContents);
    Assert.Contains("AGE-SECRET-KEY-", keyContents);

    // Cleanup
    await AgeKeygenCLI.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: false);
    Assert.False(File.Exists("keys.txt"));
  }

  /// <summary>
  /// Tests that the <see cref="AgeKeygenCLI.AddKeyAsync(string, bool, bool, CancellationToken)"/> method writes an age key to the specified file, and adds the key to the sops age key file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task AddKeyAsync_GivenValidPathAndBooleanToAddKeyToSopsAgeKeyFile_ShouldWriteKeyToFileAndAddKeyToSopsAgeKeyFile()
  {
    // Act
    await AgeKeygenCLI.AddKeyAsync("keys.txt", shouldOverwrite: true, addToSopsAgeKeyFile: true);
    string keyContents = await AgeKeygenCLI.ShowKeyAsync("keys.txt");
    string sopsAgeKeyFileContents = await AgeKeygenCLI.ShowSopsAgeKeyFileAsync();

    // Assert
    Assert.DoesNotContain("Public key:", keyContents);
    Assert.Contains("# created:", keyContents);
    Assert.Contains("# public key:", keyContents);
    Assert.Contains("AGE-SECRET-KEY-", keyContents);
    Assert.Contains(keyContents, sopsAgeKeyFileContents);

    // Cleanup
    await AgeKeygenCLI.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: true);
    Assert.False(File.Exists("keys.txt"));
    sopsAgeKeyFileContents = await AgeKeygenCLI.ShowSopsAgeKeyFileAsync();
    Assert.DoesNotContain(keyContents, sopsAgeKeyFileContents);
  }
}
