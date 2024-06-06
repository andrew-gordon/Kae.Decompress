
using SharpCompress.Common;
using SharpCompress.Readers;

namespace GordoSoft.DCompress
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayUsage();
            }

            var fileName = args[0];

            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found: {0}", fileName);
                DisplayUsage();
            }

            var outputFolder = args.Length == 2 ? args[1] : ".";

            if (outputFolder != "." && !Directory.Exists(outputFolder))
            {
                Console.WriteLine("Output folder not found: {0}", fileName);
                DisplayUsage();
            }

            using (Stream stream = File.OpenRead(fileName))
            {
                var reader = ReaderFactory.Open(stream);
                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        Console.WriteLine("Extracting {0} [{1} bytes]", reader.Entry.Key, reader.Entry.Size);
                        reader.WriteEntryToDirectory(outputFolder, new ExtractionOptions { ExtractFullPath = true, Overwrite = true });
                    }
                }
            }
        }

        private static void DisplayUsage()
        {
            Console.WriteLine("DCompress <filename> [<output folder>]");
            Console.WriteLine();
            Console.WriteLine("Notes:");
            Console.WriteLine("  If output folder is not specified, then the archive will be extracted in the current folder.");
            Console.WriteLine("  Supports decompression of RAR and ZIP files.");
            Environment.Exit(1);
        }
    }
}
