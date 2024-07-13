# 🔑 .NET Age CLI

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Test](https://github.com/devantler/dotnet-age-cli/actions/workflows/test.yaml/badge.svg)](https://github.com/devantler/dotnet-age-cli/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler/dotnet-age-cli/graph/badge.svg?token=RhQPb4fE7z)](https://codecov.io/gh/devantler/dotnet-age-cli)

<details>
  <summary>Show/hide folder structure</summary>

<!-- readme-tree start -->

```
.
├── .github
│   ├── scripts
│   └── workflows
├── Devantler.AgeCLI
│   └── assets
│       └── binaries
└── Devantler.AgeCLI.Tests
    └── AgeKeyTests

8 directories
```

<!-- readme-tree end -->

</details>

A simple .NET library that embeds the Age CLI.

## 🚀 Getting Started

To get started, you can install the package from NuGet.

```bash
dotnet add package Devantler.AgeCLI
```

## 📝 Usage

> [!NOTE]
> The library currently only supports the `age-keygen` binary commands, and the `age` binary commands are yet to be implemented.

You can generate an Age key in-memory or save it to a file.

```csharp
using Devantler.AgeCLI;

// Generate an Age key in-memory
string key = await AgeKeygenCLI.AddKeyAsync();

// Generate an Age key in-memory and save it to your SOPS Age key file.
string key = await AgeKeygenCLI.AddKeyAsync(addToSopsAgeKeyFile: true);

// Generate an Age key and save it to a file.
await AgeKeygenCLI.AddKeyAsync("keys.txt");

// Generate an Age key and save it to a file, and add it to your SOPS Age key file.
await AgeKeygenCLI.AddKeyAsync("keys.txt", addToSopsAgeKeyFile: true);
```

You can also remove existing Age keys.

```csharp
using Devantler.AgeCLI;

// Remove an Age key.
await AgeKeygenCLI.RemoveKeyAsync("keys.txt");

// Remove an Age key, and remove it from your SOPS Age key file.
await AgeKeygenCLI.RemoveKeyAsync("keys.txt", removeFromSopsAgeKeyFile: true);

// Remove key from SOPS Age key file.
string key = """
  # created: 2024-07-13T11:03:46+02:00
  # public key: age1yfe7n00tmz280uwvm09qfx8vyg4y7m63e49n5hy5ra8a3dqdgdgszw8tdz
  AGE-SECRET-KEY-1YA0R28P2TM7AWYHA9UL839ZMX30VE2PCEGRJKK2SD6YGFQWVHCTSE3S7NC
  """;
await AgeKeygenCLI.RemoveKeyFromSopsAgeKeyFileAsync(key);
```

You can list existing Age keys.

```csharp
using Devantler.AgeCLI;

// Show an existing Age key.
var key = await AgeKeygenCLI.ShowKeyAsync("keys.txt");

// Show the SOPS Age key file.
var keys = await AgeKeygenCLI.ShowSopsAgeKeyFileAsync();
```
