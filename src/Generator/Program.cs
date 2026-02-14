using System;
using System.IO;
using System.Linq;
using ClangSharp;
using ClangSharp.Interop;
using Generator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static ClangSharp.Interop.CXTranslationUnit_Flags;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables("TSK_GENERATOR_")
    .Build();

using ILoggerFactory factory = LoggerFactory
    .Create(builder => builder
        .AddConsole()
        .AddConfiguration(config.GetSection("Logging")));

ILogger logger = factory.CreateLogger(nameof(Program));

var tskConfig = config.GetSection("TskConfig").Get<TskConfig>();
var language = tskConfig.Language;
var std = tskConfig.LanguageStandard;

var options = new PInvokeGeneratorConfiguration(
    language,
    std,
    tskConfig.DefaultNamespace,
    tskConfig.OutputLocation,
    string.Empty,
    PInvokeGeneratorOutputMode.CSharp,
    PInvokeGeneratorConfigurationOptions.GenerateMultipleFiles |
    PInvokeGeneratorConfigurationOptions.GenerateHelperTypes |
    PInvokeGeneratorConfigurationOptions.GenerateTestsXUnit |
    PInvokeGeneratorConfigurationOptions.GenerateLatestCode)
{
    TestOutputLocation = tskConfig.TestOutputLocation,
    ExcludedNames = tskConfig.ExcludeNames,
    RemappedNames = tskConfig.RemappedNames,
    LibraryPath = tskConfig.LibraryPath,
    TraversalNames = tskConfig.TraversalNames.Select(c => Path.Combine(tskConfig.FileDirectory, c)).ToArray(),
};

using var generator = new PInvokeGenerator(options);

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags
const CXTranslationUnit_Flags translationFlags = 
    CXTranslationUnit_None 
    | CXTranslationUnit_IncludeAttributedTypes 
    | CXTranslationUnit_VisitImplicitAttributes;
// ReSharper restore BitwiseOperatorOnEnumWithoutFlags

var clangCommandLineArgs = string.IsNullOrWhiteSpace(std)
    ? new[] {
        $"--language={language}",               // Treat subsequent input files as having type <language>
        "-Wno-pragma-once-outside-header"       // We are processing files which may be header files
    } : [
        $"--language={language}",               // Treat subsequent input files as having type <language>
        $"--std={std}",                         // Language standard to compile for
        "-Wno-pragma-once-outside-header"       // We are processing files which may be header files
    ];

string[] includeDirectories = [
    .. tskConfig.IncludeDirectories, 
    .. OperatingSystem.IsWindows() ? 
        tskConfig.IncludeDirectoriesWindows : 
        tskConfig.IncludeDirectoriesLinux];

clangCommandLineArgs = [.. clangCommandLineArgs, .. includeDirectories.Select(c=> $"--include-directory={c}")];
logger.LogDebug("Clang CLI args {Args}", (object) clangCommandLineArgs);

foreach (var file in tskConfig.Files)
{
    var fullPath = Path.Combine(tskConfig.FileDirectory, file);
    logger.LogInformation("Parsing {FullPath}",fullPath);
    CXTranslationUnit.TryParse(generator.IndexHandle, fullPath, clangCommandLineArgs, [], translationFlags, out var unit);
    using var translationUnit = TranslationUnit.GetOrCreate(unit);
    generator.GenerateBindings(translationUnit, fullPath, clangCommandLineArgs, translationFlags);
}