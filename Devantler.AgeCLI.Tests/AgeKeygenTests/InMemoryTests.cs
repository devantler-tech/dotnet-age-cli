namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.InMemory"/> method.
/// </summary>
public partial class InMemoryTests
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
    string keyString = key.ToString();

    // Assert
    Assert.NotNull(key);
    Assert.NotEmpty(key.PublicKey);
    Assert.NotEmpty(key.PrivateKey);
    Assert.Equal(keyString, key.ToString());
  }
}
