using System;
using System.IO;
using CommandLine;
using Marvin.JsonPatch;
using Newtonsoft.Json.Linq;

namespace JsonPatch
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new Program();
            try
            {
                var result = Parser.Default.ParseArguments<PatchOptions>(args)
                    .MapResult(options => p.Run<int>(options), _ => 1);
                Environment.Exit(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} {ex.StackTrace}");
            }
        }

        public int Run<TResult>(PatchOptions options)
        {
            if (string.IsNullOrEmpty(options.Source))
            {
                Console.WriteLine("You need to specify a Source and Patch file. use --?");
                return 1;
            }

            Console.WriteLine("Merging of Arrays is set to 'Replace'");
            var srcFile = File.ReadAllText(options.Source);
            string patch = "{}";
            if (!string.IsNullOrEmpty(options.PatchFile))
            { patch = File.ReadAllText(options.PatchFile);}
            else if (!string.IsNullOrEmpty(options.PatchInline))
            {
                patch = options.PatchInline;
                Console.WriteLine("PatchInline:");
                Console.WriteLine(patch);
            }
            else
            {
                Console.WriteLine("Either PatchFile or Patchline was not specified.");
                return 5;
            }
            var srcJson = JObject.Parse(srcFile);
            var patchJson = JObject.Parse(patch);
            srcJson.Merge(patchJson, new JsonMergeSettings() {MergeArrayHandling = MergeArrayHandling.Replace});
            File.WriteAllText(options.Source, srcJson.ToString());
            return 0;
        }
    }
}