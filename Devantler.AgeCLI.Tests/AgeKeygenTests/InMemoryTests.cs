using System.Globalization;
using System.Text.RegularExpressions;

namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.InMemory"/> method.
/// </summary>
public partial class InMemoryTests
{
  [GeneratedRegex(@"(\r\n|\r|\n)")]
  private static partial Regex NewlineRegex();

  /// <summary>
  /// Tests that an age key is returned.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task InMemory_ShouldReturnAgeKey()
  {
    // Act
    var key = await AgeKeygen.InMemory();
    string keyString = key.ToString();

    // Assert
    Assert.NotNull(key);
    Assert.NotEmpty(key.PublicKey);
    Assert.NotEmpty(key.PrivateKey);
    Assert.Contains(
      NewlineRegex().Replace($"""
        # created: {key.CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}
        # public key: {key.PublicKey}
        {key.PrivateKey}
        """, Environment.NewLine
      ),
      keyString,
      StringComparison.Ordinal
    );
  }
}
