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
    ├── AgeKeygenTests
    └── Utils

9 directories
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
> The library currently only supports the `age-keygen` binary commands. The `age` binary commands are yet to be implemented.

You can generate an Age key in-memory or save it to a file.

```csharp
using Devantler.AgeCLI;

// Generate an Age key in-memory
string key = await AgeKeygen.InMemoryAsync();

// Generate an Age key and save it to a file.
await AgeKeygen.ToFileAsync("keys.txt");
```
