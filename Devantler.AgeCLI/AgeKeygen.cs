using CliWrap;
using Devantler.CLIRunner;
using Devantler.Keys.Age;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Devantler.AgeCLI;

/// <summary>
/// A class to run age-keygen CLI commands.
/// </summary>
public static class AgeKeygen
{
  /// <summary>
  /// The age-keygen CLI command.
  /// </summary>
  static Command Command => GetCommand();

  internal static Command GetCommand(PlatformID? platformID = default, Architecture? architecture = default, string? runtimeIdentifier = default)
  {
    platformID ??= Environment.OSVersion.Platform;
    architecture ??= RuntimeInformation.ProcessArchitecture;
    runtimeIdentifier ??= RuntimeInformation.RuntimeIdentifier;

    string binary = (platformID, architecture, runtimeIdentifier) switch
    {
      (PlatformID.Unix, Architecture.X64, "osx-x64") => "age-keygen-osx-x64",
      (PlatformID.Unix, Architecture.Arm64, "osx-arm64") => "age-keygen-osx-arm64",
      (PlatformID.Unix, Architecture.X64, "linux-x64") => "age-keygen-linux-x64",
      (PlatformID.Unix, Architecture.Arm64, "linux-arm64") => "age-keygen-linux-arm64",
      (PlatformID.Win32NT, Architecture.X64, "win-x64") => "age-keygen-windows-x64.exe",
      _ => throw new PlatformNotSupportedException($"Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
    };
    return Cli.Wrap($"{AppContext.BaseDirectory}/runtimes/{runtimeIdentifier}/native/{binary}");
  }

  /// <summary>
  /// Generates a new Age key and returns it.
  /// </summary>
  /// <param name="token"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public static async Task<AgeKey> InMemory(CancellationToken token = default)
  {
    var (exitCode, message) = await CLI.RunAsync(Command, silent: true, includeStdErr: false, cancellationToken: token).ConfigureAwait(false);
    if (exitCode != 0)
    {
      throw new InvalidOperationException($"Failed to generate key: {message}");
    }
    string[] lines = message.Split(Environment.NewLine);
    string publicKey = lines[1].Split(" ")[3];
    string privateKey = lines[2];
    var createdAt = DateTime.Parse(lines[0].Split(" ")[2], CultureInfo.InvariantCulture);
    var key = new AgeKey(
      publicKey,
      privateKey,
      createdAt
    );
    return key;
  }

  /// <summary>
  /// Generates a new Age key and writes it to a file.
  /// </summary>
  /// <param name="path"></param>
  /// <param name="token"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  public static async Task ToFile(string path, CancellationToken token = default)
  {
    var (exitCode, message) = await CLI.RunAsync(Command.WithArguments(["-o", path]), silent: true, cancellationToken: token).ConfigureAwait(false);
    if (exitCode != 0)
    {
      throw new InvalidOperationException($"Failed to generate key: {message}");
    }
  }
}
