using System.Collections.Generic;

namespace Generator;

public class TskConfig
{
    public string Language { get; set; }
    
    public string LanguageStandard { get; set; }
    
    public string IncludePath { get; set; }
    
    public string DefaultNamespace { get; set; }
    
    public string OutputLocation { get; set; }
    
    public string TestOutputLocation { get; set; }
    
    public string LibraryPath { get; set; }

    public List<string> TraversalNames { get; set; }
    
    public List<string> Files { get; set; }
    
    public List<string> ExcludeNames { get; set; }
    
    public Dictionary<string, string> RemappedNames { get; set; }
}