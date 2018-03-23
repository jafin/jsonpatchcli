using CommandLine;

namespace JsonPatch
{
    [Verb("patch", HelpText = "Patch a json file")]
    public class PatchOptions
    {
        [Option]
        public string Source { get; set; }
        [Option]
        public string PatchFile { get; set; }
        [Option]
        public string PatchInline { get; set; }

    }
}