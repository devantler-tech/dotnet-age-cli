using CliWrap;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Devantler.AgeCLI;

/// <summary>
/// A class to run age-keygen CLI commands.
/// </summary>
public static partial class AgeKeygen
{
  [GeneratedRegex(@"^Public key:.*(\r\n|\r|\n)", RegexOptions.Multiline)]
  private static partial Regex PublicKeyRegex();

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
      (PlatformID.Unix, Architecture.X64, "osx-x64") => "age-keygen-darwin-amd64",
      (PlatformID.Unix, Architecture.Arm64, "osx-arm64") => "age-keygen-darwin-arm64",
      (PlatformID.Unix, Architecture.X64, "linux-x64") => "age-keygen-linux-amd64",
      (PlatformID.Unix, Architecture.Arm64, "linux-arm64") => "age-keygen-linux-arm64",
      (PlatformID.Win32NT, Architecture.X64, "win-x64") => "age-keygen-windows-amd64.exe",
      _ => throw new PlatformNotSupportedException($"Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
    };
    return Cli.Wrap($"{AppContext.BaseDirectory}assets{Path.DirectorySeparatorChar}binaries{Path.DirectorySeparatorChar}{binary}");
  }


  /// <summary>
  /// Add a new key in-memory.
  /// </summary>
  /// <param name="addToSopsAgeKeyFile">Whether to add the key to the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>A tuple containing the exit code and the key.</returns>
  public static async Task<string> AddKeyAsync(bool addToSopsAgeKeyFile = false, CancellationToken token = default) =>
    await AddKeyAsync(Command, addToSopsAgeKeyFile: addToSopsAgeKeyFile, token: token).ConfigureAwait(false);

  /// <summary>
  /// Add a new key and save it to a file.
  /// </summary>
  /// <param name="keyPath">The path to save the key to.</param>
  /// <param name="shouldOverwrite">Whether to overwrite the key if it already exists.</param>
  /// <param name="addToSopsAgeKeyFile">Whether to add the key to the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>An integer representing the exit code.</returns>
  public static async Task AddKeyAsync(string keyPath, bool shouldOverwrite = false, bool addToSopsAgeKeyFile = false, CancellationToken token = default)
  {
    var cmd = Command.WithArguments(["-o", keyPath]);
    if (File.Exists(keyPath) && shouldOverwrite)
    {
      File.Delete(keyPath);
    }
    _ = await AddKeyAsync(cmd, keyPath, addToSopsAgeKeyFile, token).ConfigureAwait(false);
  }

  /// <summary>
  /// Add a new key in-memory.
  /// </summary>
  /// <param name="cmd"></param>
  /// <param name="keyPath"></param>
  /// <param name="addToSopsAgeKeyFile">Whether to add the key to the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>A tuple containing the exit code and the key.</returns>
  public static async Task<string> AddKeyAsync(Command cmd, string keyPath = "", bool addToSopsAgeKeyFile = false, CancellationToken token = default)
  {
    var (exitCode, message) = await CLIRunner.CLIRunner.RunAsync(cmd, token, silent: false).ConfigureAwait(false);
    if (exitCode != 0)
    {
      throw new AgeException($"Failed to generate key: {message}");
    }
    string key = !string.IsNullOrEmpty(keyPath) ?
      await ShowKeyAsync(keyPath, token).ConfigureAwait(false) :
      PublicKeyRegex().Replace(message, string.Empty);
    if (addToSopsAgeKeyFile)
    {
      await SopsAgeKeyFileWriter.AddKeyAsync(key, token).ConfigureAwait(false);
    }
    return key;
  }

  /// <summary>
  /// Removes an existing key.
  /// </summary>
  /// <param name="keyPath">The key to remove.</param>
  /// <param name="removeFromSopsAgeKeyFile">Whether to remove the key from the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  public static async Task RemoveKeyAsync(string keyPath, bool removeFromSopsAgeKeyFile = false, CancellationToken token = default)
  {
    string keyContents = await ShowKeyAsync(keyPath, token).ConfigureAwait(false);
    if (File.Exists(keyPath))
    {
      File.Delete(keyPath);
    }
    if (removeFromSopsAgeKeyFile)
    {
      await RemoveKeyFromSopsAgeKeyFileAsync(keyContents, token).ConfigureAwait(false);
    }
  }

  /// <summary>
  /// Remove a key from the sops age key file.
  /// </summary>
  /// <param name="key"></param>
  /// <param name="token"></param>
  /// <returns></returns>
  public static async Task RemoveKeyFromSopsAgeKeyFileAsync(string key, CancellationToken token = default) =>
    await SopsAgeKeyFileWriter.RemoveKeyAsync(key, token).ConfigureAwait(false);

  /// <summary>
  /// Show a key from a file.
  /// </summary>
  /// <param name="keyPath">The path to the key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns></returns>
  public static async Task<string> ShowKeyAsync(string keyPath, CancellationToken token = default)
  {
    if (!File.Exists(keyPath))
    {
      throw new FileNotFoundException($"The file {keyPath} does not exist.");
    }
    string key = await File.ReadAllTextAsync(keyPath, token).ConfigureAwait(false);
    return key;
  }

  /// <summary>
  /// Show the sops age key file.
  /// </summary>
  /// <param name="token"></param>
  /// <returns></returns>
  public static async Task<string> ShowSopsAgeKeyFileAsync(CancellationToken token = default) =>
    await SopsAgeKeyFileWriter.ReadFileAsync(token).ConfigureAwait(false);
}
