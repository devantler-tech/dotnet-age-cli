using System.Runtime.InteropServices;

static class SopsAgeKeyFileWriter
{
  internal static async Task AddKeyAsync(string key, CancellationToken token = default)
  {
    string sopsAgeKeyFile = Environment.GetEnvironmentVariable("SOPS_AGE_KEY_FILE") ?? "";
    if (!string.IsNullOrWhiteSpace(sopsAgeKeyFile))
    {
      await WriteKeyAsync(sopsAgeKeyFile, key, token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      sopsAgeKeyFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Application Support/sops/age/keys.txt";
      await WriteKeyAsync(sopsAgeKeyFile, key, token);

    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
      string xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config";
      sopsAgeKeyFile = $"{xdgConfigHome}/sops/age/keys.txt";
      await WriteKeyAsync(sopsAgeKeyFile, key, token);

    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      sopsAgeKeyFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/sops/age/keys.txt";
      await WriteKeyAsync(sopsAgeKeyFile, key, token);
    }
  }

  internal static async Task RemoveKeyAsync(string key, CancellationToken token = default)
  {
    string sopsAgeKeyFile = Environment.GetEnvironmentVariable("SOPS_AGE_KEY_FILE") ?? "";
    if (!string.IsNullOrWhiteSpace(sopsAgeKeyFile))
    {
      await RemoveKeyFromFileAsync(sopsAgeKeyFile, key, token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
      sopsAgeKeyFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/Library/Application Support/sops/age/keys.txt";
      await RemoveKeyFromFileAsync(sopsAgeKeyFile, key, token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
      string xdgConfigHome = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME") ?? $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}/.config";
      sopsAgeKeyFile = $"{xdgConfigHome}/sops/age/keys.txt";
      await RemoveKeyFromFileAsync(sopsAgeKeyFile, key, token);
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
      sopsAgeKeyFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/sops/age/keys.txt";
      await RemoveKeyFromFileAsync(sopsAgeKeyFile, key, token);
    }
  }

  static async Task RemoveKeyFromFileAsync(string sopsAgeKeyFile, string key, CancellationToken token)
  {
    if (!string.IsNullOrWhiteSpace(sopsAgeKeyFile) && File.Exists(sopsAgeKeyFile))
    {
      string fileContents = await File.ReadAllTextAsync(sopsAgeKeyFile, token);
      if (fileContents.Contains(key))
      {
        fileContents = fileContents.Replace(key, "");
        await File.WriteAllTextAsync(sopsAgeKeyFile, fileContents, token);
      }
    }
  }

  static async Task WriteKeyAsync(string sopsAgeKeyFile, string key, CancellationToken token)
  {
    if (!string.IsNullOrWhiteSpace(sopsAgeKeyFile))
    {
      string? directory = Path.GetDirectoryName(sopsAgeKeyFile);
      if (directory is not null && !Directory.Exists(directory))
      {
        _ = Directory.CreateDirectory(directory);
      }
      if (!File.Exists(sopsAgeKeyFile))
      {
        _ = File.Create(sopsAgeKeyFile);
      }
      string fileContents = await File.ReadAllTextAsync(sopsAgeKeyFile, token);
      if (!fileContents.Contains(key))
      {
        await File.AppendAllTextAsync(sopsAgeKeyFile, key, token);
      }
    }
  }
}

