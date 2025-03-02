# 🔑 .NET Age CLI

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Test](https://github.com/devantler-tech/dotnet-age-cli/actions/workflows/test.yaml/badge.svg)](https://github.com/devantler-tech/dotnet-age-cli/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler-tech/dotnet-age-cli/graph/badge.svg?token=RhQPb4fE7z)](https://codecov.io/gh/devantler-tech/dotnet-age-cli)

<details>
  <summary>Show/hide folder structure</summary>

<!-- readme-tree start -->
```
.
├── .github
│   ├── scripts
│   └── workflows
├── src
│   └── Devantler.AgeCLI
│       └── runtimes
│           ├── linux-arm64
│           │   └── native
│           ├── linux-x64
│           │   └── native
│           ├── osx-arm64
│           │   └── native
│           ├── osx-x64
│           │   └── native
│           └── win-x64
│               └── native
│                   └── age
└── tests
    └── Devantler.AgeCLI.Tests
        └── AgeKeygenTests

21 directories
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

```csharp
using Devantler.AgeCLI;

var (exitCode, output) = await AgeKeygen.RunAsync(["arg1", "arg2"]);
```
