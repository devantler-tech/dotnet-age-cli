namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

/// <summary>
/// Tests for the <see cref="AgeKeygen.ShowKeyAsync(string, CancellationToken)"/> method.
/// </summary>
public class ShowKeyAsyncTests
{

  /// <summary>
  /// Tests that a <see cref="FileNotFoundException"/> is thrown when the key file does not exist.
  /// </summary>
  [Fact]
  public async Task ShowKeyAsync_GivenNonExistentKeyFile_ShouldThrowFileNotFoundException()
  {
    // Arrange
    string keyPath = "non-existent-key.txt";

    // Act
    async Task Act() => await AgeKeygen.ShowKeyAsync(keyPath, CancellationToken.None).ConfigureAwait(false);

    // Assert
    _ = await Assert.ThrowsAsync<FileNotFoundException>(Act);
  }
}
