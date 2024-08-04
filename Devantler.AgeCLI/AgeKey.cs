using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Devantler.AgeCLI;

/// <summary>
/// A class to represent an Age key.
/// </summary>
public class AgeKey()
{
  /// <summary>
  /// Creates a new instance of the <see cref="AgeKey"/> class.
  /// </summary>
  /// <param name="path"></param>
  [SetsRequiredMembers]
  public AgeKey(string path) : this()
  {
    string keyString = File.ReadAllText(path);
    string[] lines = keyString.Split("\n");
    CreatedAt = DateTime.Parse(lines[0].Split(" ")[2], CultureInfo.InvariantCulture);
    PublicKey = lines[1].Split(" ")[2];
    PrivateKey = lines[2];
  }

  /// <summary>
  /// Creates a new instance of the <see cref="AgeKey"/> class.
  /// </summary>
  /// <param name="createdAt"></param>
  /// <param name="publicKey"></param>
  /// <param name="privateKey"></param>
  [SetsRequiredMembers]
  public AgeKey(DateTime createdAt, string publicKey, string privateKey) : this()
  {
    CreatedAt = createdAt;
    PublicKey = publicKey;
    PrivateKey = privateKey;
  }

  /// <summary>
  /// The date and time the key was created.
  /// </summary>
  public required DateTime CreatedAt { get; set; }
  /// <summary>
  /// The public key.
  /// </summary>
  public required string PublicKey { get; set; }
  /// <summary>
  /// The private key.
  /// </summary>
  public required string PrivateKey { get; set; }

  /// <summary>
  /// Prints the key in the Age format.
  /// </summary>
  /// <returns></returns>
  public override string ToString()
  {
    // I need the date in this format 2021-01-02T15:30:45+01:00
    return $"""
    # created: {CreatedAt.ToString("yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture)}
    # public key: {PublicKey}
    {PrivateKey}
    """;
  }
}
