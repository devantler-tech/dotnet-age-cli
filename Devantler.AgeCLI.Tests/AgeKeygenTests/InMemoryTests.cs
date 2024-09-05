using Devantler.Keys.Age.Utils;

namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.InMemory"/> method.
/// </summary>
public class InMemoryTests
{
  /// <summary>
  /// Tests that an age key is returned.
  /// </summary>
  /// <returns></returns>
  [Fact]
  public async Task InMemory_ShouldReturnAgeKey()
  {
    // Act
    var key = await AgeKeygen.InMemory();

    // Assert
    Assert.NotNull(key);
    Assert.NotEmpty(key.PublicKey);
    Assert.NotEmpty(key.PrivateKey);
    Assert.Equal(key.ToString(), $"# created: {DateTimeFormatter.FormatDateTimeWithCustomOffset(key.CreatedAt)}{Environment.NewLine}# public key: {key.PublicKey}{Environment.NewLine}{key.PrivateKey}");
  }
}
