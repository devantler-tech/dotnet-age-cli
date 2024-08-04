using System.Globalization;

namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.ToFile"/> method.
/// </summary>
public class ToFileTests
{
  /// <summary>
  /// Tests that an age key is written to a file.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task ToFile_ShouldWriteAgeKeyToFile()
  {
    // Arrange
    string path = "test.key";
    if (File.Exists(path))
    {
      File.Delete(path);
    }

    // Act
    await AgeKeygen.ToFile(path);

    // Assert
    Assert.True(File.Exists(path));
    string keyString = await File.ReadAllTextAsync(path);
    var key = new AgeKey(path);
    Assert.Contains("# created: ", keyString, StringComparison.Ordinal);
    Assert.Contains("# public key: ", keyString, StringComparison.Ordinal);
    Assert.Contains("AGE-SECRET-KEY-", keyString, StringComparison.Ordinal);
    Assert.Contains(key.PublicKey, keyString, StringComparison.Ordinal);
    Assert.Contains(key.PrivateKey, keyString, StringComparison.Ordinal);
  }
}
