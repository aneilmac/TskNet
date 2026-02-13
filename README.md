# TskNet

.NET bindings for [Sleuthkit][TSK].

## Generating FFI bindings

FFI bindings are generated via the [ClangSharpPInvokeGenerator][ClangSharpPInvokeGenerator] tool.

You can install this tool by running 

```bash
dotnet tool restore
```

You can then generate the FFI bindings by running

```bash
dotnet tool run ClangSharpPInvokeGenerator "@generate.rsp"
````

## 📜 Licensing

The contents of the repository are "Copyright (c) 2026 aneilmac".

Source code: Licensed under the [MIT License][license].

The library requires usage of [Sleuthkit][TSK] which
is distributed under several licences [described here][TSKLicences].

---

[license]: LICENSE "Project License"
[ClangSharpPInvokeGenerator]: https://github.com/dotnet/ClangSharp "ClangSharp PInvoke Generator"
[TSK]: https://sleuthkit.org/sleuthkit/ "sleuthkit.org"
[TSKLicences]: https://github.com/sleuthkit/sleuthkit/tree/develop/licenses "Sleuthkit licences"