using System.Globalization;
using System.Text.RegularExpressions;
using Devantler.AgeCLI.Tests.Utils;
using Devantler.Keys.Age;

namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.ToFile"/> method.
/// </summary>
public partial class ToFileTests
{
  [GeneratedRegex(@"(\r\n|\r|\n)")]
  private static partial Regex NewlineRegex();

  /// <summary>
  /// Tests that an age key is written to a file.
  /// </summary>
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
    string keyString = await File.ReadAllTextAsync(path);
    string[] lines = keyString.Split("\n");
    string publicKey = lines[1].Split(" ")[3];
    string privateKey = lines[2];
    var createdAt = DateTime.Parse(lines[0].Split(" ")[2], CultureInfo.InvariantCulture);
    var key = new AgeKey(
      publicKey,
      privateKey,
      createdAt
    );

    // Assert
    Assert.True(File.Exists(path));
    Assert.NotNull(key);
    Assert.NotEmpty(key.PublicKey);
    Assert.NotEmpty(key.PrivateKey);
    Assert.Contains(
      NewlineRegex().Replace($"""
        # created: {DateTimeFormatter.FormatDateTimeWithCustomOffset(key.CreatedAt)}
        # public key: {key.PublicKey}
        {key.PrivateKey}
        """, Environment.NewLine
      ),
      keyString,
      StringComparison.Ordinal
    );
  }

  /// <summary>
  /// Tests that an <see cref="InvalidOperationException"/> is thrown when the age-keygen CLI command fails.
  /// </summary>
  [Fact]
  public async Task ToFile_GivenInvalidOutputPath_ShouldThrowInvalidOperationException()
  {
    // Arrange
    string path = "/dev/null";

    // Act
    async Task Act() => await AgeKeygen.ToFile(path).ConfigureAwait(false);

    // Assert
    _ = await Assert.ThrowsAsync<InvalidOperationException>(Act);
  }
}
