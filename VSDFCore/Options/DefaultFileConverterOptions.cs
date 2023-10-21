namespace VSDFCore.Options;

public class DefaultFileConverterOptions : IConverterOptions
{
    public int BufferSize { get; set; } = DefaultOptionValues.BufferSize;
    public int MaxPartCount { get; set; } = DefaultOptionValues.MaxPartCount;
    public int MaxConcurrentFiles { get; set; } = DefaultOptionValues.MaxConcurrentFiles;
    public int Width { get; set; } = DefaultOptionValues.Width;
    public int Height { get; set; } = DefaultOptionValues.Height;
    public byte RAlpha { get; set; } = DefaultOptionValues.RAlpha;
    public byte RgAlpha { get; set; } = DefaultOptionValues.RgAlpha;
    public byte RgbAlpha { get; set; } = DefaultOptionValues.RgbAlpha;
    public bool DebugMode { get; set; } = DefaultOptionValues.DebugMode;
}