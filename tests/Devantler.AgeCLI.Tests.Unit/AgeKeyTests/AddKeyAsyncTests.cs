namespace Devantler.AgeCLI.Tests.Unit.AgeKeyTests;

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
  public async Task AddKeyAsync_ShouldReturnZeroExitCodeAndAgeKey()
  {
    // Act
    var (exitCode, key) = await AgeCLI.AddKeyAsync();

    // Assert
    Assert.Equal(0, exitCode);
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
  public async Task AddKeyAsync_ShouldReturnZeroExitCodeAndAgeKey_WhenAddingKeyToSopsAgeKeyFile()
  {
    // Act
    var (exitCode, key) = await AgeCLI.AddKeyAsync(addToSopsAgeKeyFile: true);
    string sopsAgeKeyFileContents = await AgeCLI.ShowSopsAgeKeyFileAsync();


    // Assert
    Assert.Equal(0, exitCode);
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
}
