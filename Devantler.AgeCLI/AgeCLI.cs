using CliWrap;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Devantler.AgeCLI;

/// <summary>
/// A class to run age CLI commands.
/// </summary>
public static partial class AgeCLI
{
  [GeneratedRegex(@"^Public key:.*(\r\n|\r|\n)", RegexOptions.Multiline)]
  private static partial Regex PublicKeyRegex();
  static Command AgeKeygen
  {
    get
    {
      string binary = (Environment.OSVersion.Platform, RuntimeInformation.ProcessArchitecture, RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) switch
      {
        (PlatformID.Unix, Architecture.X64, true) => "age-keygen-darwin-amd64",
        (PlatformID.Unix, Architecture.Arm64, true) => "age-keygen-darwin-arm64",
        (PlatformID.Unix, Architecture.X64, false) => "age-keygen-linux-amd64",
        (PlatformID.Unix, Architecture.Arm64, false) => "age-keygen-linux-arm64",
        (PlatformID.Win32NT, Architecture.X64, _) => "age-keygen-windows-amd64.exe",
        _ => throw new PlatformNotSupportedException($"Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
      };
      return Cli.Wrap($"{AppContext.BaseDirectory}assets/binaries/{binary}");
    }
  }

  /// <summary>
  /// Add a new key in-memory.
  /// </summary>
  /// <param name="addToSopsAgeKeyFile">Whether to add the key to the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>A tuple containing the exit code and the key.</returns>
  public static async Task<string> AddKeyAsync(bool addToSopsAgeKeyFile = false, CancellationToken token = default)
  {
    var (exitCode, message) = await CLIRunner.CLIRunner.RunAsync(AgeKeygen, token, silent: false);
    if (exitCode != 0)
    {
      throw new AgeCLIException($"Failed to generate key: {message}");
    }
    string key = PublicKeyRegex().Replace(message, string.Empty);
    if (addToSopsAgeKeyFile)
    {
      await SopsAgeKeyFileWriter.AddKeyAsync(key, token);
    }
    return key;
  }

  /// <summary>
  /// Add a new key and save it to a file.
  /// </summary>
  /// <param name="path">The path to save the key to.</param>
  /// <param name="shouldOverwrite">Whether to overwrite the key if it already exists.</param>
  /// <param name="addToSopsAgeKeyFile">Whether to add the key to the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>An integer representing the exit code.</returns>
  public static async Task AddKeyAsync(string path, bool shouldOverwrite = false, bool addToSopsAgeKeyFile = false, CancellationToken token = default)
  {
    var cmd = AgeKeygen.WithArguments(["-o", path]);
    if (File.Exists(path) && shouldOverwrite)
    {
      File.Delete(path);
    }
    var (exitCode, message) = await CLIRunner.CLIRunner.RunAsync(cmd, token, silent: false);
    if (exitCode != 0)
    {
      throw new AgeCLIException($"Failed to generate key: {message}");
    }
    string key = PublicKeyRegex().Replace(message, string.Empty);
    if (addToSopsAgeKeyFile)
    {
      await SopsAgeKeyFileWriter.AddKeyAsync(key, token);
    }
  }

  /// <summary>
  /// Removes an existing key.
  /// </summary>
  /// <param name="key">The key to remove.</param>
  /// <param name="removeFromSopsAgeKeyFile">Whether to remove the key from the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  public static async Task RemoveKeyAsync(string key, bool removeFromSopsAgeKeyFile = false, CancellationToken token = default)
  {
    if (File.Exists(key))
    {
      File.Delete(key);
    }
    if (removeFromSopsAgeKeyFile)
    {
      await SopsAgeKeyFileWriter.RemoveKeyAsync(key, token);
    }
  }

  /// <summary>
  /// Show a key from a file.
  /// </summary>
  /// <param name="path">The path to the key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns></returns>
  public static async Task<string> ShowKeyAsync(string path, CancellationToken token = default)
  {
    if (!File.Exists(path))
    {
      throw new FileNotFoundException($"The file {path} does not exist.");
    }
    string key = await File.ReadAllTextAsync(path, token);
    return key;
  }

  /// <summary>
  /// Show the sops age key file.
  /// </summary>
  /// <param name="token"></param>
  /// <returns></returns>
  public static async Task<string> ShowSopsAgeKeyFileAsync(CancellationToken token = default) =>
    await SopsAgeKeyFileWriter.ReadSopsAgeKeyFileAsync(token);
}
