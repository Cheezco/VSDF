using Spectre.Console;
using VSDFConverter.Options;
using VSDFCore;

namespace VSDFConverter;

public class FileConverter
{
    public static void Run(FileConverterOptions options)
    {
        var logger = MiscUtils.CreateLogger();
        try
        {
            var converter = new Converter(logger: logger);

            AnsiConsole.Status()
                .Start("Running...", _ =>
                {
                    switch (options.Operation)
                    {
                        case Operation.ToVsdf:
                            converter.ToVsdf(options.InputFile, options.OutputDir);
                            break;
                        case Operation.FromVsdf:
                            converter.FromVsdf(options.InputFile, options.OutputDir);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                });
        }
        catch (Exception)
        {
            logger.Fatal("Failed to convert file");
            Console.ReadKey();
        }
    }
}