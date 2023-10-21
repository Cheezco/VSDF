using CommandLine;
using VSDFCore;
using VSDFCore.Options;

namespace VSDFConverter.Options;

public class FileConverterOptions : IConverterOptions
{
    [Option("bufferSize", Default = DefaultOptionValues.BufferSize, Required = false)]
    public int BufferSize { get; set; }

    [Option("maxPartCount", Default = DefaultOptionValues.MaxPartCount, Required = false)]
    public int MaxPartCount { get; set; }

    [Option("maxConcurrentFiles", Default = DefaultOptionValues.MaxConcurrentFiles, Required = false)]
    public int MaxConcurrentFiles { get; set; }

    [Option("width", Default = DefaultOptionValues.Width, HelpText = "Image width. All parts have same width",
        Required = false)]
    public int Width { get; set; }

    [Option("height", Default = DefaultOptionValues.Height, HelpText = "Image height. All parts have same height",
        Required = false)]
    public int Height { get; set; }

    [Option("rAlpha", Default = DefaultOptionValues.RAlpha,
        HelpText = "Alpha value when only R component contains information", Required = false)]
    public byte RAlpha { get; set; }

    [Option("rgAlpha", Default = DefaultOptionValues.RgAlpha,
        HelpText = "Alpha value when R and G components contain information", Required = false)]
    public byte RgAlpha { get; set; }

    [Option("rgbAlpha", Default = DefaultOptionValues.RgbAlpha,
        HelpText = "Alpha value when all components contain information", Required = false)]
    public byte RgbAlpha { get; set; }

    [Option("debug", Default = DefaultOptionValues.DebugMode, HelpText = "Debug mode", Required = false)]

    public bool DebugMode { get; set; }

    [Option('o', "op", Default = Operation.ToVsdf, HelpText = "Operation", Required = true)]
    public Operation Operation { get; set; }

    [Value(0, HelpText = "Input file", Required = true)]
    public string InputFile { get; set; } = string.Empty;

    [Value(1, HelpText = "Output directory", Required = true)]
    public string OutputDir { get; set; } = string.Empty;
}