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
        _ => throw new PlatformNotSupportedException($"🚨 Unsupported platform: {Environment.OSVersion.Platform} {RuntimeInformation.ProcessArchitecture}"),
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
  public static async Task<(int, string)> AddKeyAsync(bool addToSopsAgeKeyFile = false, CancellationToken token = default)
  {
    var (exitCode, key) = await CLIRunner.CLIRunner.RunAsync(AgeKeygen, token, silent: false);
    key = PublicKeyRegex().Replace(key, string.Empty);
    if (exitCode != 0)
    {
      return (exitCode, key);
    }
    if (addToSopsAgeKeyFile)
    {
      await SopsAgeKeyFileWriter.AddKeyAsync(key, token);
    }
    return (exitCode, key);
  }

  /// <summary>
  /// Add a new key and save it to a file.
  /// </summary>
  /// <param name="path">The path to save the key to.</param>
  /// <param name="shouldOverwrite">Whether to overwrite the key if it already exists.</param>
  /// <param name="addToSopsAgeKeyFile">Whether to add the key to the sops age key file.</param>
  /// <param name="token">The cancellation token.</param>
  /// <returns>An integer representing the exit code.</returns>
  public static async Task<int> AddKeyAsync(string path, bool shouldOverwrite = false, bool addToSopsAgeKeyFile = false, CancellationToken token = default)
  {
    var cmd = AgeKeygen.WithArguments(["-o", path]);
    if (File.Exists(path) && shouldOverwrite)
    {
      File.Delete(path);
    }
    var (exitCode, key) = await CLIRunner.CLIRunner.RunAsync(cmd, token, silent: false);
    key = PublicKeyRegex().Replace(key, string.Empty);
    if (exitCode != 0)
    {
      return exitCode;
    }
    if (addToSopsAgeKeyFile)
    {
      await SopsAgeKeyFileWriter.AddKeyAsync(key, token);
    }
    return exitCode;
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
  public static async Task<(int, string)> ShowKeyAsync(string path, CancellationToken token = default)
  {
    if (!File.Exists(path))
    {
      string result = $"Key '{path}' not found";
      return (1, result);
    }
    string key = await File.ReadAllTextAsync(path, token);
    return (0, key);
  }

  public static async Task<string> ShowSopsAgeKeyFileAsync(CancellationToken token = default)
  {
    string sopsAgeKeyFileContents = "";
    string sopsAgeKeyFile = Environment.GetEnvironmentVariable("SOPS_AGE_KEY_FILE") ?? "";
    if (!string.IsNullOrWhiteSpace(sopsAgeKeyFile))
    {
      sopsAgeKeyFileContents = await File.ReadAllTextAsync(sopsAgeKeyFile, token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      sopsAgeKeyFileContents = await File.ReadAllTextAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Application Support/sops/age/keys.txt", token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
      string xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config";
      sopsAgeKeyFileContents = await File.ReadAllTextAsync($"{xdgConfigHome}/sops/age/keys.txt", token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      sopsAgeKeyFileContents = await File.ReadAllTextAsync($"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/sops/age/keys.txt", token);
    }

    return sopsAgeKeyFileContents;
  }
}
