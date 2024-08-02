using System.Runtime.InteropServices;

namespace Devantler.AgeCLI.Tests.AgeKeygenTests;

public class GetCommandTests
{
  /// <summary>
  /// Test to verify that the command returns the correct binary for MacOS on x64 architecture.
  /// </summary>
  [Fact]
  public void Command_ShouldReturnDarwinAmd64Binary()
  {
    // Arrange
    string expectedBinary = "age-keygen-darwin-amd64";

    // Act
    string actualBinary = Path.GetFileName(AgeKeygen.GetCommand(PlatformID.Unix, Architecture.X64, "osx-x64").TargetFilePath);

    // Assert
    Assert.Equal(expectedBinary, actualBinary);
  }

  /// <summary>
  /// Test to verify that the command returns the correct binary for Linux on ARM64 architecture.
  /// </summary>
  [Fact]
  public void Command_ShouldReturnLinuxArm64Binary()
  {
    // Arrange
    string expectedBinary = "age-keygen-linux-arm64";

    // Act
    string actualBinary = Path.GetFileName(AgeKeygen.GetCommand(PlatformID.Unix, Architecture.Arm64, "linux-arm64").TargetFilePath);

    // Assert
    Assert.Equal(expectedBinary, actualBinary);
  }

  /// <summary>
  /// Test to verify that the command returns a <see cref="PlatformNotSupportedException"/> when the platform is not supported.
  /// </summary>
  [Fact]
  public void Command_GivenInvaldiPlatform_ShouldThrowPlatformNotSupportedException()
  {
    // Arrange
    var platformID = PlatformID.Other;
    var architecture = Architecture.Wasm;
    string runtimeIdentifier = "wasm";

    // Act
    void Act() => AgeKeygen.GetCommand(platformID, architecture, runtimeIdentifier);

    // Assert
    Assert.Throws<PlatformNotSupportedException>(Act);
  }
}
