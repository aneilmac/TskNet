# TskNet

.NET bindings for [Sleuthkit][TSK].

## Generating FFI bindings

FFI bindings are generated via [ClangSharp][ClangSharp].

```bash
dotnet run --project .\src\Generator\Generator.csproj
```

## 📜 Licensing

The contents of the repository are "Copyright (c) 2026 aneilmac".

Source code: Licensed under the [MIT License][license].

The library requires usage of [Sleuthkit][TSK] which
is distributed under several licences [described here][TSKLicences].

---

[license]: LICENSE "Project License"
[ClangSharp]: https://github.com/dotnet/ClangSharp "ClangSharp"
[TSK]: https://sleuthkit.org/sleuthkit/ "sleuthkit.org"
[TSKLicences]: https://github.com/sleuthkit/sleuthkit/tree/develop/licenses "Sleuthkit licences"