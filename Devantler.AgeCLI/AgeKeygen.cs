using System.Globalization;
using System.Runtime.InteropServices;
using CliWrap;
using Devantler.CLIRunner;
using Devantler.Keys.Age;

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
      (PlatformID.Win32NT, Architecture.X64, "win-x64") => "age-keygen-win-x64.exe",
      _ => throw new PlatformNotSupportedException($"Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
    };
    string binaryPath = Path.Combine(AppContext.BaseDirectory, binary);
    return !File.Exists(binaryPath) ?
      throw new FileNotFoundException($"{binaryPath} not found.") :
      Cli.Wrap(binaryPath);
  }

  /// <summary>
  /// Runs the age-keygen CLI command with the given arguments.
  /// </summary>
  /// <param name="arguments"></param>
  /// <param name="validation"></param>
  /// <param name="silent"></param>
  /// <param name="includeStdErr"></param>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  public static async Task<(int ExitCode, string Message)> RunAsync(
    string[] arguments,
    CommandResultValidation validation = CommandResultValidation.ZeroExitCode,
    bool silent = false,
    bool includeStdErr = true,
    CancellationToken cancellationToken = default)
  {
    return await CLI.RunAsync(
      Command.WithArguments(arguments),
      validation: validation,
      silent: silent,
      includeStdErr: includeStdErr,
      cancellationToken: cancellationToken).ConfigureAwait(false);
  }

  /// <summary>
  /// Generates a new Age key and returns it.
  /// </summary>
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task<AgeKey> InMemory(CancellationToken cancellationToken = default)
  {
    var (exitCode, output) = await RunAsync(
      [],
      silent: true,
      includeStdErr: false,
      cancellationToken: cancellationToken).ConfigureAwait(false);
    if (exitCode != 0)
    {
      throw new InvalidOperationException($"Failed to generate key: {output}");
    }
    string[] lines = output.Split(Environment.NewLine);
    var createdAt = DateTime.Parse(lines[0].Split(" ")[2], CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
    string publicKey = lines[1].Split(" ")[3];
    string privateKey = lines[2];
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
  /// <param name="cancellationToken"></param>
  /// <returns></returns>
  /// <exception cref="InvalidOperationException"></exception>
  [Obsolete("This method is deprecated. Use RunAsync instead.")]
  public static async Task<AgeKey> ToFile(string path, CancellationToken cancellationToken = default)
  {
    var (exitCode, output) = await RunAsync(["-o", path], silent: true, cancellationToken: cancellationToken).ConfigureAwait(false);
    if (exitCode != 0)
    {
      throw new InvalidOperationException($"Failed to generate key: {output}");
    }
    string key = await File.ReadAllTextAsync(path, cancellationToken).ConfigureAwait(false);
    string[] lines = key.Split("\n");
    var createdAt = DateTime.Parse(lines[0].Split(" ")[2], CultureInfo.InvariantCulture);
    string publicKey = lines[1].Split(" ")[3];
    string privateKey = lines[2];
    return new AgeKey(
      publicKey,
      privateKey,
      createdAt
    );
  }
}
